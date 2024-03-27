using MyTeacher.Helper.Responses;
using System.Net.Mime;
using System.Net;
using System.Text;
using System.Threading.RateLimiting;
using Newtonsoft.Json;
using CoreService.Application.Constants;

namespace CoreService.API.Registrations
{
    public static class PresentationRegistration
    {
        public static void AddPresentationRegistrations(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            #region RATE_LIMITING_POLICY

            services.AddRateLimiter(rateOptions =>
            {
                //rateOptions.RejectionStatusCode = 429;
                rateOptions.AddPolicy(ApplicationConstants.FixedRateLimitingPolicyName, httpContext => RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User?.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 20,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    }));
                rateOptions.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = 429;
                    context.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        var response = BaseErrorResponse.BuildBaseErrorResponse($"You are sending too many requests. Please try again {retryAfter.TotalMinutes} minutes later", context.HttpContext.TraceIdentifier, HttpStatusCode.InternalServerError);
                        await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(response), Encoding.UTF8, cancellationToken: token);
                    }
                    else
                    {
                        var response = BaseErrorResponse.BuildBaseErrorResponse($"You are sending too many requests. Please try again later", context.HttpContext.TraceIdentifier, HttpStatusCode.InternalServerError);
                        await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(response), Encoding.UTF8, cancellationToken: token);
                    }
                };
            }
            );

            #endregion RATE_LIMITING_POLICY

            #region CORS_POLICY

            services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy(ApplicationConstants.AllowOnlyLocalCorsPolicyName, policyOptions =>
                {
                    policyOptions.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });

            #endregion CORS_POLICY
        }
    }
}