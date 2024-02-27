using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Abstracts
{
    public interface IConsumerEventBusProvider
    {
        IBusControl ConfigureBus(Action<IRabbitMqBusFactoryConfigurator> registrationAction = null);
    }
}
