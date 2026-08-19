using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DbContexts;
using Infrastructure.Email;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IStmpEmailSender, StmpEmailSender>();
            return services;
        }
    }
}
