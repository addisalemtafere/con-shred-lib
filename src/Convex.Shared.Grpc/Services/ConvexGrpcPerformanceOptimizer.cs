using Convex.Shared.Grpc.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Convex.Shared.Grpc.Services;

/// <summary>
/// High-performance optimizer for gRPC operations
/// </summary>
public class ConvexGrpcPerformanceOptimizer
{
    private readonly ILogger<ConvexGrpcPerformanceOptimizer> _logger;
    private readonly ConvexGrpcOptions _options;
    private readonly ConcurrentDictionary<string, object> _responseCache = new();
    private readonly object _lock = new();

    /// <summary>
    /// Initializes a new instance of the ConvexGrpcPerformanceOptimizer
    /// </summary>
    /// <param name="logger">Logger instance</param>
    /// <param name="options">gRPC configuration options</param>
    public ConvexGrpcPerformanceOptimizer(
        ILogger<ConvexGrpcPerformanceOptimizer> logger,
        IOptions<ConvexGrpcOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    /// <summary>
    /// Execute gRPC call with performance optimizations
    /// </summary>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    /// <param name="serviceName">Service name</param>
    /// <param name="operationName">Operation name</param>
    /// <param name="request">Request object</param>
    /// <param name="grpcCall">gRPC call function</param>
    /// <param name="cacheKey">Optional cache key</param>
    /// <returns>Response object</returns>
    public async Task<TResponse> ExecuteWithOptimizations<TRequest, TResponse>(
        string serviceName,
        string operationName,
        TRequest request,
        Func<TRequest, Task<TResponse>> grpcCall,
        string? cacheKey = null)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestId = Guid.NewGuid().ToString();

        try
        {
            // Check cache first
            if (_options.EnableCaching && !string.IsNullOrEmpty(cacheKey))
            {
                if (_responseCache.TryGetValue(cacheKey, out var cachedResponse))
                {
                    _logger.LogDebug("Cache hit for {Operation} in {ServiceName}", operationName, serviceName);
                    return (TResponse)cachedResponse;
                }
            }

            // Execute gRPC call with timeout
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_options.RequestTimeoutSeconds));
            var response = await grpcCall(request);

            // Cache response if enabled
            if (_options.EnableCaching && !string.IsNullOrEmpty(cacheKey))
            {
                _responseCache.TryAdd(cacheKey, response!);
                
                // Schedule cache cleanup
                _ = Task.Delay(TimeSpan.FromSeconds(_options.CacheTtlSeconds))
                    .ContinueWith(_ => _responseCache.TryRemove(cacheKey, out var _));
            }

            stopwatch.Stop();
            _logger.LogDebug("Completed {Operation} in {ServiceName} in {ElapsedMs}ms", 
                operationName, serviceName, stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (OperationCanceledException) when (stopwatch.ElapsedMilliseconds >= _options.RequestTimeoutSeconds * 1000)
        {
            stopwatch.Stop();
            _logger.LogWarning("Timeout for {Operation} in {ServiceName} after {ElapsedMs}ms", 
                operationName, serviceName, stopwatch.ElapsedMilliseconds);
            throw new TimeoutException($"gRPC call to {serviceName}.{operationName} timed out after {_options.RequestTimeoutSeconds} seconds");
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Error in {Operation} for {ServiceName} after {ElapsedMs}ms", 
                operationName, serviceName, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    /// <summary>
    /// Execute gRPC call with retry logic
    /// </summary>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    /// <param name="serviceName">Service name</param>
    /// <param name="operationName">Operation name</param>
    /// <param name="request">Request object</param>
    /// <param name="grpcCall">gRPC call function</param>
    /// <returns>Response object</returns>
    public async Task<TResponse> ExecuteWithRetry<TRequest, TResponse>(
        string serviceName,
        string operationName,
        TRequest request,
        Func<TRequest, Task<TResponse>> grpcCall)
    {
        var attempt = 0;
        var maxAttempts = _options.MaxRetryAttempts;
        var baseDelay = _options.InitialBackoffMs;

        while (attempt < maxAttempts)
        {
            try
            {
                return await ExecuteWithOptimizations(serviceName, operationName, request, grpcCall);
            }
            catch (Exception ex) when (attempt < maxAttempts - 1)
            {
                attempt++;
                var delay = CalculateBackoffDelay(attempt, baseDelay);
                
                _logger.LogWarning(ex, "Attempt {Attempt} failed for {Operation} in {ServiceName}, retrying in {DelayMs}ms", 
                    attempt, operationName, serviceName, delay);

                await Task.Delay(delay);
            }
        }

        // Final attempt
        return await ExecuteWithOptimizations(serviceName, operationName, request, grpcCall);
    }

    /// <summary>
    /// Execute multiple gRPC calls in parallel
    /// </summary>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    /// <param name="serviceName">Service name</param>
    /// <param name="requests">List of requests</param>
    /// <param name="grpcCall">gRPC call function</param>
    /// <param name="maxConcurrency">Maximum concurrent operations</param>
    /// <returns>List of responses</returns>
    public async Task<List<TResponse>> ExecuteParallel<TRequest, TResponse>(
        string serviceName,
        List<TRequest> requests,
        Func<TRequest, Task<TResponse>> grpcCall,
        int? maxConcurrency = null)
    {
        var concurrency = maxConcurrency ?? _options.MaxConcurrentConnections;
        var semaphore = new SemaphoreSlim(concurrency, concurrency);
        var tasks = new List<Task<TResponse>>();

        foreach (var request in requests)
        {
            tasks.Add(ExecuteWithSemaphore(semaphore, serviceName, request, grpcCall));
        }

        var results = await Task.WhenAll(tasks);
        return results.ToList();
    }

    private async Task<TResponse> ExecuteWithSemaphore<TRequest, TResponse>(
        SemaphoreSlim semaphore,
        string serviceName,
        TRequest request,
        Func<TRequest, Task<TResponse>> grpcCall)
    {
        await semaphore.WaitAsync();
        try
        {
            return await ExecuteWithOptimizations(serviceName, "ParallelOperation", request, grpcCall);
        }
        finally
        {
            semaphore.Release();
        }
    }

    private int CalculateBackoffDelay(int attempt, int baseDelay)
    {
        var delay = (int)(baseDelay * Math.Pow(_options.BackoffMultiplier, attempt - 1));
        return Math.Min(delay, _options.MaxBackoffMs);
    }

    /// <summary>
    /// Clear response cache
    /// </summary>
    public void ClearCache()
    {
        lock (_lock)
        {
            _responseCache.Clear();
            _logger.LogInformation("Response cache cleared");
        }
    }

    /// <summary>
    /// Get cache statistics
    /// </summary>
    /// <returns>Cache statistics</returns>
    public object GetCacheStatistics()
    {
        return new
        {
            CacheEnabled = _options.EnableCaching,
            CacheSize = _responseCache.Count,
            CacheTtlSeconds = _options.CacheTtlSeconds,
            Timestamp = DateTime.UtcNow
        };
    }
}
