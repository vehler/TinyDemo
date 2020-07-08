using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using TinyDemo.Common.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;

namespace TinyDemo.Common
{
    public static class JWTStrategyRegistry
    {

        public static IServiceCollection AddJWTScheme(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettingsSection = configuration.GetSection("Jwt");

            var jwtSettings = jwtSettingsSection.Get<JWTSettings>();

            services.Configure<JWTSettings>(jwtSettingsSection);

            var encodedSecret = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                // TODO Set to true on production
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(encodedSecret),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token
                    // expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                };
            });


            return services;
        }
    }
}
