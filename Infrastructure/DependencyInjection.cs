using Application.Payment.Paymob;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DbContexts;
using Infrastructure.Email;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>
                (options => options.UseSqlServer
                    (configuration.GetConnectionString("LocalConnection")));

            services.Configure<EmailSettings>
                (configuration.GetSection("EmailSettings"));

            services.Configure<PaymobOptions>
                (configuration.GetSection("PaymobSettings"));

            services.AddHttpClient<PaymobClient>((serviceProvider, client) =>
            {
                var options = serviceProvider
                    .GetRequiredService<IOptions<PaymobOptions>>()
                    .Value;

                client.BaseAddress = new Uri(options.BaseUrl);
            });


            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IStmpEmailSender, StmpEmailSender>();
            services.AddScoped<IEventRepo, EventRepo>();
            services.AddScoped<IEventCategoryRepo, EventCategoryRepo>();
            services.AddScoped<IEventOccurenceRepo, EventOccurenceRepo>();
            return services;
        }
    }
}
