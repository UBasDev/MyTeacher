using CoreService.Application.Repositories;
using CoreService.Domain.DomainEvents.User;
using CoreService.Domain.Entities.Profile;
using CoreService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CoreService.Application.DomainEventHandlers.User
{
    public class CreateNewProfileWhenUserCreatedDomainEventHandler(IUnitOfWork unitOfWork, ILogger<CreateNewProfileWhenUserCreatedDomainEventHandler> logger, IPublisher publisher) : IDomainEventHandler<CreateNewProfileWhenUserCreatedDomainEvent>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<CreateNewProfileWhenUserCreatedDomainEventHandler> _logger = logger;
        private readonly IPublisher _publisher = publisher;
        public async Task Handle(CreateNewProfileWhenUserCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var profileToCreate = ProfileEntity.CreateNewProfile(notification.Age, notification.CreatedUserId);
            await _unitOfWork.ProfileWriteRepository.InsertSingleAsync(profileToCreate);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            if(notification.ProfilePictureData != null) profileToCreate.CreateProfilePictureWhenProfileCreated(notification.CreatedUserId.ToString(), profileToCreate.Id.ToString(), notification.ProfilePictureData, notification.ProfilePictureExtension, notification.ProfilePictureName);

            foreach(var currentEvent in profileToCreate.DomainEvents)
            {
                await _publisher.Publish(currentEvent, cancellationToken);
            }
        }
    }
}
