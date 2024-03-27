using Microsoft.IdentityModel.Tokens;
using MyTeacher.Helper.Models;
using MyTeacher.JWT.Abstracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyTeacher.JWT.TokenGenerator
{
    public class TokenGenerator(JwtTokenSettings jwtTokenSettings) : ITokenGenerator
    {
        private readonly JwtTokenSettings _jwtTokenSettings = jwtTokenSettings;

        public string GenerateJwtToken(string userId, string username, string email, string role, TimeSpan expireTime)
        {
            // Set the token descriptor with required information
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                        {
                            new Claim(_jwtTokenSettings.TokenClaimTypes.Sub, userId),
                            new Claim(_jwtTokenSettings.TokenClaimTypes.Email, email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(ClaimTypes.Role, role),
                            new Claim(_jwtTokenSettings.TokenClaimTypes.Username, username)
                        }
                ),
                Expires = DateTime.UtcNow.Add(expireTime), // Set expiration time
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenSettings.IssuerSigningKey)),
                    SecurityAlgorithms.HmacSha512
                ),
                Issuer = _jwtTokenSettings.TokenGeneratedFromIssuer,
                Audience = _jwtTokenSettings.TokenGeneratedForAudience,
                IssuedAt = DateTime.UtcNow,
                TokenType = JwtRegisteredClaimNames.Typ,
                NotBefore = DateTime.UtcNow,
            };

            // Create the token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // Write the token as a string
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}