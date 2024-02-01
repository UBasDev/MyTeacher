using CoreService.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoreService.Application.Middlewares
{
    public class JwtMiddleware(RequestDelegate next, UserModel userModel)
    {
        private readonly RequestDelegate _next = next;
        private readonly UserModel _userModel = userModel;
        public async Task InvokeAsync(HttpContext context)
        {
            var jwtToken = context.Request.Headers.FirstOrDefault(h => h.Key == "Authorization").Value;
            if (!string.IsNullOrEmpty(jwtToken))
            {
                var isValid = ValidateToken(jwtToken.ToString().Split(" ")[1]);
            }
            _userModel.Username = "test1";
            _userModel.Email = "test1@gmail.com";
            await _next(context);
        }
        private static bool ValidateToken(string authToken)
        {
            var validationParameters = GetValidationParameters();

            try
            {
                IPrincipal claims = new JwtSecurityTokenHandler().ValidateToken(authToken, validationParameters, out SecurityToken validatedToken);
            }
            catch (SecurityTokenException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = true, // Because there is no expiration in the generated token
                ValidateAudience = true, // Because there is no audiance in the generated token
                ValidateIssuer = true,   // Because there is no issuer in the generated token
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                RequireSignedTokens = true,
                RequireExpirationTime = true,
                RequireAudience = true,
                SaveSigninToken = true,
                NameClaimType = "username",
                LogValidationExceptions = true,
                ValidTypes = new List<string> { JwtRegisteredClaimNames.Typ },
                ValidAlgorithms = new List<string> { SecurityAlgorithms.HmacSha512 },
                ValidIssuers = new List<string> { "https://localhost:7186" },
                ValidAudiences = new List<string> { "http://localhost:4200", "https://localhost:7186" },
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret-key-1-secret-key-2-secret-key-3-secret-key-4-secret-key-5")) // The same key as the one that generate the token
            };
        }
        private static byte[] ParseBase64WithoutPadding1(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
