using CoreService.Application.Repositories;
using CoreService.Domain.DomainEvents.User;
using CoreService.Domain.Entities.Profile;
using CoreService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CoreService.Application.DomainEventHandlers.User
{
    public class CreateNewProfileWithPictureWhenUserCreatedDomainEventHandler(IUnitOfWork unitOfWork, ILogger<CreateNewProfileWithPictureWhenUserCreatedDomainEventHandler> logger, IPublisher publisher) : IDomainEventHandler<CreateNewProfileWithPictureWhenUserCreatedDomainEvent>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<CreateNewProfileWithPictureWhenUserCreatedDomainEventHandler> _logger = logger;
        private readonly IPublisher _publisher = publisher;

        public async Task Handle(CreateNewProfileWithPictureWhenUserCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var profileToCreate = ProfileEntity.CreateNewProfile(notification.Age, notification.CreatedUserId, notification.Firstname, notification.Lastname, notification.BirthDate);
                await _unitOfWork.ProfileWriteRepository.InsertSingleAsync(profileToCreate, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                if (notification.ProfilePictureData != null) profileToCreate.CreateProfilePictureWhenProfileCreated(notification.CreatedUserId.ToString(), profileToCreate.Id.ToString(), notification.ProfilePictureData, notification.ProfilePictureExtension, notification.ProfilePictureName);

                foreach (var currentEvent in profileToCreate.DomainEvents)
                {
                    await _publisher.Publish(currentEvent, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating this profile. Error: {@Error}", ex);
            }
        }
    }
}