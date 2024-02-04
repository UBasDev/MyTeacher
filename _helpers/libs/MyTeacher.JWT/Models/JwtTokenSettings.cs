using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTeacher.Helper.Models
{
    public class JwtTokenSettings
    {
        public bool MapInboundClaims { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public bool RequireSignedTokens { get; set; }
        public bool RequireExpirationTime { get; set; }
        public bool RequireAudience { get; set; }
        public bool SaveSigninToken { get; set; }
        public bool LogValidationExceptions { get; set; }
        public string[] ValidIssuers { get; set; }
        public string[] ValidAudiences { get; set; }
        public string IssuerSigningKey { get; set; }
        public TokenClaimTypes TokenClaimTypes { get; set; }
        public string TokenGeneratedFromIssuer { get; set; }
        public string TokenGeneratedForAudience { get; set; }
    }
    public class TokenClaimTypes
    {
        public string Sub { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
