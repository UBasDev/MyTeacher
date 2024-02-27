using CoreService.Application.Repositories.ProfilePictureRepository;
using CoreService.Persistence.Repositories.ProfilePictureRepository;
using ProfilePicture.WORKER;
using ProfilePicture.WORKER.Models;
using RabbitMQ.Abstracts;
using RabbitMQ.Concretes;

var builder = Host.CreateApplicationBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
var configuration = new ConfigurationBuilder()
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Configurations"))
    .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile(path: $"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
.Build();
var appSettings = new AppSettings();
configuration.Bind(nameof(AppSettings), appSettings);
builder.Services.AddSingleton(appSettings);
builder.Services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));

#region MONGODB_SETTINGS
builder.Services.AddSingleton(appSettings.MongoDbSettings);
builder.Services.AddSingleton<IProfilePictureWriteRepository, ProfilePictureWriteRepository>();
#endregion

#region EVENTBUS_SETTINGS
builder.Services.AddSingleton(appSettings.EventBusSettings);
builder.Services.AddSingleton<IConsumerEventBusProvider, ConsumerEventBusProvider>();
#endregion

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
