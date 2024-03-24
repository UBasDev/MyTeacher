using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Constants
{
    public static class ApplicationConstants
    {
        public static readonly string FixedRateLimitingPolicyName = "fixedRateLimitPolicy";
        public static readonly string AllowOnlyLocalCorsPolicyName = "allowOnlyLocalCorsPolicy";
    }
}