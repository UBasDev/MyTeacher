using CoreService.Application.Contexts;
using CoreService.Application.Models;
using CoreService.Application.Repositories;
using CoreService.Application.Repositories.GenericRepository;
using MediatR.NotificationPublishers;
using Microsoft.AspNetCore.Builder;
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
        public static void AddApplicationRegistrations(this IServiceCollection services, string databaseConnectionUrl)
        {
            services.AddSingleton<UserModel>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(databaseConnectionUrl, opt => { opt.EnableRetryOnFailure(); }));
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.NotificationPublisher = new ForeachAwaitPublisher();
            });
        }
        public static void AddApplicationMiddlewares(this IApplicationBuilder app)
        {

        }
    }
}
