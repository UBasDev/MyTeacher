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
    public class CreateNewProfileWithPictureWhenUserCreatedDomainEvent(Guid createdUserId, UInt16 age, string firstname, string lastname, ulong birthDate, byte[] profilePictureData, string profilePictureExtension, string profilePictureName) : IDomainEvent
    {
        public Guid CreatedUserId { get; } = createdUserId;
        public UInt16 Age { get; } = age;
        public string Firstname { get; } = firstname;
        public string Lastname { get; } = lastname;
        public ulong BirthDate { get; } = birthDate;
        public byte[] ProfilePictureData { get; } = profilePictureData;
        public string ProfilePictureExtension { get; } = profilePictureExtension;
        public string ProfilePictureName { get; } = profilePictureName;
    }
}