using MyTeacher.Helper.Models;
using RabbitMQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Models
{
    public class AppSettings
    {
        public RabbitMqSettings RabbitMqSettings { get; set; } = new();
        public string DatabaseConnectionUrl { get; set; } = string.Empty;
        public JwtTokenSettings JwtTokenSettings { get; set; }
    }
}
