using CoreService.Domain.DomainEvents.User;
using CoreService.Domain.Entities.Common;
using CoreService.Domain.Entities.Profile;
using CoreService.Domain.Entities.Role;
using CoreService.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nest;
using System.Security.Cryptography;

namespace CoreService.Domain.Entities.User
{
    public class UserEntity : BaseEntity<Guid>, ISoftDelete
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
        }
        public string Username { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordSalt { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        //public RoleEntity Role { get; private set; } = new();
        public ProfileEntity Profile { get; private set; }
        public DateTimeOffset? UpdatedAt { get ; private set; }
        public DateTimeOffset? DeletedAt { get ; private set ; }
        public bool IsActive { get; private set; } = true;
        public bool IsDeleted { get; private set; } = false;

        public static UserEntity CreateNewUser(string username, string email, string passwordFromRequest)
        {
            return new UserEntity(username, email, passwordFromRequest);
        }
        public void CreateProfileWhenUserCreated(Guid userId, UInt16 age, byte[]? profilePicture)
        {
            AddDomainEvents(new CreateNewProfileWhenUserCreatedDomainEvent(userId, age, profilePicture));
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
