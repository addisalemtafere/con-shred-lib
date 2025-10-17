namespace Convex.Shared.Models.Enums;

/// <summary>
/// Transaction type enumeration
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Deposit transaction
    /// </summary>
    Deposit = 1,

    /// <summary>
    /// Withdrawal transaction
    /// </summary>
    Withdrawal = 2,

    /// <summary>
    /// Bet stake transaction
    /// </summary>
    BetStake = 3,

    /// <summary>
    /// Win payout transaction
    /// </summary>
    WinPayout = 4,

    /// <summary>
    /// Refund transaction
    /// </summary>
    Refund = 5,

    /// <summary>
    /// Cashout transaction
    /// </summary>
    Cashout = 6,

    /// <summary>
    /// Bonus transaction
    /// </summary>
    Bonus = 7,

    /// <summary>
    /// Commission transaction
    /// </summary>
    Commission = 8,

    /// <summary>
    /// Tax transaction
    /// </summary>
    Tax = 9,

    /// <summary>
    /// Fee transaction
    /// </summary>
    Fee = 10
}