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
        private string id = string.Empty;
        public string Id
        {
            get => id;
            set => id = value;
        }
        private string username = string.Empty;
        public string Username
        {
            get => username;
            set => username = value;
        }
        private string email = string.Empty;
        public string Email
        {
            get => email;
            set => email = value;
        }
        private string role = string.Empty;
        public string Role
        {
            get => role;
            set => role = value;
        }
        public static UserModel BuildUserModel(string id, string username, string email, string role)
        {
            return new UserModel(id, username, email, role);
        }
    }
}
