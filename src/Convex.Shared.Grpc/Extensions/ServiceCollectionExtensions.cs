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

        // Register core gRPC services
        services.AddSingleton<ConvexGrpcConnectionPool>();
        services.AddSingleton<ConvexGrpcCircuitBreaker>();
        services.AddSingleton<ConvexGrpcMetrics>();
        services.AddSingleton<ConvexGrpcPerformanceOptimizer>();
        
        // Register Kubernetes services
        services.AddSingleton<ConvexGrpcKubernetesServiceDiscovery>();
        services.AddSingleton<ConvexGrpcHealthCheckService>();
        services.AddSingleton<ConvexGrpcServiceManager>();
        services.AddSingleton<ConvexGrpcLoadBalancer>();
        services.AddSingleton<ConvexGrpcLoadBalancedClientFactory>();
        
        // Register gRPC client factory as singleton
        services.AddSingleton<IGrpcClientFactory, ConvexGrpcClientFactory>();

        // Register gRPC server as singleton
        services.AddSingleton<IGrpcServerService, ConvexGrpcServer>();

        // Register gRPC services with dynamic configuration
        services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            // Message sizes will be configured from ConvexGrpcOptions
        });

        return services;
    }

    /// <summary>
    /// Add Convex gRPC services with simple API key authentication
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="isProduction">True for production (with auth/TLS), false for development</param>
    /// <param name="serviceApiKey">API key for this service (used when calling other services)</param>
    /// <param name="validApiKeys">Valid API keys for incoming requests (other services calling this service)</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexGrpc(this IServiceCollection services, bool isProduction = false, string? serviceApiKey = null, List<string>? validApiKeys = null)
    {
        return services.AddConvexGrpc(options =>
        {
            if (isProduction)
            {
                // Production configuration
                options.EnableTls = true; // Enable TLS for production
                options.EnableAuthentication = true; // Enable authentication for production
                options.EnableCompression = true; // Enable compression for production
                options.ServiceApiKey = serviceApiKey; // Set service API key
                options.ValidApiKeys = validApiKeys ?? new List<string>(); // Set valid API keys
            }
            else
            {
                // Development configuration
                options.EnableTls = false; // Disable TLS for local development
                options.EnableAuthentication = false; // Disable authentication for local development
                options.EnableCompression = false; // Disable compression for local development
            }
            options.ServerPort = 50051; // Default port
        });
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

        // Register core gRPC services
        services.AddSingleton<ConvexGrpcConnectionPool>();
        services.AddSingleton<ConvexGrpcCircuitBreaker>();
        services.AddSingleton<ConvexGrpcMetrics>();
        services.AddSingleton<ConvexGrpcPerformanceOptimizer>();
        
        // Register Kubernetes services
        services.AddSingleton<ConvexGrpcKubernetesServiceDiscovery>();
        services.AddSingleton<ConvexGrpcHealthCheckService>();
        services.AddSingleton<ConvexGrpcServiceManager>();
        services.AddSingleton<ConvexGrpcLoadBalancer>();
        services.AddSingleton<ConvexGrpcLoadBalancedClientFactory>();
        
        // Register gRPC client factory as singleton
        services.AddSingleton<IGrpcClientFactory, ConvexGrpcClientFactory>();

        // Register gRPC server as singleton
        services.AddSingleton<IGrpcServerService, ConvexGrpcServer>();

        // Register gRPC services with dynamic configuration
        services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            // Message sizes will be configured from ConvexGrpcOptions
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