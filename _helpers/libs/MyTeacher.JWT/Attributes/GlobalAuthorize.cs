using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyTeacher.Helper.Models;
using MyTeacher.Helper.Responses;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyTeacher.Helper.Attributes
{
    public class GlobalAuthorize(params string[]? allowedRoles) : Attribute, IAuthorizationFilter
    {
        private string[] AllowedRoles { get; set; } = allowedRoles ?? [];
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var jwtToken = context.HttpContext.Request.Headers.FirstOrDefault(h => h.Key == "Authorization").Value;
            if (string.IsNullOrEmpty(jwtToken))
            {
                var response = new BaseResponse<string>()
                {
                    ErrorMessage = "You are not allowed here",
                    IsSuccessful = false,
                    SuccessMessage = null
                };
                context.Result = new ContentResult()
                {
                    Content = JsonSerializer.Serialize(response),
                    ContentType = MediaTypeNames.Application.Json,
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }
            var jwtSettings = context.HttpContext.RequestServices.GetRequiredService<JwtTokenSettings>();
            (string? errorMessage, UserModel? userClaims) = ValidateToken(jwtToken.ToString().Split(" ")[1], jwtSettings);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                var response = new BaseResponse<string>()
                {
                    ErrorMessage = errorMessage,
                    IsSuccessful = false,
                    SuccessMessage = null
                };
                context.Result = new ContentResult()
                {
                    Content = JsonSerializer.Serialize(response),
                    ContentType = MediaTypeNames.Application.Json,
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }
            if (AllowedRoles.Length > 1)
            {
                var isRoleExist = AllowedRoles.Any(r => r.Equals(userClaims?.Role));
                if (!isRoleExist)
                {
                    var response = new BaseResponse<string>()
                    {
                        ErrorMessage = "Your role is not allowed",
                        IsSuccessful = false,
                        SuccessMessage = null
                    };
                    context.Result = new ContentResult()
                    {
                        Content = JsonSerializer.Serialize(response),
                        ContentType = MediaTypeNames.Application.Json,
                        StatusCode = StatusCodes.Status403Forbidden
                    };
                    return;
                }
            }
            var userModel = context.HttpContext.RequestServices.GetRequiredService<UserModel>();
            userModel.Username = userClaims?.Username;
            userModel.Role = userClaims?.Role;
            userModel.Email = userClaims?.Email;
            userModel.Id = userClaims?.Id;
        }
        private static (string?, UserModel?) ValidateToken(string authToken, JwtTokenSettings jwtSettings)
        {
            try
            {
                var jwtHandler = new JwtSecurityTokenHandler()
                {
                    MapInboundClaims = jwtSettings.MapInboundClaims
                };
                IPrincipal claims = jwtHandler.ValidateToken(
                    authToken,
                    new TokenValidationParameters()
                    {
                        ValidateLifetime = jwtSettings.ValidateLifetime, // Because there is no expiration in the generated token
                        ValidateAudience = jwtSettings.ValidateAudience, // Because there is no audiance in the generated token
                        ValidateIssuer = jwtSettings.ValidateIssuer,   // Because there is no issuer in the generated token
                        ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                        ClockSkew = TimeSpan.Zero,
                        RequireSignedTokens = jwtSettings.RequireSignedTokens,
                        RequireExpirationTime = jwtSettings.RequireExpirationTime,
                        RequireAudience = jwtSettings.RequireAudience,
                        SaveSigninToken = jwtSettings.SaveSigninToken,
                        NameClaimType = jwtSettings.TokenClaimTypes.Username,
                        RoleClaimType = jwtSettings.TokenClaimTypes.Role,
                        LogValidationExceptions = jwtSettings.LogValidationExceptions,
                        ValidTypes = new List<string> { JwtRegisteredClaimNames.Typ },
                        ValidAlgorithms = new List<string> { SecurityAlgorithms.HmacSha512 },
                        ValidIssuers = jwtSettings.ValidIssuers,
                        ValidAudiences = jwtSettings.ValidAudiences,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.IssuerSigningKey)) // The same key as the one that generate the token
                    },
                    //out SecurityToken validatedToken
                    out _
                );

                var identity = (ClaimsIdentity)claims.Identity;
                if (identity == null) return ("You have wrong token type", null);
                return (null,
                    UserModel.BuildUserModel(
                        identity.Claims.FirstOrDefault(c => c.Type == jwtSettings.TokenClaimTypes.Sub)?.Value,
                        identity.Claims.FirstOrDefault(c => c.Type == jwtSettings.TokenClaimTypes.Username)?.Value,
                        identity.Claims.FirstOrDefault(c => c.Type == jwtSettings.TokenClaimTypes.Email)?.Value,
                        identity.Claims.FirstOrDefault(c => c.Type == jwtSettings.TokenClaimTypes.Role)?.Value
                    )
                );
            }
            catch (SecurityTokenException ex)
            {
                return (ex.Message, null);
            }
            catch (Exception ex)
            {
                return (ex.Message, null);
            }
        }
    }
}
