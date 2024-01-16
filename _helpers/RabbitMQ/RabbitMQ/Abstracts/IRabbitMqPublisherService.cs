using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Abstracts
{
    public interface IRabbitMqPublisherService : IDisposable
    {
        void PublishMessageSync(string message);
        Task PublishMessageAsync(string message);
    }
}
