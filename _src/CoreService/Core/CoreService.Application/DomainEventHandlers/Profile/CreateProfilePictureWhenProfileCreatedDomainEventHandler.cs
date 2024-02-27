using CoreService.Application.MQContracts.Commands.Abstracts;
using CoreService.Application.MQContracts.Commands.Concretes;
using CoreService.Application.Repositories;
using CoreService.Domain.DomainEvents.Profile;
using CoreService.Domain.DomainEvents.User;
using CoreService.Domain.Events;
using Microsoft.Extensions.Logging;
using RabbitMQ.Abstracts;

namespace CoreService.Application.DomainEventHandlers.Profile
{
    public class CreateProfilePictureWhenProfileCreatedDomainEventHandler(ILogger<CreateProfilePictureWhenProfileCreatedDomainEventHandler> logger, IPublisherEventBusProvider publisherEventBusProvider) : IDomainEventHandler<CreateProfilePictureWhenProfileCreatedDomainEvent>
    {
        private readonly ILogger<CreateProfilePictureWhenProfileCreatedDomainEventHandler> _logger = logger;
        private readonly IPublisherEventBusProvider _publisherEventBusProvider = publisherEventBusProvider;
        public async Task Handle(CreateProfilePictureWhenProfileCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            await _publisherEventBusProvider.EventBus.Send<ICreateProfilePictureMQCommand>(
                new CreateProfilePictureMQCommand()
                {
                    UserId = notification.UserId,
                    ProfilePicture = notification.ProfilePicture,
                    UserProfileId = notification.UserProfileId
                }, cancellationToken
                );
        }
    }
}
