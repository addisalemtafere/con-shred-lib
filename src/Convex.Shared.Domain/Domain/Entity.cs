namespace Convex.Shared.Domain;

/// <summary>
/// Base entity class for all domain entities
/// </summary>
/// <typeparam name="TId">The type of the entity identifier</typeparam>
public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{
    /// <summary>
    /// Gets the entity identifier
    /// </summary>
    public TId Id { get; protected set; } = default!;

    /// <summary>
    /// Initializes a new instance of the Entity class
    /// </summary>
    protected Entity()
    {
        // Protected constructor for ORM frameworks
    }

    /// <summary>
    /// Initializes a new instance of the Entity class with an identifier
    /// </summary>
    /// <param name="id">The entity identifier</param>
    protected Entity(TId id)
    {
        Id = id;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current entity
    /// </summary>
    /// <param name="obj">The object to compare with the current entity</param>
    /// <returns>True if the objects are equal; otherwise, false</returns>
    public override bool Equals(object? obj)
    {
        return Equals(obj as Entity<TId>);
    }

    /// <summary>
    /// Determines whether the specified entity is equal to the current entity
    /// </summary>
    /// <param name="other">The entity to compare with the current entity</param>
    /// <returns>True if the entities are equal; otherwise, false</returns>
    public bool Equals(Entity<TId>? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        if (Id.Equals(default(TId)) || other.Id.Equals(default(TId)))
            return false;

        return Id.Equals(other.Id);
    }

    /// <summary>
    /// Returns the hash code for the current entity
    /// </summary>
    /// <returns>A hash code for the current entity</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// Determines whether two entities are equal
    /// </summary>
    /// <param name="left">The first entity to compare</param>
    /// <param name="right">The second entity to compare</param>
    /// <returns>True if the entities are equal; otherwise, false</returns>
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two entities are not equal
    /// </summary>
    /// <param name="left">The first entity to compare</param>
    /// <param name="right">The second entity to compare</param>
    /// <returns>True if the entities are not equal; otherwise, false</returns>
    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
    {
        return !Equals(left, right);
    }
}