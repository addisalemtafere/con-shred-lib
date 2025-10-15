namespace Convex.Shared.Common.Models;

/// <summary>
/// Base entity class for all domain entities
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string CreatedBy { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required string CreatedWs { get; set; }
    public required string LastModifiedBy { get; set; }
    public required DateTime LastModifiedAt { get; set; }
    public required string LastModifiedWs { get; set; }
}
