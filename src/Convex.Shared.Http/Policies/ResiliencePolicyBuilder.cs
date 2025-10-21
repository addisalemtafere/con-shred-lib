using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace Convex.Shared.Http.Policies;

/// <summary>
/// Builder for creating resilience policies for HTTP clients
/// </summary>
public static class ResiliencePolicyBuilder
{
    /// <summary>
    /// Creates a comprehensive resilience policy with retry, circuit breaker, and timeout
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> CreateResiliencePolicy(
        ConvexHttpClientOptions options,
        ILogger logger)
    {
        var policies = new List<IAsyncPolicy<HttpResponseMessage>>();

        // Add retry policy
        if (options.RetryPolicy.MaxRetryAttempts > 0)
        {
            var retryPolicy = CreateRetryPolicy(options.RetryPolicy, logger);
            policies.Add(retryPolicy);
        }

        // Add circuit breaker policy
        if (options.CircuitBreaker.Enabled)
        {
            var circuitBreakerPolicy = CreateCircuitBreakerPolicy(options.CircuitBreaker, logger);
            policies.Add(circuitBreakerPolicy);
        }

        // Add timeout policy
        if (options.TimeoutPolicy.Enabled)
        {
            var timeoutPolicy = CreateTimeoutPolicy(options.TimeoutPolicy, logger);
            policies.Add(timeoutPolicy);
        }

        // Combine all policies
        return policies.Count > 0 
            ? Policy.WrapAsync(policies.ToArray())
            : Policy.NoOpAsync<HttpResponseMessage>();
    }

    /// <summary>
    /// Creates a retry policy with exponential backoff and jitter
    /// </summary>
    private static IAsyncPolicy<HttpResponseMessage> CreateRetryPolicy(
        RetryPolicyOptions options,
        ILogger logger)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(response => options.RetryableStatusCodes.Contains((int)response.StatusCode))
            .WaitAndRetryAsync(
                retryCount: options.MaxRetryAttempts,
                sleepDurationProvider: retryAttempt =>
                {
                    if (!options.UseExponentialBackoff)
                        return options.BaseDelay;

                    var delay = TimeSpan.FromMilliseconds(
                        options.BaseDelay.TotalMilliseconds * Math.Pow(2, retryAttempt - 1));

                    // Apply jitter to prevent thundering herd
                    var jitter = Random.Shared.NextDouble() * options.JitterFactor;
                    delay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * (1 + jitter));

                    return TimeSpan.FromMilliseconds(Math.Min(delay.TotalMilliseconds, options.MaxDelay.TotalMilliseconds));
                },
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    logger.LogWarning(
                        "Retry {RetryCount} for {RequestUri} after {Delay}ms. Reason: {Reason}",
                        retryCount,
                        context.GetValueOrDefault("RequestUri", "Unknown"),
                        timespan.TotalMilliseconds,
                        outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString());
                });
    }

    /// <summary>
    /// Creates a circuit breaker policy
    /// </summary>
    private static IAsyncPolicy<HttpResponseMessage> CreateCircuitBreakerPolicy(
        CircuitBreakerOptions options,
        ILogger logger)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: options.FailureThreshold,
                durationOfBreak: options.DurationOfBreak,
                onBreak: (exception, duration) =>
                {
                    logger.LogError(
                        "Circuit breaker opened for {Duration}ms. Reason: {Reason}",
                        duration.TotalMilliseconds,
                        exception.Exception?.Message ?? exception.Result?.StatusCode.ToString());
                },
                onReset: () =>
                {
                    logger.LogInformation("Circuit breaker reset - requests will be allowed");
                },
                onHalfOpen: () =>
                {
                    logger.LogInformation("Circuit breaker half-open - testing with next request");
                });
    }

    /// <summary>
    /// Creates a timeout policy
    /// </summary>
    private static IAsyncPolicy<HttpResponseMessage> CreateTimeoutPolicy(
        TimeoutPolicyOptions options,
        ILogger logger)
    {
        return Policy.TimeoutAsync<HttpResponseMessage>(options.Timeout);
    }
}
