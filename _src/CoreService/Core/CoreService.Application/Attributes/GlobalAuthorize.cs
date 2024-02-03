
using CoreService.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyTeacher.Helper.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;


namespace CoreService.Application.Attributes
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
            (string? errorMessage, UserModel? userClaims) = ValidateToken(jwtToken.ToString().Split(" ")[1]);
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
        private static (string?, UserModel?) ValidateToken(string authToken)
        {
            try
            {
                var jwtHandler = new JwtSecurityTokenHandler()
                {
                    MapInboundClaims = false
                };
                IPrincipal claims = jwtHandler.ValidateToken(
                    authToken,
                    new TokenValidationParameters()
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
                        RoleClaimType = "role",
                        LogValidationExceptions = true,
                        ValidTypes = new List<string> { JwtRegisteredClaimNames.Typ },
                        ValidAlgorithms = new List<string> { SecurityAlgorithms.HmacSha512 },
                        ValidIssuers = new List<string> { "https://localhost:7186" },
                        ValidAudiences = new List<string> { "http://localhost:4200", "https://localhost:7186" },
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret-key-1-secret-key-2-secret-key-3-secret-key-4-secret-key-5")) // The same key as the one that generate the token
                    },
                    //out SecurityToken validatedToken
                    out _
                );

                var identity = (ClaimsIdentity)claims.Identity;
                if (identity == null) return ("You have wrong token type", null);
                return (null,
                    UserModel.BuildUserModel(
                        identity.Claims.FirstOrDefault(c => c.Type == "sub")?.Value,
                        identity.Claims.FirstOrDefault(c => c.Type == "username")?.Value,
                        identity.Claims.FirstOrDefault(c => c.Type == "email")?.Value,
                        identity.Claims.FirstOrDefault(c => c.Type == "role")?.Value
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
