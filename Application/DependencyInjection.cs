using Application.DTOs.Email;
using Application.DTOs.User.Request;
using Application.Email;
using Application.Interfaces;
using Application.Mappers;
using Application.Services;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register application services here
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IEventOccurenceService, EventOccurenceService>();

            // Add FluentValidation validators

            services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyMarker>();

            services.AddAutoMapper(typeof(ApplicationAssemblyMarker).Assembly);
            return services;
        }
    }
}
