using CoreService.Domain.Entities.Profile;
using CoreService.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.DomainEvents.User
{
    public class CreateNewProfileWhenUserCreatedDomainEvent(Guid createdUserId, UInt16 age, byte[] profilePictureData, string profilePictureExtension, string profilePictureName) : IDomainEvent
    {
        public Guid CreatedUserId { get; } = createdUserId;
        public UInt16 Age { get; } = age;
        public byte[] ProfilePictureData { get; } = profilePictureData;
        public string ProfilePictureExtension { get; } = profilePictureExtension;
        public string ProfilePictureName { get; set; } = profilePictureName;
    }
}
