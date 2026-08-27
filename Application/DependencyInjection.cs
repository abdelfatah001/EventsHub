using Application.EventOccurences.Email;
using Application.EventOccurences.User.Request;
using Application.Email;
using Application.EventCategories;
using Application.EventOccurences;
using Application.Events;
using Application.Mappers;
using Application.Users;
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
