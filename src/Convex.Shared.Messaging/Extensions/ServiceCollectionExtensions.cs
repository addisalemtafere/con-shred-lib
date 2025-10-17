using Convex.Shared.Messaging.Configuration;
using Convex.Shared.Messaging.Interfaces;
using Convex.Shared.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Convex.Shared.Messaging.Extensions;

/// <summary>
/// Extension methods for configuring Convex messaging services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Convex messaging services
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configureOptions">Optional configuration action</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexMessaging(
        this IServiceCollection services,
        Action<ConvexMessagingOptions>? configureOptions = null)
    {
        // Configure options
        if (configureOptions != null)
        {
            services.Configure(configureOptions);
        }

        // Register services
        services.AddSingleton<IConvexMessageBus, ConvexMessageBus>();

        return services;
    }
}