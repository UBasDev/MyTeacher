using CoreService.Domain.Entities.Profile;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.DomainEvents.User
{
    public class UserEntityCreatedDomainEvent : INotification
    {
        public ProfileEntity Profile { get; set; }
        public UserEntityCreatedDomainEvent(ProfileEntity profile)
        {
            Profile = profile;
        }
    }
}
