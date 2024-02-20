using CoreService.Application.Repositories;
using CoreService.Domain.DomainEvents.User;
using CoreService.Domain.Entities.Profile;
using CoreService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.DomainEventHandlers.User
{
    public class CreateNewProfileWhenUserCreatedDomainEventHandler(IUnitOfWork unitOfWork, ILogger<CreateNewProfileWhenUserCreatedDomainEventHandler> logger) : IDomainEventHandler<CreateNewProfileWhenUserCreatedDomainEvent>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<CreateNewProfileWhenUserCreatedDomainEventHandler> _logger = logger;
        public async Task Handle(CreateNewProfileWhenUserCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            await _unitOfWork.ProfileWriteRepository.InsertSingleAsync(ProfileEntity.CreateNewProfile(notification.Age, notification.CreatedUserId));
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
