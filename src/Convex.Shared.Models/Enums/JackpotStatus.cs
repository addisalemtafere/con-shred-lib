namespace Convex.Shared.Models.Enums;

/// <summary>
/// Jackpot status enumeration
/// </summary>
public enum JackpotStatus
{
    /// <summary>
    /// Home jackpot
    /// </summary>
    Home = 1,

    /// <summary>
    /// Draw jackpot
    /// </summary>
    Draw = 2,

    /// <summary>
    /// Away jackpot
    /// </summary>
    Away = 3,

    /// <summary>
    /// Pending jackpot
    /// </summary>
    Pending = 4
}

/// <summary>
/// Jackpot won status enumeration
/// </summary>
public enum JackpotWonStatus
{
    /// <summary>
    /// Won jackpot
    /// </summary>
    Won,

    /// <summary>
    /// Loss jackpot
    /// </summary>
    Loss,

    /// <summary>
    /// Pending jackpot
    /// </summary>
    Pending
}

/// <summary>
/// Jackpot choice enumeration
/// </summary>
public enum JackpotChoice
{
    /// <summary>
    /// Home choice
    /// </summary>
    Home,

    /// <summary>
    /// Draw choice
    /// </summary>
    Draw,

    /// <summary>
    /// Away choice
    /// </summary>
    Away
}