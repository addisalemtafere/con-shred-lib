using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Convex.Shared.Http.Extensions;

/// <summary>
/// Extension methods for configuring Convex HTTP client
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Convex HTTP client with default configuration
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configureOptions">Optional configuration action</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexHttpClient(
        this IServiceCollection services,
        Action<ConvexHttpClientOptions>? configureOptions = null)
    {
        // Configure options
        if (configureOptions != null)
        {
            services.Configure(configureOptions);
        }

        // Register the HTTP client
        services.AddHttpClient<IConvexHttpClient, ConvexHttpClient>();

        return services;
    }

    /// <summary>
    /// Adds Convex HTTP client with specific base address
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="baseAddress">The base address for the HTTP client</param>
    /// <param name="apiKey">Optional API key</param>
    /// <param name="timeout">Optional timeout (default: 30 seconds)</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexHttpClient(
        this IServiceCollection services,
        string baseAddress,
        string? apiKey = null,
        TimeSpan? timeout = null)
    {
        services.Configure<ConvexHttpClientOptions>(options =>
        {
            options.BaseAddress = new Uri(baseAddress);
            options.ApiKey = apiKey;
            options.Timeout = timeout ?? TimeSpan.FromSeconds(30);
        });

        services.AddHttpClient<IConvexHttpClient, ConvexHttpClient>();

        return services;
    }

    /// <summary>
    /// Adds Convex HTTP client for service-to-service communication
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="serviceName">The name of the service</param>
    /// <param name="baseAddress">The base address for the service</param>
    /// <param name="apiKey">Optional API key</param>
    /// <param name="timeout">Optional timeout (default: 30 seconds)</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexServiceClient(
        this IServiceCollection services,
        string serviceName,
        string baseAddress,
        string? apiKey = null,
        TimeSpan? timeout = null)
    {
        services.AddHttpClient(serviceName, client =>
        {
            client.BaseAddress = new Uri(baseAddress);
            client.Timeout = timeout ?? TimeSpan.FromSeconds(30);

            if (!string.IsNullOrEmpty(apiKey))
            {
                client.DefaultRequestHeaders.Add("X-API-Key", apiKey);
            }
        });

        return services;
    }

    /// <summary>
    /// Adds Convex HTTP client with configuration from appsettings.json
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <param name="sectionName">The configuration section name (default: "ConvexHttpClient")</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexHttpClient(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = "ConvexHttpClient")
    {
        services.Configure<ConvexHttpClientOptions>(configuration.GetSection(sectionName));
        services.AddHttpClient<IConvexHttpClient, ConvexHttpClient>();

        return services;
    }
}