namespace Convex.Shared.Models.Enums;

/// <summary>
/// Betting status enumeration
/// </summary>
public enum BettingStatus
{
    /// <summary>
    /// Open bet
    /// </summary>
    Open = 0,
    
    /// <summary>
    /// Won bet
    /// </summary>
    Win = 1,
    
    /// <summary>
    /// Lost bet
    /// </summary>
    Loss = 2,
    
    /// <summary>
    /// Refunded bet
    /// </summary>
    Refund = 3,
    
    /// <summary>
    /// Cancelled bet
    /// </summary>
    Cancelled = 4
}

/// <summary>
/// Win status enumeration
/// </summary>
public enum WinStatus
{
    /// <summary>
    /// Loss
    /// </summary>
    Loss = 2,
    
    /// <summary>
    /// Win
    /// </summary>
    Win = 1,
    
    /// <summary>
    /// Refund
    /// </summary>
    Refund = 3,
    
    /// <summary>
    /// Pending
    /// </summary>
    Pending = 4
}

/// <summary>
/// Match status enumeration
/// </summary>
public enum MatchStatus
{
    /// <summary>
    /// Not started match
    /// </summary>
    NotStarted = 0,
    
    /// <summary>
    /// Ended match
    /// </summary>
    Ended = 3,
    
    /// <summary>
    /// Closed match
    /// </summary>
    Closed = 4,
    
    /// <summary>
    /// Cancelled match
    /// </summary>
    Cancelled = 5,
    
    /// <summary>
    /// Delayed match
    /// </summary>
    Delayed = 6
}

/// <summary>
/// Odd result enumeration
/// </summary>
public enum OddResult
{
    /// <summary>
    /// Win
    /// </summary>
    Win = 1,
    
    /// <summary>
    /// Loss
    /// </summary>
    Loss = 0,
    
    /// <summary>
    /// Open
    /// </summary>
    Open = -1,
    
    /// <summary>
    /// Refund
    /// </summary>
    Refund = -2
}
