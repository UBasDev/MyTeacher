using RabbitMQ.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static RabbitMQ.Concretes.RabbitMqConsumerService;

namespace RabbitMQ.Abstracts
{
    public interface IRabbitMqConsumerService
    {
        void ConsumeAllSync(Action<string> @event);
        Task ConsumeAllAsync(Func<string, Task> @event);
    }
}
