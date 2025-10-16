using System.ComponentModel.DataAnnotations;

namespace Convex.Shared.Common.Models;

/// <summary>
/// Base entity class for all domain entities
/// </summary>
public abstract class BaseEntity<TKey> 
{
    public required TKey Id { get; set; } 

    public required string CreatedBy { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required string CreatedWs { get; set; }
    public required string LastModifiedBy { get; set; }
    public required DateTime LastModifiedAt { get; set; }
    public required string LastModifiedWs { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    
    [Timestamp]
    public byte[]? RowVersion {  get; set; } // used for optimistic concurrency
}
