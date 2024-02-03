using CoreService.Domain.DomainEvents.User;
using CoreService.Domain.Entities.Common;
using CoreService.Domain.Entities.Profile;
using CoreService.Domain.Entities.Role;
using MediatR;
using System.Security.Cryptography;

namespace CoreService.Domain.Entities.User
{
    public class UserEntity : BaseEntity<Guid>
    {
        private UserEntity(string username, string email, string passwordSalt, string passwordHash) //This constructor has been created for EFCore.
        {
            Username = username ?? throw new Exception($"{nameof(username)} field cannot be empty"); ;
            Email = email ?? throw new Exception($"{nameof(email)} field cannot be empty");
            PasswordSalt = passwordSalt;
            PasswordHash = passwordHash;
        }
        private UserEntity(string username, string email, string passwordFromRequest)
        {
            Username = username ?? throw new Exception($"{nameof(username)} field cannot be empty");
            Email = email ?? throw new Exception($"{nameof(email)} field cannot be empty");

            var salt = GenerateSalt();
            PasswordSalt = Convert.ToBase64String(salt);
            PasswordHash = ComputeHash(passwordFromRequest, PasswordSalt);

            //Role = role ?? throw new Exception($"{nameof(role)} field cannot be empty");

            //AddDomainEvents(new CreateNewProfileWhenUserCreatedDomainEvent());
        }
        public string Username { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordSalt { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        //public RoleEntity Role { get; private set; } = new();
        public ProfileEntity Profile { get; set; }
        public static UserEntity CreateNewUser(string username, string email, string passwordFromRequest)
        {
            return new UserEntity(username, email, passwordFromRequest);
        }

        public void ChangeUsername(string newUsername)
        {
            Username = newUsername;
        }

        public void ChangeEmail(string newEmail)
        {
            Email = newEmail;
        }

        /*
        public void ChangeRole(RoleEntity newRole)
        {
            Role = newRole;
        }
        */
        private static byte[] GenerateSalt()
        {
            var rng = RandomNumberGenerator.Create();
            var salt = new byte[24];
            rng.GetBytes(salt);
            return salt;
        }
        public static string ComputeHash(string password, string saltString)
        {
            var salt = Convert.FromBase64String(saltString);
            using var hashGenerator = new Rfc2898DeriveBytes(password, salt, 10101, HashAlgorithmName.SHA512);
            var bytes = hashGenerator.GetBytes(24);
            return Convert.ToBase64String(bytes);
        }
    }
}
