namespace Convex.Shared.Domain;

/// <summary>
/// Base aggregate root class for all domain aggregates
/// </summary>
/// <typeparam name="TId">The type of the aggregate identifier</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>
    where TId : notnull
{
    private readonly List<DomainEvent> _domainEvents = new();

    /// <summary>
    /// Initializes a new instance of the AggregateRoot class
    /// </summary>
    protected AggregateRoot()
    {
    }

    /// <summary>
    /// Initializes a new instance of the AggregateRoot class with an identifier
    /// </summary>
    /// <param name="id">The aggregate identifier</param>
    protected AggregateRoot(TId id) : base(id)
    {
    }

    /// <summary>
    /// Gets the read-only collection of domain events
    /// </summary>
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Adds a domain event to the aggregate
    /// </summary>
    /// <param name="domainEvent">The domain event to add</param>
    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Removes a domain event from the aggregate
    /// </summary>
    /// <param name="domainEvent">The domain event to remove</param>
    protected void RemoveDomainEvent(DomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        _domainEvents.Remove(domainEvent);
    }

    /// <summary>
    /// Clears all domain events from the aggregate
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
