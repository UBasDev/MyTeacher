using CoreService.Domain.Entities.Common;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CoreService.Domain.Entities.ProfilePicture
{
    public class ProfilePictureEntity : BaseEntity<ObjectId>, ISoftDelete
    {
        private ProfilePictureEntity(string userId, string userProfileId, string photoPath, string photoExtension, UInt32 photoLength)
        {
            UserId = userId;
            UserProfileId = userProfileId;
            PhotoPath = photoPath;
            PhotoExtension = photoExtension;
            PhotoLength = photoLength;
        }
        public string UserId { get; private set; } = string.Empty;
        public string UserProfileId { get; private set; } = string.Empty;
        public string PhotoPath { get; private set; } = string.Empty;
        public string PhotoExtension { get; private set; } = string.Empty;
        public UInt32 PhotoLength { get; private set; } = 0;
        public DateTimeOffset? UpdatedAt { get; private set; }
        public DateTimeOffset? DeletedAt { get; private set; }
        public bool IsActive { get; private set; } = true;
        public bool IsDeleted { get; private set; } = false;
        public static ProfilePictureEntity CreateNewProfilePicture(string userId, string userProfileId, string photoPath, string photoExtension, UInt32 photoLength)
        {
            return new ProfilePictureEntity(userId, userProfileId, photoPath, photoExtension, photoLength);
        }
    }
}
