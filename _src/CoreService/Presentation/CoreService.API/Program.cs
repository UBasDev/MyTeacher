using CoreService.API.Registrations;
using CoreService.Application.Constants;
using CoreService.Application.Models;
using CoreService.Application.Registrations;
using CoreService.Persistence.Registrations;
using MyTeacher.Helper.Culture;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
var configuration = new ConfigurationBuilder()
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Configurations"))
    .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile(path: $"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
.Build();

var appSettings = new AppSettings();
configuration.Bind(nameof(AppSettings), appSettings);

builder.Host.UseSerilog();
builder.Services.AddSingleton(appSettings);
builder.Services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddHttpContextAccessor(); ADD HERE IF YOU NEED TO REACH CONTEXT FROM HANDLERS

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationRegistrations(appSettings.DatabaseConnectionUrl, appSettings.JwtTokenSettings, appSettings.MongoDbSettings, appSettings.EventBusSettings, environment);
builder.Services.AddPersistenceRegistrations(appSettings);
builder.Services.AddPresentationRegistrations();

var app = builder.Build();
app.UseCors(ApplicationConstants.AllowOnlyLocalCorsPolicyName);
app.UseSerilogRequestLogging();
app.UseRateLimiter();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.AddApplicationMiddlewares();
//app.AddCultureRegistrations();
app.MapControllers().RequireRateLimiting(ApplicationConstants.FixedRateLimitingPolicyName);
app.Run();