namespace Convex.Shared.Models.Enums;

/// <summary>
/// Bet status enumeration
/// </summary>
public enum BetStatus
{
    /// <summary>
    /// Bet is open/pending
    /// </summary>
    Open = 0,

    /// <summary>
    /// Bet won
    /// </summary>
    Won = 1,

    /// <summary>
    /// Bet lost
    /// </summary>
    Lost = 2,

    /// <summary>
    /// Bet refunded
    /// </summary>
    Refunded = 3,

    /// <summary>
    /// Bet cancelled
    /// </summary>
    Cancelled = 4
}
