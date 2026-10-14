using API.DTOs.Auth;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static partial class Extensions
    {
        public static IServiceCollection AddAuthenticationPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();

            if (jwtSettings is null)
                throw new InvalidOperationException("JWT settings are not configured properly.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
            .AddJwtBearer("JwtBearer", options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,

                    ValidateAudience = true,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.Issuer,

                    ValidAudience = jwtSettings.Audience,

                    IssuerSigningKey = new SymmetricSecurityKey
                        (System.Text.Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

                };
            });

            return services;
        }
    }
}
