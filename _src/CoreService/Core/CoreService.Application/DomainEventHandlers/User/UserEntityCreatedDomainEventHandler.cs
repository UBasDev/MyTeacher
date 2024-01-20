using CoreService.Application.Repositories;
using CoreService.Domain.DomainEvents.User;
using CoreService.Domain.Entities.Profile;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.DomainEventHandlers.User
{
    public class UserEntityCreatedDomainEventHandler(IUnitOfWork unitOfWork) : INotificationHandler<SetDefaultRoleWhenUserCreatedDomainEvent>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task Handle(SetDefaultRoleWhenUserCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _unitOfWork.ProfileWriteRepository.InsertSingle(ProfileEntity.CreateNewProfile(notification.Profile.Age));
            _unitOfWork.SaveChanges();
        }
    }
}
