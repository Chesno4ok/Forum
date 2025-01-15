
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Forum.Application.Auth;
using Microsoft.IdentityModel.Tokens;

namespace Forum.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureScalar(this WebApplication app)
        {
            app.MapScalarApiReference(options =>
            {
                options.WithOpenApiRoutePattern("/swagger/v1/swagger.json");
                options.Theme = ScalarTheme.Solarized;
                options
                    .WithPreferredScheme("Authorization") // Security scheme name from the OpenAPI document
                    .WithApiKeyAuthentication(apiKey =>
                    {
                        apiKey.Token = "your-api-key";
                    });

                // Object initializer
                options.Authentication = new ScalarAuthenticationOptions
                {
                    PreferredSecurityScheme = "Authorization", // Security scheme name from the OpenAPI document
                    ApiKey = new ApiKeyOptions
                    {
                        Token = "your-api-key"
                    }
                };

            });
        }
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Chesnok Forum", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }
        public static void ConfigureAuth(this IServiceCollection services)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });

            
            services.AddAuthorization();
        }
    }
}
