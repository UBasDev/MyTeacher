using CoreService.Domain.Entities.Common;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CoreService.Domain.Entities.ProfilePicture
{
    public class ProfilePicture : BaseEntity<ObjectId>, ISoftDelete
    {
        private ProfilePicture(string userId, string photoPath, string photoExtension, UInt32 photoLength)
        {
            UserId = userId;
            PhotoPath = photoPath;
            PhotoExtension = photoExtension;
            PhotoLength = photoLength;
        }
        public string UserId { get; private set; } = string.Empty;
        public string PhotoPath { get; private set; } = string.Empty;
        public string PhotoExtension { get; private set; } = string.Empty;
        public UInt32 PhotoLength { get; private set; } = 0;
        public DateTimeOffset? UpdatedAt { get; private set; }
        public DateTimeOffset? DeletedAt { get; private set; }
        public bool IsActive { get; private set; } = true;
        public bool IsDeleted { get; private set; } = false;
        public static ProfilePicture CreateNewProfilePicture(string userId, string photoPath, string photoExtension, UInt32 photoLength)
        {
            return new ProfilePicture(userId, photoPath, photoExtension, photoLength);
        }
    }
}
