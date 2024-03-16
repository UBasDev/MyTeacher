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
    sealed public class UserEntity : BaseEntityWithSoftDelete<Guid>
    {
        public UserEntity()
        {
            Username = string.Empty;
            Email = string.Empty;
            PasswordHash = string.Empty;
            PasswordSalt = string.Empty;
            LastLoginDate = null;
            Role = null;
            RoleId = null;
            Profile = null;
        }
        private UserEntity(string username, string email, string passwordFromRequest)
        {
            Username = username ?? throw new Exception($"{nameof(username)} field cannot be empty");
            Email = email ?? throw new Exception($"{nameof(email)} field cannot be empty");

            var salt = GenerateSalt();
            PasswordSalt = Convert.ToBase64String(salt);
            PasswordHash = ComputeHash(passwordFromRequest, PasswordSalt);
        }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordSalt { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTimeOffset? LastLoginDate { get; private set; }
        public RoleEntity? Role { get; private set; }
        public Guid? RoleId { get; private set; }
        public ProfileEntity? Profile { get; private set; }

        public static UserEntity CreateNewUser(string username, string email, string passwordFromRequest)
        {
            return new UserEntity(username, email, passwordFromRequest);
        }
        public void CreateProfileWhenUserCreated(Guid userId, UInt16 age, byte[] profilePictureData, string profilePictureExtension, string profilePictureName)
        {
            AddDomainEvents(new CreateNewProfileWhenUserCreatedDomainEvent(userId, age, profilePictureData, profilePictureExtension, profilePictureName));
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
