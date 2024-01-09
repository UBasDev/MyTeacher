using CoreService.Domain.Entities.Common;

namespace CoreService.Domain.Entities.User
{
    public class UserEntity : BaseEntity, IUserEntityActions
    {
        public UserEntity(string username, string email, string passwordSalt, string passwordHash)
        {
            Username = username;
            Email = email;
            PasswordSalt = passwordSalt;
            PasswordHash = passwordHash;
        }
        private UserEntity() { }
        public string Username { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordSalt { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;

    }
}
