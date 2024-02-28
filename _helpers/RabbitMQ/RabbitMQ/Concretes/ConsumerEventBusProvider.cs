using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Abstracts;
using RabbitMQ.Models;

namespace RabbitMQ.Concretes
{
    public static class AddEventBusForConsumersProvider
    {
        public static void AddEventBusForConsumers<T>(this IServiceCollection services, EventBusSettings _eventBusSettings) where T : IConsumer
        {
            try
            {
                services.AddMassTransit(massTransitOptions =>
                {
                    massTransitOptions.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: _eventBusSettings.Prefix, includeNamespace: false));
                    massTransitOptions.AddConsumer(typeof(T));
                    massTransitOptions.UsingRabbitMq((context, rabbitMqConfig) =>
                    {
                        //cfg.ConcurrentMessageLimit = 28;
                        //cfg.PrefetchCount = 28;
                        rabbitMqConfig.UseMessageRetry(r => r.Immediate(_eventBusSettings.MessageRetryCount));
                        rabbitMqConfig.UseRateLimit(50, TimeSpan.FromSeconds(_eventBusSettings.RateLimitCount));

                        rabbitMqConfig.UseCircuitBreaker(cbConfiguration1 =>
                        {
                            cbConfiguration1.TripThreshold = 25;
                            cbConfiguration1.ActiveThreshold = 5;
                            cbConfiguration1.TrackingPeriod = TimeSpan.FromMinutes(5);
                            cbConfiguration1.ResetInterval = TimeSpan.FromMinutes(10);
                        });

                        rabbitMqConfig.Host(
                            host: $"amqp://{_eventBusSettings.Username}:{_eventBusSettings.Password}@{_eventBusSettings.Host}:{_eventBusSettings.Port}",
                            hostConfiguration =>
                            {
                                hostConfiguration.Username(_eventBusSettings.Username);
                                hostConfiguration.Password(_eventBusSettings.Password);
                            });

                        rabbitMqConfig.ReceiveEndpoint(
                            queueName: _eventBusSettings.QueueName,
                            configureEndpoint1 =>
                            {
                                configureEndpoint1.ConfigureConsumer(context, typeof(T));
                            });

                        rabbitMqConfig.ConfigureEndpoints(context);
                    });
                });
            }
            catch (Exception ex)
            {
                //_logger.LogError("An error occurred while configuring event bus. The error: {@ErrorMessage}", ex.Message);
                throw new Exception($"An error occurred while configuring event bus. The error: {ex.Message}");
            }
        }
    }
}
