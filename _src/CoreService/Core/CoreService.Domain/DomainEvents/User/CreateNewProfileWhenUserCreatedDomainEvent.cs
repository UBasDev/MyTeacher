﻿using CoreService.Domain.Entities.Profile;
using CoreService.Domain.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.DomainEvents.User
{
    public class CreateNewProfileWhenUserCreatedDomainEvent(Guid createdUserId, UInt16 age) : IDomainEvent
    {
        public Guid CreatedUserId { get; } = createdUserId;
        public UInt16 Age { get; set; } = age;
    }
}
