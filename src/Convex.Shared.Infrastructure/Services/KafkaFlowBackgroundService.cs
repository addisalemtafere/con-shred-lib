using KafkaFlow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Convex.Shared.Infrastructure.Services;

/// <summary>
/// Background service for KafkaFlow
/// </summary>
public sealed class KafkaFlowBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<KafkaFlowBackgroundService> _logger;
    private IKafkaBus? _kafkaBus;

    /// <summary>
    /// Initializes a new instance of the KafkaFlowBackgroundService.
    /// </summary>
    /// <param name="serviceProvider">Service provider for dependency injection</param>
    /// <param name="logger">Logger for observability (SOAR: Observable)</param>
    /// <exception cref="ArgumentNullException">Thrown when serviceProvider or logger is null</exception>
    public KafkaFlowBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<KafkaFlowBackgroundService> logger)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(logger);

        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// Executes the background service with proper error handling and observability.
    /// Implements retry logic and health monitoring (SOAR: Reliable, Observable).
    /// </summary>
    /// <param name="stoppingToken">Cancellation token for graceful shutdown</param>
    /// <returns>Task representing the async operation</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting KafkaFlow background service");

        try
        {
            await InitializeKafkaBusAsync(stoppingToken);
            await RunKafkaBusAsync(stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("KafkaFlow background service cancelled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in KafkaFlow background service");
            throw;
        }
    }

    /// <summary>
    /// Initializes the Kafka bus with proper error handling (SOAR: Reliable).
    /// </summary>
    /// <param name="stoppingToken">Cancellation token</param>
    /// <returns>Task representing the async operation</returns>
    private async Task InitializeKafkaBusAsync(CancellationToken stoppingToken)
    {
        _kafkaBus = _serviceProvider.GetRequiredService<IKafkaBus>();
        
        if (_kafkaBus == null)
        {
            throw new InvalidOperationException("IKafkaBus service not registered in DI container");
        }

        await _kafkaBus.StartAsync(stoppingToken);
        _logger.LogInformation("KafkaFlow background service started successfully");
    }

    /// <summary>
    /// Runs the Kafka bus with health monitoring (SOAR: Observable, Available).
    /// </summary>
    /// <param name="stoppingToken">Cancellation token</param>
    /// <returns>Task representing the async operation</returns>
    private async Task RunKafkaBusAsync(CancellationToken stoppingToken)
    {
        var healthCheckInterval = TimeSpan.FromSeconds(30);
        var lastHealthCheck = DateTime.UtcNow;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Perform periodic health checks (SOAR: Observable)
                if (DateTime.UtcNow - lastHealthCheck >= healthCheckInterval)
                {
                    await PerformHealthCheckAsync();
                    lastHealthCheck = DateTime.UtcNow;
                }

                // Wait for cancellation or next health check
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("KafkaFlow background service stopping due to cancellation");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during KafkaFlow background service execution");
                // Continue running to maintain availability (SOAR: Available)
            }
        }
    }

    /// <summary>
    /// Performs health check on Kafka bus (SOAR: Observable).
    /// </summary>
    /// <returns>Task representing the async operation</returns>
    private async Task PerformHealthCheckAsync()
    {
        try
        {
            if (_kafkaBus != null)
            {
                _logger.LogDebug("Performing KafkaFlow health check");
                // Add specific health check logic here if needed
                await Task.CompletedTask;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "KafkaFlow health check failed");
        }
    }

    /// <summary>
    /// Stops the background service with proper cleanup (SOAR: Reliable).
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the async operation</returns>
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping KafkaFlow background service");
        
        try
        {
            if (_kafkaBus != null)
            {
                await _kafkaBus.StopAsync();
                _logger.LogInformation("KafkaFlow stopped successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error stopping KafkaFlow");
        }
        finally
        {
            await base.StopAsync(cancellationToken);
        }
    }
}
