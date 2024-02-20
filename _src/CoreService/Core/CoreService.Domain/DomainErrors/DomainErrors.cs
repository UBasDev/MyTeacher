using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.DomainErrors
{
    public static class DomainErrors
    {
        public static class UserEntityErrors
        {
            public static string NotFound = "This user not found.";
            public static string InvalidRoles = "Your role is not allowed.";
            public static string DuplicateUsername = "This username is already in use.";
        }
    }
}
