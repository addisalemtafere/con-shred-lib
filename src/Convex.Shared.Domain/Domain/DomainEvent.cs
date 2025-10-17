using MediatR;

namespace Convex.Shared.Domain;

/// <summary>
/// Base class for all domain events
/// </summary>
public abstract class DomainEvent : INotification
{
    /// <summary>
    /// Gets the unique identifier for this domain event
    /// </summary>
    public Guid Id { get; protected set; } = Guid.NewGuid();

    /// <summary>
    /// Gets the date and time when the domain event occurred
    /// </summary>
    public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the correlation ID for tracking related events across services
    /// </summary>
    public Guid? CorrelationId { get; set; }

    /// <summary>
    /// Gets or sets the causation ID for tracking the event that caused this event
    /// </summary>
    public Guid? CausationId { get; set; }

    /// <summary>
    /// Sets the correlation ID for this domain event
    /// </summary>
    /// <param name="correlationId">The correlation identifier</param>
    public void SetCorrelationId(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    /// <summary>
    /// Sets the causation ID for this domain event
    /// </summary>
    /// <param name="causationId">The causation identifier</param>
    public void SetCausationId(Guid causationId)
    {
        CausationId = causationId;
    }
}