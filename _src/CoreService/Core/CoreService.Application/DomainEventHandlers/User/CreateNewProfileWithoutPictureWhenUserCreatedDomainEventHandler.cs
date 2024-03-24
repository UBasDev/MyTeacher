using CoreService.Application.Repositories;
using CoreService.Domain.DomainEvents.User;
using CoreService.Domain.Entities.Profile;
using CoreService.Domain.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.DomainEventHandlers.User
{
    public class CreateNewProfileWithoutPictureWhenUserCreatedDomainEventHandler(ILogger<CreateNewProfileWithoutPictureWhenUserCreatedDomainEventHandler> logger, IUnitOfWork unitOfWork) : IDomainEventHandler<CreateNewProfileWithoutPictureWhenUserCreatedDomainEvent>
    {
        private ILogger<CreateNewProfileWithoutPictureWhenUserCreatedDomainEventHandler> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(CreateNewProfileWithoutPictureWhenUserCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var profileToCreate = ProfileEntity.CreateNewProfile(notification.Age, notification.CreatedUserId, notification.Firstname, notification.Lastname, notification.BirthDate);
                await _unitOfWork.ProfileWriteRepository.InsertSingleAsync(profileToCreate, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating this profile. Error: {@Error}", ex);
            }
        }
    }
}