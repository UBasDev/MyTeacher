using CoreService.Domain.Events;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.DomainEvents.User
{
    public class CreateNewProfileWithoutPictureWhenUserCreatedDomainEvent(Guid createdUserId, UInt16 age, string firstname, string lastname, ulong birthDate) : IDomainEvent
    {
        public Guid CreatedUserId { get; } = createdUserId;
        public UInt16 Age { get; } = age;
        public string Firstname { get; } = firstname;
        public string Lastname { get; } = lastname;
        public ulong BirthDate { get; } = birthDate;
    }
}