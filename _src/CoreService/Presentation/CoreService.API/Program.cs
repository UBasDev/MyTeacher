using CoreService.Application.ColumnWriters;
using CoreService.Application.Models;
using CoreService.Application.Registrations;
using CoreService.Domain.Elasticsearch;
using CoreService.Persistence.Registrations;
using Elasticsearch.Net;
using MyTeacher.Helper.Attributes;
using Nest;
using NpgsqlTypes;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.PostgreSQL;
using System;
using System.Reflection;

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

builder.Services.AddApplicationRegistrations(appSettings.DatabaseConnectionUrl, appSettings.JwtTokenSettings, environment);
builder.Services.AddPersistenceRegistrations(appSettings);

var app = builder.Build();

app.UseSerilogRequestLogging();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.AddApplicationMiddlewares();
app.MapControllers();

app.Run();