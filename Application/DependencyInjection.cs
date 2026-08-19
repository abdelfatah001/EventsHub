using Application.DTOs.Email;
using Application.DTOs.User.Request;
using Application.Email;
using Application.Interfaces;
using Application.Mappers;
using Application.Services;
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

            // Add FluentValidation validators
            services.AddValidatorsFromAssemblyContaining<AddUserDto>();
            services.AddValidatorsFromAssemblyContaining<UpdateEmailDto>();
            services.AddValidatorsFromAssemblyContaining<UpdatePasswordDto>();
            services.AddValidatorsFromAssemblyContaining<UpdateProfileDto>();
            services.AddValidatorsFromAssemblyContaining<ForgetPasswordDto>();
            services.AddValidatorsFromAssemblyContaining<SendEmailDto>();


            services.AddAutoMapper(typeof(ApplicationAssemblyMarker).Assembly);
            return services;
        }
    }
}
