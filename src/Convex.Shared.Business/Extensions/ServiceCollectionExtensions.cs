using Convex.Shared.Business.Interfaces;
using Convex.Shared.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Convex.Shared.Business.Extensions;

/// <summary>
/// Service collection extensions for business services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add business services to the service collection
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>Service collection</returns>
    /// <exception cref="ArgumentNullException">Thrown when services is null</exception>
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Register business services
        services.AddScoped<IBettingCalculator, BettingCalculator>();

        // Register configuration
        services.AddSingleton<BettingConfiguration>();

        return services;
    }

    /// <summary>
    /// Add business services with custom configuration
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configureOptions">Configuration action</param>
    /// <returns>Service collection</returns>
    /// <exception cref="ArgumentNullException">Thrown when services or configureOptions is null</exception>
    public static IServiceCollection AddBusinessServices(this IServiceCollection services, Action<BettingConfiguration> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureOptions);

        // Register business services
        services.AddScoped<IBettingCalculator, BettingCalculator>();

        // Register configuration with custom setup
        services.AddSingleton(provider =>
        {
            var config = new BettingConfiguration();
            configureOptions(config);
            return config;
        });

        return services;
    }
}