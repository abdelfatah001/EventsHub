using Application.Email;
using Application.EventCategories;
using Application.EventOccurences;
using Application.Events;
using Application.ResetPasswordToken;
using Application.Users;
using Application.Vertification;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
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
            services.AddScoped<IVerificationService, VerificationService>();
            services.AddScoped<VerificationEmailBuilder>();
            services.AddScoped<IPasswordResetTokenService, PasswordResetTokenService>();
            services.AddScoped<IPasswordHasher<VertificationCode>, PasswordHasher<VertificationCode>>();
            // Add FluentValidation validators

            services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyMarker>();

            services.AddAutoMapper(typeof(ApplicationAssemblyMarker).Assembly);
            return services;
        }
    }
}
