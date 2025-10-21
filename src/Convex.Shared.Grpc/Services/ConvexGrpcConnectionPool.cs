using Convex.Shared.Grpc.Configuration;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Convex.Shared.Grpc.Services;

/// <summary>
/// High-performance connection pool for gRPC channels with load balancing
/// </summary>
public class ConvexGrpcConnectionPool : IDisposable
{
    private readonly ILogger<ConvexGrpcConnectionPool> _logger;
    private readonly ConvexGrpcOptions _options;
    private readonly ConcurrentDictionary<string, ConcurrentQueue<GrpcChannel>> _channelPools = new();
    private readonly ConcurrentDictionary<string, int> _activeConnections = new();
    private readonly object _lock = new();
    private bool _disposed = false;

    /// <summary>
    /// Initializes a new instance of the ConvexGrpcConnectionPool
    /// </summary>
    /// <param name="logger">Logger instance</param>
    /// <param name="options">gRPC configuration options</param>
    public ConvexGrpcConnectionPool(
        ILogger<ConvexGrpcConnectionPool> logger,
        IOptions<ConvexGrpcOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    /// <summary>
    /// Get a gRPC channel from the connection pool
    /// </summary>
    /// <param name="endpoint">Service endpoint</param>
    /// <returns>Available gRPC channel</returns>
    public GrpcChannel GetChannel(Uri endpoint)
    {
        if (!_options.EnableConnectionPooling)
        {
            return CreateNewChannel(endpoint);
        }

        var key = endpoint.ToString();
        var pool = _channelPools.GetOrAdd(key, _ => new ConcurrentQueue<GrpcChannel>());

        // Try to get existing channel from pool
        if (pool.TryDequeue(out var existingChannel) && 
            existingChannel.State != ConnectivityState.Shutdown)
        {
            _logger.LogDebug("Reusing gRPC channel from pool for {Endpoint}", endpoint);
            return existingChannel;
        }

        // Create new channel if pool is empty or channel is shutdown
        var newChannel = CreateNewChannel(endpoint);
        _logger.LogDebug("Created new gRPC channel for {Endpoint}", endpoint);
        return newChannel;
    }

    /// <summary>
    /// Return a gRPC channel to the connection pool
    /// </summary>
    /// <param name="endpoint">Service endpoint</param>
    /// <param name="channel">Channel to return</param>
    public void ReturnChannel(Uri endpoint, GrpcChannel channel)
    {
        if (!_options.EnableConnectionPooling || _disposed)
        {
            try
            {
                channel.ShutdownAsync().Wait(TimeSpan.FromSeconds(5));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error shutting down gRPC channel");
            }
            return;
        }

        var key = endpoint.ToString();
        var pool = _channelPools.GetOrAdd(key, _ => new ConcurrentQueue<GrpcChannel>());

        // Check pool size limit
        if (pool.Count < _options.ConnectionPoolSize)
        {
            pool.Enqueue(channel);
            _logger.LogDebug("Returned gRPC channel to pool for {Endpoint}", endpoint);
        }
        else
        {
            // Pool is full, shutdown the channel
            try
            {
                channel.ShutdownAsync().Wait(TimeSpan.FromSeconds(5));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error shutting down gRPC channel");
            }
        }
    }

    /// <summary>
    /// Get load-balanced endpoint for service discovery
    /// </summary>
    /// <param name="serviceName">Service name</param>
    /// <returns>Load-balanced endpoint</returns>
    public Uri GetLoadBalancedEndpoint(string serviceName)
    {
        if (!_options.EnableLoadBalancing)
        {
            return GetDefaultEndpoint(serviceName);
        }

        // Simple round-robin load balancing
        var serviceConfigs = _options.Services.Where(s => s.Name == serviceName).ToList();
        if (serviceConfigs.Count == 0)
        {
            return GetDefaultEndpoint(serviceName);
        }

        var index = Math.Abs(serviceName.GetHashCode()) % serviceConfigs.Count;
        var selectedConfig = serviceConfigs[index];
        
        _logger.LogDebug("Load balanced to {Endpoint} for service {ServiceName}", 
            selectedConfig.Endpoint, serviceName);
        
        return new Uri(selectedConfig.Endpoint);
    }

    private GrpcChannel CreateNewChannel(Uri endpoint)
    {
        var channelOptions = new GrpcChannelOptions
        {
            MaxReceiveMessageSize = _options.MaxReceiveMessageSize,
            MaxSendMessageSize = _options.MaxSendMessageSize,
            Credentials = _options.EnableTls ? ChannelCredentials.SecureSsl : ChannelCredentials.Insecure,
            HttpHandler = CreateOptimizedHttpHandler()
        };

        return GrpcChannel.ForAddress(endpoint, channelOptions);
    }

    private HttpClientHandler CreateOptimizedHttpHandler()
    {
        var handler = new HttpClientHandler
        {
            MaxConnectionsPerServer = _options.MaxConcurrentConnections,
            UseCookies = false,
            UseDefaultCredentials = false
        };

        return handler;
    }

    private Uri GetDefaultEndpoint(string serviceName)
    {
        return serviceName switch
        {
            "UserService" => new Uri("http://localhost:50052"),
            "PaymentService" => new Uri("http://localhost:50054"),
            "BettingService" => new Uri("http://localhost:50053"),
            "GameService" => new Uri("http://localhost:50055"),
            "NotificationService" => new Uri("http://localhost:50056"),
            "AdminService" => new Uri("http://localhost:50057"),
            "AuthService" => new Uri("http://localhost:50051"),
            _ => throw new InvalidOperationException($"Service '{serviceName}' not found")
        };
    }

    /// <summary>
    /// Get connection pool statistics
    /// </summary>
    /// <returns>Pool statistics</returns>
    public Dictionary<string, int> GetPoolStatistics()
    {
        var stats = new Dictionary<string, int>();
        
        foreach (var pool in _channelPools)
        {
            stats[pool.Key] = pool.Value.Count;
        }
        
        return stats;
    }

    /// <summary>
    /// Dispose the connection pool and all channels
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose the connection pool resources
    /// </summary>
    /// <param name="disposing">True if called from Dispose(), false if called from finalizer</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _disposed = true;
            
            foreach (var pool in _channelPools.Values)
            {
                while (pool.TryDequeue(out var channel))
                {
                    try
                    {
                        channel.ShutdownAsync().Wait(TimeSpan.FromSeconds(5));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error shutting down gRPC channel during disposal");
                    }
                }
            }
            
            _channelPools.Clear();
            _activeConnections.Clear();
        }
    }
}
