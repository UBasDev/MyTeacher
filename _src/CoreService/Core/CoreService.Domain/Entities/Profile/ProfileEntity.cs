using CoreService.Domain.DomainEvents.Profile;
using CoreService.Domain.Entities.Common;
using CoreService.Domain.Entities.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.Entities.Profile
{
    sealed public class ProfileEntity : BaseEntity<Guid>, ISoftDelete
    {
        private ProfileEntity(UInt16 age, Guid userId)
        {
            Age = age;
            UserId = userId;
        }
        public UInt16 Age { get; private set; }
        
        public UserEntity User { get; private set; }
        public Guid UserId { get; private set; }
        public DateTimeOffset? UpdatedAt { get; private set; }
        public DateTimeOffset? DeletedAt { get; private set; }
        public bool IsActive { get; private set; } = true;
        public bool IsDeleted { get; private set; } = false;
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
