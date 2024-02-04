using Microsoft.AspNetCore.Http;
using MyTeacher.Helper.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MyTeacher.Helper.Middlewares
{
    public class GlobalExceptionMiddleware(RequestDelegate next)
    {
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
                var response = BaseErrorResponse.BuildBaseErrorResponse($"An unexpected error occured during the process. Error is: {ex.Message}", httpContext.TraceIdentifier);
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response), Encoding.UTF8);
            }
        }
    }
}
