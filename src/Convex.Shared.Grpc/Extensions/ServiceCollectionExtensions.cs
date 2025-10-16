using Convex.Shared.Grpc.Configuration;
using Convex.Shared.Grpc.Interfaces;
using Convex.Shared.Grpc.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Convex.Shared.Grpc.Extensions;

/// <summary>
/// Extension methods for configuring Convex gRPC services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Convex gRPC services with default configuration
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexGrpc(this IServiceCollection services)
    {
        return services.AddConvexGrpc(options => { });
    }

    /// <summary>
    /// Add Convex gRPC services with custom configuration
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configureOptions">Action to configure gRPC options</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexGrpc(this IServiceCollection services, Action<ConvexGrpcOptions> configureOptions)
    {
        // Configure options
        services.Configure(configureOptions);

        // Register gRPC client factory as singleton
        services.AddSingleton<IGrpcClientFactory, ConvexGrpcClientFactory>();

        // Register gRPC server as singleton
        services.AddSingleton<IGrpcServerService, ConvexGrpcServer>();

        // Register gRPC services
        services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.MaxReceiveMessageSize = 4 * 1024 * 1024; // 4MB
            options.MaxSendMessageSize = 4 * 1024 * 1024; // 4MB
        });

        return services;
    }

    /// <summary>
    /// Add Convex gRPC services with configuration from appsettings.json
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <param name="sectionName">The configuration section name</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexGrpc(this IServiceCollection services, IConfiguration configuration, string sectionName = "ConvexGrpc")
    {
        // Configure from appsettings.json
        services.Configure<ConvexGrpcOptions>(configuration.GetSection(sectionName));

        // Register gRPC client factory as singleton
        services.AddSingleton<IGrpcClientFactory, ConvexGrpcClientFactory>();

        // Register gRPC server as singleton
        services.AddSingleton<IGrpcServerService, ConvexGrpcServer>();

        // Register gRPC services
        services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.MaxReceiveMessageSize = 4 * 1024 * 1024; // 4MB
            options.MaxSendMessageSize = 4 * 1024 * 1024; // 4MB
        });

        return services;
    }

    /// <summary>
    /// Add gRPC client for specific service with proper configuration
    /// </summary>
    /// <typeparam name="TClient">The gRPC client type</typeparam>
    /// <param name="services">The service collection</param>
    /// <param name="serviceName">The service name</param>
    /// <param name="endpoint">The service endpoint</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddGrpcClient<TClient>(this IServiceCollection services, string serviceName, string endpoint)
        where TClient : class
    {
        services.AddGrpcClient<TClient>(options =>
        {
            options.Address = new Uri(endpoint);
        });

        return services;
    }
}