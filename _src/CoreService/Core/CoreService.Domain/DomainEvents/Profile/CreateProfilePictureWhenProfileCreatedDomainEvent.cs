using CoreService.Domain.Events;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.DomainEvents.Profile
{
    public class CreateProfilePictureWhenProfileCreatedDomainEvent(string userId, string userProfileId, byte[] profilePicture) : IDomainEvent
    {
        public string UserId { get; } = userId;
        public string UserProfileId { get; } = userProfileId;
        public byte[] ProfilePicture { get; } = profilePicture;
    }
}
