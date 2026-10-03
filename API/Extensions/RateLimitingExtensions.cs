using Microsoft.Extensions.DependencyInjection;
using System.Threading.RateLimiting;

namespace API.Extensions
{
    public static partial class Extensions
    {
        public static IServiceCollection AddRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddPolicy("AuthLimiter", HttpContext =>
                {
                    var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Inknown";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: ip,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        });
                });
            });

            return services;
        }

    }
}