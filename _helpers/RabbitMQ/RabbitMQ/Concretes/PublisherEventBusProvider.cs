using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Logging;
using RabbitMQ.Abstracts;
using RabbitMQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Concretes
{
    public class PublisherEventBusProvider : IPublisherEventBusProvider
    {
        public ISendEndpoint EventBus { get; private set; }

        private readonly EventBusSettings _eventBusSettings;
        private readonly ILogger<PublisherEventBusProvider> _logger;
        public PublisherEventBusProvider(EventBusSettings eventBusSettings, ILogger<PublisherEventBusProvider> logger)
        {
            _logger = logger;
            _eventBusSettings = eventBusSettings;
            EventBus = ConfigureBus();
        }
        private ISendEndpoint ConfigureBus()
        {
            try
            {
                var configuredBus = Bus.Factory.CreateUsingRabbitMq(configuration =>
                {
                    configuration.Host(
                        host: $"amqp://{_eventBusSettings.Username}:{_eventBusSettings.Password}@{_eventBusSettings.Host}:{_eventBusSettings.Port}",
                        hostConfiguration =>
                        {
                            hostConfiguration.Username(_eventBusSettings.Username);
                            hostConfiguration.Password(_eventBusSettings.Password);
                        });
                    configuration.UseMessageRetry(r => r.Immediate(_eventBusSettings.MessageRetryCount));

                    configuration.UseRateLimit(50, TimeSpan.FromSeconds(_eventBusSettings.RateLimitCount));

                    configuration.UseCircuitBreaker(cbConfiguration =>
                    {
                        cbConfiguration.TripThreshold = 25;
                        cbConfiguration.ActiveThreshold = 5;
                        cbConfiguration.TrackingPeriod = TimeSpan.FromMinutes(5);
                        cbConfiguration.ResetInterval = TimeSpan.FromMinutes(10);
                    });
                });
                return configuredBus.GetSendEndpoint(new Uri($"amqp://{_eventBusSettings.Username}:{_eventBusSettings.Password}@{_eventBusSettings.Host}:{_eventBusSettings.Port}/{_eventBusSettings.QueueName}")).Result;
            }catch(Exception ex)
            {
                _logger.LogError("An error occurred while configuring event bus. The error: {@Error}", ex.Message);
                throw new Exception($"An error occurred while configuring event bus. The error: {ex.Message}");
            }
        }
    }
}
