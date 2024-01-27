using CoreService.Application.Contexts;
using CoreService.Application.Repositories;
using CoreService.Application.Repositories.GenericRepository;
using MediatR.NotificationPublishers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Registrations
{
    public static class ApplicationRegistration
    {
        public static void AddApplicationRegistrations(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql("User ID=postgres;Password=admin;Server=localhost;Port=5432;Database=MyTeacher-Core;Include Error Detail=true;Pooling=true;Connection Lifetime=0;", opt => { opt.EnableRetryOnFailure(); }));
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.NotificationPublisher = new ForeachAwaitPublisher();
            });
        }
    }
}
