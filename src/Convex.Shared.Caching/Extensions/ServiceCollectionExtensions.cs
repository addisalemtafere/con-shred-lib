using Convex.Shared.Caching.Interfaces;
using Convex.Shared.Caching.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Convex.Shared.Caching.Extensions;

/// <summary>
/// Extension methods for configuring Convex caching services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Convex memory caching
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexMemoryCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<IDistributedCache, MemoryDistributedCache>();
        services.AddSingleton<IConvexCache, ConvexCache>();

        return services;
    }

    /// <summary>
    /// Adds Convex Redis caching
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="connectionString">Redis connection string</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexRedisCache(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
        });

        services.AddSingleton<IConvexCache, ConvexCache>();

        return services;
    }

    /// <summary>
    /// Adds Convex Redis caching with configuration
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configureOptions">Redis configuration action</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexRedisCache(
        this IServiceCollection services,
        Action<RedisCacheOptions> configureOptions)
    {
        services.AddStackExchangeRedisCache(configureOptions);
        services.AddSingleton<IConvexCache, ConvexCache>();

        return services;
    }
}
