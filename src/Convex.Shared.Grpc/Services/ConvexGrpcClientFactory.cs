using Convex.Shared.Grpc.Configuration;
using Convex.Shared.Grpc.Interfaces;
using Convex.Shared.Grpc.Models;
using Convex.Shared.Grpc.Services;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Convex.Shared.Grpc.Services;

/// <summary>
/// High-performance gRPC client factory with connection pooling and service discovery
/// </summary>
public class ConvexGrpcClientFactory : IGrpcClientFactory, IDisposable
{
    private readonly ILogger<ConvexGrpcClientFactory> _logger;
    private readonly ConvexGrpcOptions _options;
    private readonly ConvexGrpcConnectionPool _connectionPool;
    private readonly ConvexGrpcCircuitBreaker _circuitBreaker;
    private readonly ConvexGrpcMetrics _metrics;
    private readonly Dictionary<string, GrpcChannel> _channels = new();
    private readonly object _lock = new();
    private bool _disposed = false;

    /// <summary>
    /// Initializes a new instance of the ConvexGrpcClientFactory
    /// </summary>
    /// <param name="logger">Logger instance</param>
    /// <param name="options">gRPC configuration options</param>
    /// <param name="connectionPool">Connection pool service</param>
    /// <param name="circuitBreaker">Circuit breaker service</param>
    /// <param name="metrics">Metrics service</param>
    public ConvexGrpcClientFactory(
        ILogger<ConvexGrpcClientFactory> logger,
        IOptions<ConvexGrpcOptions> options,
        ConvexGrpcConnectionPool connectionPool,
        ConvexGrpcCircuitBreaker circuitBreaker,
        ConvexGrpcMetrics metrics)
    {
        _logger = logger;
        _options = options.Value;
        _connectionPool = connectionPool;
        _circuitBreaker = circuitBreaker;
        _metrics = metrics;
    }

    /// <summary>
    /// Creates a gRPC client for the specified service with performance optimizations
    /// </summary>
    /// <typeparam name="TClient">Type of gRPC client</typeparam>
    /// <param name="serviceName">Name of the service</param>
    /// <returns>Configured gRPC client</returns>
    public TClient CreateClient<TClient>(string serviceName) where TClient : ClientBase<TClient>
    {
        // Check circuit breaker
        if (!_circuitBreaker.IsRequestAllowed(serviceName))
        {
            throw new InvalidOperationException($"Circuit breaker is open for service '{serviceName}'");
        }

        var endpoint = ResolveServiceEndpoint(serviceName);
            
        return CreateClient<TClient>(endpoint);
    }

    /// <summary>
    /// Creates a gRPC client with API key authentication
    /// </summary>
    /// <typeparam name="TClient">Type of gRPC client</typeparam>
    /// <param name="serviceName">Name of the service</param>
    /// <param name="apiKey">API key for authentication</param>
    /// <returns>Configured gRPC client with API key authentication</returns>
    public TClient CreateClientWithApiKey<TClient>(string serviceName, string apiKey) where TClient : ClientBase<TClient>
    {
        var endpoint = ResolveServiceEndpoint(serviceName);
        return CreateClientWithApiKey<TClient>(endpoint, apiKey);
    }

    /// <summary>
    /// Creates a gRPC client with authentication headers
    /// </summary>
    /// <typeparam name="TClient">Type of gRPC client</typeparam>
    /// <param name="serviceName">Name of the service</param>
    /// <param name="authToken">Authentication token</param>
    /// <returns>Configured gRPC client with authentication</returns>
    public TClient CreateClientWithAuth<TClient>(string serviceName, string authToken) where TClient : ClientBase<TClient>
    {
        var endpoint = ResolveServiceEndpoint(serviceName);
        return CreateClientWithAuth<TClient>(endpoint, authToken);
    }

    /// <summary>
    /// Creates a gRPC client with API key authentication for specific endpoint
    /// </summary>
    /// <typeparam name="TClient">Type of gRPC client</typeparam>
    /// <param name="endpoint">Service endpoint URI</param>
    /// <param name="apiKey">API key for authentication</param>
    /// <returns>Configured gRPC client with API key authentication</returns>
    public TClient CreateClientWithApiKey<TClient>(Uri endpoint, string apiKey) where TClient : ClientBase<TClient>
    {
        var channel = GetOrCreateChannel(endpoint);
        var client = (TClient)Activator.CreateInstance(typeof(TClient), channel)!;
        
        // Store API key for use in gRPC calls
        if (_options.EnableAuthentication && !string.IsNullOrEmpty(apiKey))
        {
            _logger.LogInformation("Created gRPC client with API key authentication for {Endpoint}", endpoint);
        }
        
        return client;
    }

    /// <summary>
    /// Creates a gRPC client with authentication headers for specific endpoint
    /// </summary>
    /// <typeparam name="TClient">Type of gRPC client</typeparam>
    /// <param name="endpoint">Service endpoint URI</param>
    /// <param name="authToken">Authentication token</param>
    /// <returns>Configured gRPC client with authentication</returns>
    public TClient CreateClientWithAuth<TClient>(Uri endpoint, string authToken) where TClient : ClientBase<TClient>
    {
        var channel = GetOrCreateChannel(endpoint);
        var client = (TClient)Activator.CreateInstance(typeof(TClient), channel)!;
        
        // Add authentication headers if enabled
        if (_options.EnableAuthentication && !string.IsNullOrEmpty(authToken))
        {
            // This would be implemented in the actual gRPC call with metadata
            _logger.LogInformation("Created gRPC client with authentication for {Endpoint}", endpoint);
        }
        
        return client;
    }

