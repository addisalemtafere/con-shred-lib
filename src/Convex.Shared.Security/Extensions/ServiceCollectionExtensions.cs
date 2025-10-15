using Convex.Shared.Security.Configuration;
using Convex.Shared.Security.Interfaces;
using Convex.Shared.Security.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Convex.Shared.Security.Extensions;

/// <summary>
/// Extension methods for configuring Convex security services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Convex security services
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configureOptions">Optional configuration action</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexSecurity(
        this IServiceCollection services,
        Action<ConvexSecurityOptions>? configureOptions = null)
    {
        // Configure options
        if (configureOptions != null)
        {
            services.Configure(configureOptions);
        }

        // Register services
        services.AddSingleton<IConvexSecurityService, ConvexSecurityService>();

        return services;
    }

    /// <summary>
    /// Adds Convex JWT authentication
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configureOptions">Optional configuration action</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexJwtAuthentication(
        this IServiceCollection services,
        Action<ConvexSecurityOptions>? configureOptions = null)
    {
        // Configure options
        if (configureOptions != null)
        {
            services.Configure(configureOptions);
        }

        // Add JWT authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var securityOptions = serviceProvider.GetRequiredService<IOptions<ConvexSecurityOptions>>().Value;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = securityOptions.JwtIssuer,
                    ValidAudience = securityOptions.JwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityOptions.JwtSecret)),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Add("Token-Expired", "true");
                }
                        return Task.CompletedTask;
                    }
                };
            });

        // Add authorization
        services.AddAuthorization();

        return services;
    }

    /// <summary>
    /// Adds Convex API key authentication
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configureOptions">Optional configuration action</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexApiKeyAuthentication(
        this IServiceCollection services,
        Action<ConvexSecurityOptions>? configureOptions = null)
    {
        // Configure options
        if (configureOptions != null)
        {
            services.Configure(configureOptions);
        }

        // Add API key authentication
        services.AddAuthentication("ApiKey")
            .AddScheme<AuthenticationSchemeOptions, AuthenticationHandler<AuthenticationSchemeOptions>>(
                "ApiKey", options => { });

        return services;
    }

    /// <summary>
    /// Adds Convex CORS configuration
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configureOptions">Optional configuration action</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexCors(
        this IServiceCollection services,
        Action<ConvexSecurityOptions>? configureOptions = null)
    {
        // Configure options
        if (configureOptions != null)
        {
            services.Configure(configureOptions);
        }

        // Add CORS
        services.AddCors(options =>
        {
            options.AddPolicy("ConvexPolicy", builder =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var securityOptions = serviceProvider.GetRequiredService<IOptions<ConvexSecurityOptions>>().Value;

                builder.WithOrigins(securityOptions.AllowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        return services;
    }
}
