using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.Entities.User
{
    public interface IUserEntityActions
    {
        public static UserEntity CreateSingleUser(string username, string email, string passwordSalt, string passwordHash)
        {
            return new UserEntity(username, email, passwordSalt, passwordHash);
        }
    }
}
