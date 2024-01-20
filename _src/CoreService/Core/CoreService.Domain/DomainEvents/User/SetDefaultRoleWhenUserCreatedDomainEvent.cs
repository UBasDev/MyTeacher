using CoreService.Domain.Entities.Profile;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.DomainEvents.User
{
    public class SetDefaultRoleWhenUserCreatedDomainEvent(ProfileEntity profile) : INotification
    {
        public ProfileEntity Profile { get; } = profile;
    }
}
