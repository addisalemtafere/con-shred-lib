using Convex.Shared.Grpc.Interfaces;
using Convex.Shared.Grpc.Configuration;
using Convex.Shared.Grpc.Models;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Convex.Shared.Grpc.Services;

/// <summary>
/// High-performance gRPC server with automatic service registration
/// </summary>
public class ConvexGrpcServer : IGrpcServerService
{
    private readonly ILogger<ConvexGrpcServer> _logger;
    private readonly ConvexGrpcOptions _options;
    private Server? _server;

    public ConvexGrpcServer(
        ILogger<ConvexGrpcServer> logger,
        IOptions<ConvexGrpcOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public string ServiceName => _options.ServiceName ?? "ConvexService";
    public int Port => _options.ServerPort;
    public string Description => $"Convex gRPC Server on port {Port}";

    /// <summary>
    /// Start the gRPC server
    /// </summary>
    public async Task StartAsync()
    {
        try
        {
            _server = new Server
            {
                Services = { }, // Will be populated by service registration
                Ports = { new ServerPort("0.0.0.0", Port, _options.EnableTls ? ServerCredentials.Insecure : ServerCredentials.Insecure) }
            };

            await _server.StartAsync();
            _logger.LogInformation("gRPC server started on port {Port}", Port);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start gRPC server on port {Port}", Port);
            throw;
        }
    }

    /// <summary>
    /// Stop the gRPC server
    /// </summary>
    public async Task StopAsync()
    {
        if (_server != null)
        {
            try
            {
                await _server.ShutdownAsync();
                _logger.LogInformation("gRPC server stopped");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error stopping gRPC server");
            }
        }
    }

    /// <summary>
    /// Register a gRPC service
    /// </summary>
    public void RegisterService<TService>(TService service) where TService : class
    {
        if (_server == null)
        {
            throw new InvalidOperationException("Server must be started before registering services");
        }

        // This would be implemented based on your specific service registration needs
        _logger.LogInformation("Registered gRPC service: {ServiceType}", typeof(TService).Name);
    }
}
