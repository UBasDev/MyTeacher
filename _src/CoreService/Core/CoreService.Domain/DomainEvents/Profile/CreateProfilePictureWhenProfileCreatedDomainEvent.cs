using CoreService.Domain.Events;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.DomainEvents.Profile
{
    public class CreateProfilePictureWhenProfileCreatedDomainEvent(Guid userId, Guid userProfileId, byte[] profilePicture) : IDomainEvent
    {
        public Guid UserId { get; } = userId;
        public Guid UserProfileId { get; } = userProfileId;
        public byte[] ProfilePicture { get; } = profilePicture;
    }
}
