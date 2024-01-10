using CoreService.Domain.DomainEvents.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.DomainEventHandlers.User
{
    public class UserEntityCreatedDomainEventHandler : INotificationHandler<UserEntityCreatedDomainEvent>
    {
        public async Task Handle(UserEntityCreatedDomainEvent notification, CancellationToken cancellationToken)
        {

        }
    }
}
