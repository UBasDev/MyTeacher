using CoreService.Application.Models;
using CoreService.Application.Repositories;
using CoreService.Application.Repositories.GenericRepository;
using CoreService.Application.Repositories.ProfileRepository;
using CoreService.Persistence.Repositories;
using CoreService.Persistence.Repositories.GenericRepository;
using CoreService.Persistence.Repositories.ProfileRepository;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Abstracts;
using RabbitMQ.Concretes;
using RabbitMQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Persistence.Registrations
{
    public static class PersistenceRegistration
    {
        public static void AddPersistenceRegistrations(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton(appSettings.RabbitMqSettings);
            services.AddSingleton<IRabbitMqPublisherService, RabbitMqPublisherService>();
        }
    }
}
