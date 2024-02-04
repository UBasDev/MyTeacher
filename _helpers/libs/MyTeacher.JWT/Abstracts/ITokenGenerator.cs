using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTeacher.JWT.Abstracts
{
    public interface ITokenGenerator
    {
        string GenerateJwtToken(string userId, string username, string email, string role, TimeSpan expireTime);
    }
}
