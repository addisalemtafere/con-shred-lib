using Convex.Shared.Grpc.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Convex.Shared.Grpc.Services;

/// <summary>
/// Circuit breaker pattern implementation for gRPC services
/// </summary>
public class ConvexGrpcCircuitBreaker
{
    private readonly ILogger<ConvexGrpcCircuitBreaker> _logger;
    private readonly ConvexGrpcOptions _options;
    private readonly ConcurrentDictionary<string, CircuitBreakerState> _circuitStates = new();
    private readonly object _lock = new();

    /// <summary>
    /// Circuit breaker states
    /// </summary>
    public enum CircuitState
    {
        /// <summary>
        /// Normal operation
        /// </summary>
        Closed,
        
        /// <summary>
        /// Circuit is open, requests are blocked
        /// </summary>
        Open,
        
        /// <summary>
        /// Testing if service is back
        /// </summary>
        HalfOpen
    }

    /// <summary>
    /// Circuit breaker state for a service
    /// </summary>
    private class CircuitBreakerState
    {
        public CircuitState State { get; set; } = CircuitState.Closed;
        public int FailureCount { get; set; } = 0;
        public DateTime LastFailureTime { get; set; } = DateTime.MinValue;
        public int SuccessCount { get; set; } = 0;
    }

    /// <summary>
    /// Initializes a new instance of the ConvexGrpcCircuitBreaker
    /// </summary>
    /// <param name="logger">Logger instance</param>
    /// <param name="options">gRPC configuration options</param>
    public ConvexGrpcCircuitBreaker(
        ILogger<ConvexGrpcCircuitBreaker> logger,
        IOptions<ConvexGrpcOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    /// <summary>
    /// Check if circuit breaker allows the request
    /// </summary>
    /// <param name="serviceName">Service name</param>
    /// <returns>True if request is allowed, false if circuit is open</returns>
    public bool IsRequestAllowed(string serviceName)
    {
        var state = _circuitStates.GetOrAdd(serviceName, _ => new CircuitBreakerState());

        lock (_lock)
        {
            switch (state.State)
            {
                case CircuitState.Closed:
                    return true;

                case CircuitState.Open:
                    // Check if timeout has passed
                    if (DateTime.UtcNow - state.LastFailureTime > TimeSpan.FromSeconds(_options.CircuitBreakerTimeoutSeconds))
                    {
                        state.State = CircuitState.HalfOpen;
                        state.SuccessCount = 0;
                        _logger.LogInformation("Circuit breaker for {ServiceName} moved to HalfOpen state", serviceName);
                        return true;
                    }
                    return false;

                case CircuitState.HalfOpen:
                    return true;

                default:
                    return true;
            }
        }
    }

    /// <summary>
    /// Record a successful request
    /// </summary>
    /// <param name="serviceName">Service name</param>
    public void RecordSuccess(string serviceName)
    {
        var state = _circuitStates.GetOrAdd(serviceName, _ => new CircuitBreakerState());

        lock (_lock)
        {
            if (state.State == CircuitState.HalfOpen)
            {
                state.SuccessCount++;
                
                // If we have enough successes, close the circuit
                if (state.SuccessCount >= _options.CircuitBreakerThreshold)
                {
                    state.State = CircuitState.Closed;
                    state.FailureCount = 0;
                    state.SuccessCount = 0;
                    _logger.LogInformation("Circuit breaker for {ServiceName} moved to Closed state", serviceName);
                }
            }
            else if (state.State == CircuitState.Closed)
            {
                // Reset failure count on success
                state.FailureCount = 0;
            }
        }
    }

    /// <summary>
    /// Record a failed request
    /// </summary>
    /// <param name="serviceName">Service name</param>
    public void RecordFailure(string serviceName)
    {
        var state = _circuitStates.GetOrAdd(serviceName, _ => new CircuitBreakerState());

        lock (_lock)
        {
            state.FailureCount++;
            state.LastFailureTime = DateTime.UtcNow;

            if (state.State == CircuitState.Closed && state.FailureCount >= _options.CircuitBreakerThreshold)
            {
                state.State = CircuitState.Open;
                _logger.LogWarning("Circuit breaker for {ServiceName} moved to Open state after {FailureCount} failures", 
                    serviceName, state.FailureCount);
            }
            else if (state.State == CircuitState.HalfOpen)
            {
                // Any failure in half-open state opens the circuit
                state.State = CircuitState.Open;
                _logger.LogWarning("Circuit breaker for {ServiceName} moved to Open state from HalfOpen", serviceName);
            }
        }
    }

    /// <summary>
    /// Get circuit breaker state for a service
    /// </summary>
    /// <param name="serviceName">Service name</param>
    /// <returns>Current circuit state</returns>
    public CircuitState GetCircuitState(string serviceName)
    {
        var state = _circuitStates.GetOrAdd(serviceName, _ => new CircuitBreakerState());
        return state.State;
    }

    /// <summary>
    /// Get circuit breaker statistics
    /// </summary>
    /// <returns>Circuit breaker statistics</returns>
    public Dictionary<string, object> GetStatistics()
    {
        var stats = new Dictionary<string, object>();
        
        foreach (var circuit in _circuitStates)
        {
            stats[circuit.Key] = new
            {
                State = circuit.Value.State.ToString(),
                FailureCount = circuit.Value.FailureCount,
                SuccessCount = circuit.Value.SuccessCount,
                LastFailureTime = circuit.Value.LastFailureTime
            };
        }
        
        return stats;
    }

    /// <summary>
    /// Reset circuit breaker for a service
    /// </summary>
    /// <param name="serviceName">Service name</param>
    public void ResetCircuit(string serviceName)
    {
        var state = _circuitStates.GetOrAdd(serviceName, _ => new CircuitBreakerState());

        lock (_lock)
        {
            state.State = CircuitState.Closed;
            state.FailureCount = 0;
            state.SuccessCount = 0;
            state.LastFailureTime = DateTime.MinValue;
            _logger.LogInformation("Circuit breaker for {ServiceName} has been reset", serviceName);
        }
    }
}
