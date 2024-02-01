using CoreService.Application.Models;
using CoreService.Application.Repositories;
using CoreService.Domain.Entities.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Features.Commands.User.Login
{
    public class LoginCommandHandler(IUnitOfWork unitOfWork, ILogger<LoginCommandHandler> logger, UserModel userModel, IHttpContextAccessor httpContextAccessor) : IRequestHandler<LoginCommandRequest, LoginCommandResponse>
    {
        private readonly ILogger<LoginCommandHandler> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserModel _userModel = userModel;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
        {
            var response = new LoginCommandResponse();
            var foundUser = await _unitOfWork.UserReadRepository.FindByCondition(u => u.Username == request.Username).FirstOrDefaultAsync(cancellationToken);

            if (foundUser == null)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "There is no user with given username";
                _httpContextAccessor.HttpContext.Response.StatusCode = 400;
                return response;
            }
            if (foundUser.PasswordHash != UserEntity.ComputeHash(request.Password, foundUser.PasswordSalt))
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Your password is wrong";
                _httpContextAccessor.HttpContext.Response.StatusCode = 400;
                return response;
            }
            response.SuccessMessage.AccessToken = GenerateJwtToken(foundUser.Id.ToString(), foundUser.Username, foundUser.Email, TimeSpan.FromSeconds(30));
            response.SuccessMessage.RefreshToken = GenerateJwtToken(foundUser.Id.ToString(), foundUser.Username, foundUser.Email, TimeSpan.FromSeconds(60));
            var x1 = _userModel;
            return response;
        }
        private string GenerateJwtToken(string userId, string username, string email, TimeSpan expireTime)
        {
            // Set your secret key for signing the token
            var secretKey = "secret-key-1-secret-key-2-secret-key-3-secret-key-4-secret-key-5";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // Set signing credentials using the secret key
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            // Set the claims to be included in the token
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, "admin"),
            new Claim("username", username)
        };

            // Set the token descriptor with required information
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(expireTime), // Set expiration time
                SigningCredentials = credentials,
                Issuer = "https://localhost:7186",
                Audience = "https://localhost:7186",
                IssuedAt = DateTime.UtcNow,
                TokenType = JwtRegisteredClaimNames.Typ,
                NotBefore = DateTime.UtcNow,

            };

            // Create the token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // Generate the token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Write the token as a string
            var jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
