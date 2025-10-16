using Convex.Shared.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Convex.Shared.Common.Extensions;

/// <summary>
/// Extension methods for configuring Convex Common services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Convex Common services including correlation ID support
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexCommon(this IServiceCollection services)
    {
        // Register correlation ID service as singleton
        services.AddSingleton<ICorrelationIdService, CorrelationIdService>();
        
        return services;
    }

}
