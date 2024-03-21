using CoreService.Application.Models;
using CoreService.Application.Repositories;
using CoreService.Domain.Entities.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyTeacher.Helper.Models;
using MyTeacher.JWT.Abstracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Features.Commands.User.Login
{
    public class LoginCommandHandler(IUnitOfWork unitOfWork, ILogger<LoginCommandHandler> logger, ITokenGenerator tokenGenerator) : IRequestHandler<LoginCommandRequest, LoginCommandResponse>
    {
        private readonly ILogger<LoginCommandHandler> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITokenGenerator _tokenGenerator = tokenGenerator;
        public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
        {
            var response = new LoginCommandResponse();
            try
            {
                var foundUser = await _unitOfWork.UserReadRepository.FindByCondition(u => u.Username == request.Username).FirstOrDefaultAsync(cancellationToken);

                if (foundUser == null)
                {
                    response.IsSuccessful = false;
                    response.ErrorMessage = "There is no user with given username";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                if (foundUser.PasswordHash != UserEntity.ComputeHash(request.Password, foundUser.PasswordSalt))
                {
                    response.IsSuccessful = false;
                    response.ErrorMessage = "Your password is wrong";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                response.Payload.AccessToken = _tokenGenerator.GenerateJwtToken(foundUser.Id.ToString(), foundUser.Username, foundUser.Email, "admin", TimeSpan.FromSeconds(300));
                response.Payload.RefreshToken = _tokenGenerator.GenerateJwtToken(foundUser.Id.ToString(), foundUser.Username, foundUser.Email, "admin", TimeSpan.FromSeconds(600));
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred while you're attempting to login. Error: {@Error}", ex);
                response.IsSuccessful = false;
                response.ErrorMessage = $"An unexpected error occurred while you're attempting to login. Error: {ex.Message}";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return response;
        }
    }
}
