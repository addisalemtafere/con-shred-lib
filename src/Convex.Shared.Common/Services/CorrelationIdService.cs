using System.Diagnostics;

namespace Convex.Shared.Common.Services;

/// <summary>
/// Service for managing correlation IDs across Convex microservices
/// </summary>
public interface ICorrelationIdService
{
    /// <summary>
    /// Gets the current correlation ID
    /// </summary>
    string? GetCorrelationId();

    /// <summary>
    /// Sets the correlation ID for the current context
    /// </summary>
    /// <param name="correlationId">The correlation ID to set</param>
    void SetCorrelationId(string correlationId);

    /// <summary>
    /// Generates a new correlation ID
    /// </summary>
    /// <returns>A new correlation ID</returns>
    string GenerateCorrelationId();

    /// <summary>
    /// Creates a new correlation ID scope
    /// </summary>
    /// <param name="correlationId">Optional correlation ID, generates new one if null</param>
    /// <returns>Disposable scope</returns>
    IDisposable CreateScope(string? correlationId = null);
}

/// <summary>
/// Implementation of correlation ID service using AsyncLocal
/// </summary>
public class CorrelationIdService : ICorrelationIdService
{
    private static readonly AsyncLocal<string?> _correlationId = new();

    public string? GetCorrelationId()
    {
        return _correlationId.Value;
    }

    public void SetCorrelationId(string correlationId)
    {
        _correlationId.Value = correlationId;
    }

    public string GenerateCorrelationId()
    {
        return $"{Environment.MachineName}-{Process.GetCurrentProcess().Id}-{DateTimeOffset.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}";
    }

    public IDisposable CreateScope(string? correlationId = null)
    {
        var previousCorrelationId = _correlationId.Value;
        var newCorrelationId = correlationId ?? GenerateCorrelationId();
        
        _correlationId.Value = newCorrelationId;
        
        return new CorrelationIdScope(previousCorrelationId);
    }

    private class CorrelationIdScope : IDisposable
    {
        private readonly string? _previousCorrelationId;

        public CorrelationIdScope(string? previousCorrelationId)
        {
            _previousCorrelationId = previousCorrelationId;
        }

        public void Dispose()
        {
            _correlationId.Value = _previousCorrelationId;
        }
    }
}
