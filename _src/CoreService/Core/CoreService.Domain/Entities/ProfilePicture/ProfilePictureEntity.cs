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
    public sealed class ProfilePictureEntity : BaseEntityWithSoftDelete<ObjectId>
    {
        public ProfilePictureEntity()
        {
            UserId = string.Empty;
            UserProfileId = string.Empty;
            PhotoPath = string.Empty;
            PhotoExtension = string.Empty;
            PhotoLength = 0;
        }

        private ProfilePictureEntity(string userId, string userProfileId, string photoPath, string photoExtension, UInt32 photoLength)
        {
            UserId = userId;
            UserProfileId = userProfileId;
            PhotoPath = photoPath;
            PhotoExtension = photoExtension;
            PhotoLength = photoLength;
        }

        public string UserId { get; private set; }
        public string UserProfileId { get; private set; }
        public string PhotoPath { get; private set; }
        public string PhotoExtension { get; private set; }
        public UInt32 PhotoLength { get; private set; }

        public static ProfilePictureEntity CreateNewProfilePicture(string userId, string userProfileId, string photoPath, string photoExtension, UInt32 photoLength)
        {
            return new ProfilePictureEntity(userId, userProfileId, photoPath, photoExtension, photoLength);
        }
    }
}