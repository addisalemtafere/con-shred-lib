using Convex.Shared.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Convex.Shared.Models.DTOs;

/// <summary>
/// User data transfer object for API communication
/// </summary>
public class UserDto
{
    /// <summary>
    /// User unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Phone number (primary identifier)
    /// </summary>
    [Required]
    [StringLength(15)]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Full name of the user
    /// </summary>
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Email address
    /// </summary>
    [StringLength(100)]
    public string? Email { get; set; }

    /// <summary>
    /// User type (Better, Agent, Client, etc.)
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// Whether the user is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Whether the user is verified
    /// </summary>
    public bool IsVerified { get; set; }

    /// <summary>
    /// Current balance
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// Bonus balance
    /// </summary>
    public decimal BonusBalance { get; set; }

    /// <summary>
    /// Payable balance (for agents)
    /// </summary>
    public decimal PayableBalance { get; set; }

    /// <summary>
    /// Agent ID (if user is a client)
    /// </summary>
    public Guid? AgentId { get; set; }

    /// <summary>
    /// Branch ID (if user belongs to a branch)
    /// </summary>
    public Guid? BranchId { get; set; }

    /// <summary>
    /// Last login date
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}