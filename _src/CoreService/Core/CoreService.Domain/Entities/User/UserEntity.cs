using CoreService.Domain.DomainEvents.User;
using CoreService.Domain.Entities.Common;
using CoreService.Domain.Entities.Profile;
using CoreService.Domain.Entities.Role;
using CoreService.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nest;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace CoreService.Domain.Entities.User
{
    public sealed class UserEntity : BaseEntityWithSoftDelete<Guid>
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
            Username = username;
            Email = email;
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

        public void CreateProfileWithPictureWhenUserCreated(Guid userId, UInt16 age, string firstname, string lastname, ulong birthDate, byte[] profilePictureData, string profilePictureExtension, string profilePictureName)
        {
            AddDomainEvents(new CreateNewProfileWithPictureWhenUserCreatedDomainEvent(userId, age, firstname, lastname, birthDate, profilePictureData, profilePictureExtension, profilePictureName));
        }

        public void CreateProfileWithoutPictureWhenUserCreated(Guid userId, UInt16 age, string firstname, string lastname, ulong birthDate)
        {
            AddDomainEvents(new CreateNewProfileWithoutPictureWhenUserCreatedDomainEvent(userId, age, firstname, lastname, birthDate));
        }

        public void ChangeUsername(string newUsername)
        {
            Username = newUsername;
        }

        public void ChangeEmail(string newEmail)
        {
            Email = newEmail;
        }

        public void SetRole(RoleEntity role)
        {
            if (role is null) throw new ArgumentNullException("Role can't be null to set it for this user");
            this.Role = role;
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