using MassTransit;
using Microsoft.Extensions.Logging;
using RabbitMQ.Abstracts;
using RabbitMQ.Models;

namespace RabbitMQ.Concretes
{
    public class ConsumerEventBusProvider(EventBusSettings eventBusSettings, ILogger<ConsumerEventBusProvider> logger) : IConsumerEventBusProvider
    {
        private readonly EventBusSettings _eventBusSettings = eventBusSettings;
        private readonly ILogger<ConsumerEventBusProvider> _logger = logger;
        public IBusControl ConfigureBus(Action<IRabbitMqBusFactoryConfigurator> registrationAction = null)
        {
            try
            {
                return Bus.Factory.CreateUsingRabbitMq(configuration1 =>
                {
                    configuration1.Host(
                        host: $"amqp://{_eventBusSettings.Username}:{_eventBusSettings.Password}@{_eventBusSettings.Host}:{_eventBusSettings.Port}",
                        hostConfiguration =>
                        {
                            hostConfiguration.Username(_eventBusSettings.Username);
                            hostConfiguration.Password(_eventBusSettings.Password);
                        });
                    configuration1.UseMessageRetry(r => r.Immediate(_eventBusSettings.MessageRetryCount));

                    configuration1.UseRateLimit(50, TimeSpan.FromSeconds(_eventBusSettings.RateLimitCount));

                    configuration1.UseCircuitBreaker(cbConfiguration1 =>
                    {
                        cbConfiguration1.TripThreshold = 25;
                        cbConfiguration1.ActiveThreshold = 5;
                        cbConfiguration1.TrackingPeriod = TimeSpan.FromMinutes(5);
                        cbConfiguration1.ResetInterval = TimeSpan.FromMinutes(10);
                    });
                    registrationAction?.Invoke(configuration1);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while configuring event bus. The error: {@ErrorMessage}", ex.Message);
                throw new Exception($"An error occurred while configuring event bus. The error: {ex.Message}");
            }
        }
    }
}
