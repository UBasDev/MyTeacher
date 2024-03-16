using CoreService.Domain.DomainEvents.Profile;
using CoreService.Domain.Entities.Common;
using CoreService.Domain.Entities.Company;
using CoreService.Domain.Entities.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.Entities.Profile
{
    sealed public class ProfileEntity : BaseEntityWithSoftDelete<Guid>
    {
        public ProfileEntity()
        {
            Age = 0;
            Firstname = null;
            Lastname = null;
            User = null;
            UserId = null;
            Company = null;
            CompanyId = null;
        }
        private ProfileEntity(UInt16 age, Guid userId)
        {
            Age = age;
            UserId = userId;
        }
        public UInt16 Age { get; private set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public UserEntity? User { get; private set; }
        public Guid? UserId { get; private set; }
        public CompanyEntity? Company { get; private set; }
        public Guid? CompanyId { get; private set; }
        public void CreateProfilePictureWhenProfileCreated(string userId, string userProfileId, byte[] profilePictureData, string profilePictureExtension, string profilePictureName)
        {
            AddDomainEvents(new CreateProfilePictureWhenProfileCreatedDomainEvent(userId, userProfileId, profilePictureData, profilePictureExtension, profilePictureName));
        }
        public static ProfileEntity CreateNewProfile(UInt16 age, Guid userId)
        {
            return new ProfileEntity(age, userId);
        }
    }
}
