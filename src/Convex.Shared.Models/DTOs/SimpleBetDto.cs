using Convex.Shared.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Convex.Shared.Models.DTOs;

/// <summary>
/// Simple bet data transfer object for API communication
/// </summary>
public class SimpleBetDto
{
    /// <summary>
    /// Bet unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Ticket this bet belongs to
    /// </summary>
    public Guid TicketId { get; set; }

    /// <summary>
    /// Match ID from the sports data provider
    /// </summary>
    [Required]
    [StringLength(50)]
    public string MatchId { get; set; } = string.Empty;

    /// <summary>
    /// Match name/description
    /// </summary>
    [Required]
    [StringLength(200)]
    public string MatchName { get; set; } = string.Empty;

    /// <summary>
    /// Home team name
    /// </summary>
    [Required]
    [StringLength(100)]
    public string HomeTeam { get; set; } = string.Empty;

    /// <summary>
    /// Away team name
    /// </summary>
    [Required]
    [StringLength(100)]
    public string AwayTeam { get; set; } = string.Empty;

    /// <summary>
    /// Bet type (1X2, Over/Under, etc.)
    /// </summary>
    [Required]
    [StringLength(20)]
    public string BetType { get; set; } = string.Empty;

    /// <summary>
    /// Selected outcome
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Selection { get; set; } = string.Empty;

    /// <summary>
    /// Odds for this selection
    /// </summary>
    public decimal Odds { get; set; }

    /// <summary>
    /// Stake amount for this bet
    /// </summary>
    public decimal Stake { get; set; }

    /// <summary>
    /// Potential win amount
    /// </summary>
    public decimal PotentialWin { get; set; }

    /// <summary>
    /// Bet status
    /// </summary>
    public BetStatus Status { get; set; }

    /// <summary>
    /// Match start time
    /// </summary>
    public DateTime MatchStartTime { get; set; }

    /// <summary>
    /// Match end time
    /// </summary>
    public DateTime? MatchEndTime { get; set; }

    /// <summary>
    /// Final score (if available)
    /// </summary>
    [StringLength(20)]
    public string? FinalScore { get; set; }

    /// <summary>
    /// Result (if available)
    /// </summary>
    [StringLength(50)]
    public string? Result { get; set; }

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
