using FileLog.WORKER;
using FileLog.WORKER.Models;
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

//builder.Services.AddHostedService<Worker>();

builder.Services.AddHostedService<FileLogRabbitMqConsumer>();
builder.Services.AddSingleton(appSettings.RabbitMqSettings);
builder.Services.AddSingleton<IRabbitMqConsumerService, RabbitMqConsumerService>();

var host = builder.Build();
host.Run();
