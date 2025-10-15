using Convex.Shared.Logging.Interfaces;
using Convex.Shared.Logging.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Convex.Shared.Logging.Extensions;

/// <summary>
/// Extension methods for configuring Convex logging
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Convex logging services
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="serviceName">The service name</param>
    /// <param name="version">The service version</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexLogging(
        this IServiceCollection services,
        string serviceName = "ConvexService",
        string version = "1.0.0")
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.WithProperty("ServiceName", serviceName)
            .Enrich.WithProperty("Version", version)
            .Enrich.WithProperty("MachineName", Environment.MachineName)
            .Enrich.WithProperty("ProcessId", Environment.ProcessId)
            .WriteTo.Console(outputTemplate: 
                "[{Timestamp:HH:mm:ss} {Level:u3}] {ServiceName} {Message:lj} {Properties:j}{NewLine}{Exception}")
            .WriteTo.File("logs/convex-.txt", 
                rollingInterval: RollingInterval.Day,
                outputTemplate: 
                "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {ServiceName} {Message:lj} {Properties:j}{NewLine}{Exception}")
            .CreateLogger();

        // Add Serilog to DI
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog();
        });

        // Register Convex logger
        services.AddSingleton<IConvexLogger>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<ConvexLogger>>();
            return new ConvexLogger(logger, serviceName, version);
        });

        return services;
    }

    /// <summary>
    /// Adds Convex logging with custom configuration
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configureLogger">Logger configuration action</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexLogging(
        this IServiceCollection services,
        Action<LoggerConfiguration> configureLogger)
    {
        var loggerConfig = new LoggerConfiguration();
        configureLogger(loggerConfig);
        
        Log.Logger = loggerConfig.CreateLogger();

        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog();
        });

        services.AddSingleton<IConvexLogger, ConvexLogger>();

        return services;
    }
}
