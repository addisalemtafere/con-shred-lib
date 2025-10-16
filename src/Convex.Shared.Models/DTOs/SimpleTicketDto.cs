using Convex.Shared.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Convex.Shared.Models.DTOs;

/// <summary>
/// Simple ticket data transfer object for API communication
/// </summary>
public class SimpleTicketDto
{
    /// <summary>
    /// Ticket unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User who placed the ticket
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Ticket number (unique identifier)
    /// </summary>
    [Required]
    [StringLength(20)]
    public string TicketNumber { get; set; } = string.Empty;

    /// <summary>
    /// Total stake amount
    /// </summary>
    public decimal Stake { get; set; }

    /// <summary>
    /// Potential win amount
    /// </summary>
    public decimal PotentialWin { get; set; }

    /// <summary>
    /// Ticket status
    /// </summary>
    public TicketStatus Status { get; set; }

    /// <summary>
    /// Notes or comments
    /// </summary>
    [StringLength(500)]
    public string? Notes { get; set; }

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}