using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Models
{
    public class EventBusSettings
    {
        public string Prefix { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public int MessageRetryCount { get; set; }
        public int RateLimitCount { get; set; }
        public string QueueName { get; set; }
    }
}
