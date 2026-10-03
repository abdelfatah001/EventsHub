using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static partial class Extensions
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            options.AddPolicy("EventsHubCorsPolicy", policy =>
            {
                policy.WithOrigins(
                    "",
                    "")
                .AllowAnyHeader()
                .AllowAnyMethod();
            }));

            return services;
        }

    }
}