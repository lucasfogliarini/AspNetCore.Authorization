﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AspNetCore.Authorization.JsonWebToken
{
    public static class AuthenticationExtensions
    {
        public static void AddJwtAuthentication(this WebApplicationBuilder builder)
        {
            var jwtSection = builder.Configuration.GetSection(JwtConfiguration.JwtSection);
            var jwtConfiguration = jwtSection.Get<JwtConfiguration>()!;
            builder.Services.AddJwtService(jwtConfiguration);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = AuthService.GetSymmetricSecurityKey(jwtConfiguration.SecurityKey),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
        public static IServiceCollection AddJwtService(this IServiceCollection serviceCollection, JwtConfiguration jwtConfiguration)
        {
            serviceCollection.AddScoped<IAuthService, AuthService>();
            serviceCollection.AddSingleton(jwtConfiguration);
            return serviceCollection;
        }
        public static void AddSwaggerJwtAuthentication(this SwaggerGenOptions swaggerGenOptions)
        {
            swaggerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        }
    }
}
