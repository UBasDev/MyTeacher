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
                        QueueLimit = 10,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        //Bu senaryoda saniyede 20 istekte bulunabilir fakat 20'den fazla istek gelir ise bunlardan 10 tanesi kuyruğa alınabilir olacak şekilde ayarlanmıştır. QueueProcessingOrder ise eskiden yeniye okunacak şekilde ayarlanmıştır.
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
                /*
                rateOptions.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 20,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 10,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                //Bu senaryoda saniyede 20 istekte bulunabilir fakat 20'den fazla istek gelir ise bunlardan 10 tanesi kuyruğa alınabilir olacak şekilde ayarlanmıştır. QueueProcessingOrder ise eskiden yeniye okunacak şekilde ayarlanmıştır.
            })
            );
                */
                /* Maybe you want to have a limit where one can make 600 requests per minute, but only 6000 per hour. You could chain 2 rate limits:

                 options.GlobalLimiter = PartitionedRateLimiter.CreateChained(
        PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            RateLimitPartition.GetFixedWindowLimiter(httpContext.ResolveClientIpAddress(), partition =>
                new FixedWindowRateLimiterOptions
                {
                    AutoReplenishment = true,
                    PermitLimit = 600,
                    Window = TimeSpan.FromMinutes(1)
                })),
        PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            RateLimitPartition.GetFixedWindowLimiter(httpContext.ResolveClientIpAddress(), partition =>
                new FixedWindowRateLimiterOptions
                {
                    AutoReplenishment = true,
                    PermitLimit = 6000,
                    Window = TimeSpan.FromHours(1)
                })));
                 */
            }
            );
        }
    }
}