using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTeacher.Helper.Models
{
    public class UserModel
    {
        public UserModel() { }
        private UserModel(string id, string username, string email, string role)
        {
            Id = id;
            Username = username;
            Email = email;
            Role = role;
        }
        public string Id { get; private set; } = string.Empty;
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string Role { get; private set; }
        public static UserModel BuildUserModel(string id, string username, string email, string role)
        {
            return new UserModel(id, username, email, role);
        }
    }
}