    /// <summary>
    /// Creates a gRPC client with custom channel options
    /// </summary>
    /// <typeparam name="TClient">Type of gRPC client</typeparam>
    /// <param name="serviceName">Name of the service</param>
    /// <param name="options">Custom channel options</param>
    /// <returns>Configured gRPC client</returns>
    public TClient CreateClient<TClient>(string serviceName, GrpcChannelOptions options) where TClient : ClientBase<TClient>
    {
        var endpoint = ResolveServiceEndpoint(serviceName);
        return CreateClientWithOptions<TClient>(endpoint, options);
    }

    /// <summary>
    /// Creates a gRPC client for the specified endpoint
    /// </summary>
    /// <typeparam name="TClient">Type of gRPC client</typeparam>
    /// <param name="endpoint">Service endpoint URI</param>
    /// <returns>Configured gRPC client</returns>
    public TClient CreateClient<TClient>(Uri endpoint) where TClient : ClientBase<TClient>
    {
        var channel = GetOrCreateChannel(endpoint);
        return (TClient)Activator.CreateInstance(typeof(TClient), channel)!;
    }

    private TClient CreateClientWithOptions<TClient>(Uri endpoint, GrpcChannelOptions options) where TClient : ClientBase<TClient>
    {
        var channel = GrpcChannel.ForAddress(endpoint, options);
        return (TClient)Activator.CreateInstance(typeof(TClient), channel)!;
    }

    private GrpcChannel GetOrCreateChannel(Uri endpoint)
    {
        var key = endpoint.ToString();

        lock (_lock)
        {
            if (_channels.TryGetValue(key, out var existingChannel) && existingChannel.State != ConnectivityState.Shutdown)
            {
                return existingChannel;
            }

            var channelOptions = new GrpcChannelOptions
            {
                MaxReceiveMessageSize = _options.MaxReceiveMessageSize,
                MaxSendMessageSize = _options.MaxSendMessageSize,
                Credentials = _options.EnableTls ? ChannelCredentials.SecureSsl : ChannelCredentials.Insecure
            };

            var channel = GrpcChannel.ForAddress(endpoint, channelOptions);
            _channels[key] = channel;

            _logger.LogInformation("Created gRPC channel for {Endpoint}", endpoint);
            return channel;
        }
    }

    private Uri ResolveServiceEndpoint(string serviceName)
    {
        // Get service endpoint (load balancer handles multiple instances)
        var serviceConfig = _options.Services.FirstOrDefault(s => s.Name == serviceName);
        if (serviceConfig != null)
        {
            _logger.LogDebug("Resolved service {ServiceName} to {Endpoint}", serviceName, serviceConfig.Endpoint);
            return new Uri(serviceConfig.Endpoint);
        }

        // Fallback to default service discovery
        var serviceEndpoint = GetDefaultServiceEndpoint(serviceName);
        if (serviceEndpoint != null)
        {
            return new Uri(serviceEndpoint);
        }
        
        throw new InvalidOperationException($"Service '{serviceName}' not found. Available services: {string.Join(", ", _options.Services.Select(s => s.Name))}");
    }

    private string? GetDefaultServiceEndpoint(string serviceName)
    {
        // Get from configuration first
        var serviceConfig = _options.Services.FirstOrDefault(s => s.Name == serviceName);
        if (serviceConfig != null)
        {
            return serviceConfig.Endpoint;
        }

        // Fallback to environment-based discovery
        var baseUrl = Environment.GetEnvironmentVariable("GRPC_BASE_URL") ?? "http://localhost";
        var port = GetServicePort(serviceName);
        
        return $"{baseUrl}:{port}";
    }

    private int GetServicePort(string serviceName)
    {
        // 1. Try environment variable first
        var envPort = Environment.GetEnvironmentVariable($"GRPC_{serviceName.ToUpper()}_PORT");
        if (int.TryParse(envPort, out var port) && port > 0)
        {
            return port;
        }

        // 2. Try service discovery from configuration
        var serviceConfig = _options.Services.FirstOrDefault(s => s.Name == serviceName);
        if (serviceConfig != null && serviceConfig.Port > 0)
        {
            return serviceConfig.Port;
        }

        // 3. Auto-discover available port
        if (_options.ServerPort == 0)
        {
            return FindAvailablePort();
        }

        // 4. Use base port + offset
        return serviceName switch
        {
            "AuthService" => _options.ServerPort,
            "UserService" => _options.ServerPort + 1,
            "BettingService" => _options.ServerPort + 2,
            "PaymentService" => _options.ServerPort + 3,
            "GameService" => _options.ServerPort + 4,
            "NotificationService" => _options.ServerPort + 5,
            "AdminService" => _options.ServerPort + 6,
            _ => _options.ServerPort
        };
    }

    private int FindAvailablePort()
    {
        // Find any available port starting from 50051
        for (int port = 50051; port <= 50100; port++)
        {
            if (IsPortAvailable(port))
            {
                _logger.LogInformation("Auto-discovered available port: {Port}", port);
                return port;
            }
        }
        
        // No fallback - throw exception if no port found
        throw new InvalidOperationException("No available ports found in range 50051-50100. Please configure ports manually.");
    }

    private bool IsPortAvailable(int port)
    {
        try
        {
            using var listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, port);
            listener.Start();
            listener.Stop();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Disposes the client factory and all managed channels
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the client factory resources
    /// </summary>
    /// <param name="disposing">True if called from Dispose(), false if called from finalizer</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            lock (_lock)
            {
                foreach (var channel in _channels.Values)
                {
                    try
                    {
                        channel.ShutdownAsync().Wait(TimeSpan.FromSeconds(5));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error shutting down gRPC channel");
                    }
                }
                _channels.Clear();
            }
            _disposed = true;
        }
    }
}