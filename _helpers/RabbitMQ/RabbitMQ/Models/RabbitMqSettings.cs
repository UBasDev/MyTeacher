using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Models
{
    public class RabbitMqSettings
    {
        public string HostName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string VirtualHost { get; set; } = "/";
        public string ClientProvidedName { get; set; } = string.Empty;
        public bool IsDurable { get; set; } = true;
        public bool IsExclusive { get; set; } = false;
        public bool IsAutoDelete { get; set; } = false;
        public Dictionary<string, object>? Arguments { get; set; } = null;
        public uint PrefetchSize { get; set; } = 0;
        public ushort PrefetchCount { get; set; } = 1;
        public bool IsGlobal { get; set; } = false;
        public string ExchangeName { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;
        public string RoutingKey { get; set; } = string.Empty;
    }
}
