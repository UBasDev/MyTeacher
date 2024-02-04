using CoreService.Application.Contexts;
using CoreService.Application.Models;
using CoreService.Application.Repositories;
using CoreService.Application.Repositories.GenericRepository;
using MediatR.NotificationPublishers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyTeacher.Helper.Middlewares;
using MyTeacher.Helper.Models;
using MyTeacher.JWT.Abstracts;
using MyTeacher.JWT.TokenGenerator;
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
        public static void AddApplicationRegistrations(this IServiceCollection services, string databaseConnectionUrl, JwtTokenSettings jwtTokenSettings)
        {
            #region DB_CONTEXT_SETTINGS
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(databaseConnectionUrl, opt => { opt.EnableRetryOnFailure(); }));
            #endregion
            #region MEDIATR_SETTINGS
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.NotificationPublisher = new ForeachAwaitPublisher();
            });
            #endregion
            #region JWT_SETTINGS
            services.AddSingleton<UserModel>();
            services.AddSingleton(jwtTokenSettings);
            services.AddSingleton<ITokenGenerator, TokenGenerator>();
            #endregion
        }
        public static void AddApplicationMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
