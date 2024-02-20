using CoreService.Application.ColumnWriters;
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
using NpgsqlTypes;
using Serilog.Sinks.PostgreSQL;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace CoreService.Application.Registrations
{
    public static class ApplicationRegistration
    {
        public static void AddApplicationRegistrations(this IServiceCollection services, string databaseConnectionUrl, JwtTokenSettings jwtTokenSettings, string environment)
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
            //services.AddScoped<UserModel>();
            services.AddSingleton(jwtTokenSettings);
            services.AddSingleton<ITokenGenerator, TokenGenerator>();
            #endregion
            #region SERILOG_SETTINGS
            Log.Logger = new LoggerConfiguration()
    //.Enrich.AtLevel(LogEventLevel.Information, e => e.FromLogContext())
    //.Enrich.AtLevel(LogEventLevel.Information, e => e.WithExceptionDetails())
    //.Enrich.AtLevel(LogEventLevel.Information, e => e.WithProperty("Environment", environment))
    .Enrich.WithProperty("Environment", environment)
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
    {
        AutoRegisterTemplate = true,
        //OverwriteTemplate = false,
        //DetectElasticsearchVersion = true,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
        NumberOfReplicas = 1,
        NumberOfShards = 2
    })
    .WriteTo.File(
       path: Path.Combine(Directory.GetCurrentDirectory(), "LogFiles", "app_log_file.txt"),
       rollingInterval: RollingInterval.Day,
       fileSizeLimitBytes: 10 * 1024 * 1024,
       retainedFileCountLimit: 2,
       rollOnFileSizeLimit: true,
       shared: true,
       flushToDiskInterval: TimeSpan.FromSeconds(5))
    .WriteTo.PostgreSQL(
        connectionString: databaseConnectionUrl,
        tableName: "logs",
        columnOptions: new Dictionary<string, ColumnWriterBase>
        {
            {"message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
            {"message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
            {"level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
            {"raise_date", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
            {"exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
            {"properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
            {"props_test", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
            {"machine_name", new SinglePropertyColumnWriter("MachineName", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") },
            {"user_id", new UserIdColumnWriter()}
        },
        needAutoCreateTable: true,
        respectCase: true
    )
    //.ReadFrom.Configuration(configuration)
    .MinimumLevel.Information()
    .CreateLogger();
            #endregion
        }
        public static void AddApplicationMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
