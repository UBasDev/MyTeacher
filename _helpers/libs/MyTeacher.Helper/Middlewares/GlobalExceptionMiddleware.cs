﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyTeacher.Helper.Responses;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;
using System.Text;

namespace MyTeacher.Helper.Middlewares
{
    public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        private readonly ILogger<GlobalExceptionMiddleware> _logger = logger;
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = MediaTypeNames.Application.Json;
                var response = BaseErrorResponse.BuildBaseErrorResponse($"An unexpected error occured during the process. Error is: {ex.Message}", httpContext.TraceIdentifier, HttpStatusCode.InternalServerError);
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response), Encoding.UTF8);
                _logger.LogError("{@ClassName} - {@Error}", this.GetType().Name, ex.Message);
            }
        }
    }
}