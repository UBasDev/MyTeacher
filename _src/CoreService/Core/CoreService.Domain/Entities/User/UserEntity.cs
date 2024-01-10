﻿using CoreService.Domain.DomainEvents.User;
using CoreService.Domain.Entities.Common;
using CoreService.Domain.Entities.Profile;
using CoreService.Domain.Entities.Role;

namespace CoreService.Domain.Entities.User
{
    public class UserEntity : BaseEntity
    {
        public UserEntity(string username, string email, string passwordSalt, string passwordHash, RoleEntity role, ProfileEntity profile)
        {
            Username = username ?? throw new Exception($"{nameof(username)} field cannot be empty");
            Email = email ?? throw new Exception($"{nameof(email)} field cannot be empty");
            PasswordSalt = passwordSalt ?? throw new Exception($"{nameof(passwordSalt)} field cannot be empty");
            PasswordHash = passwordHash ?? throw new Exception($"{nameof(passwordHash)} field cannot be empty");
            Role = role ?? throw new Exception($"{nameof(role)} field cannot be empty");

            AddDomainEvents(new UserEntityCreatedDomainEvent(profile));

            Profile = profile ?? throw new Exception($"{nameof(profile)} field cannot be empty");
        }
        private UserEntity() { }
        public string Username { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordSalt { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public RoleEntity Role { get; private set; } = new();
        public ProfileEntity Profile { get; set; }

        public void ChangeUsername(string newUsername)
        {
            Username = newUsername;
        }

        public void ChangeEmail(string newEmail)
        {
            Email = newEmail;
        }

        public void ChangeRole(RoleEntity newRole)
        {
            Role = newRole;
        }

    }
}
