namespace Convex.Shared.Models.Constants;

/// <summary>
/// Betting-related constants
/// </summary>
public static class BettingConstants
{
    /// <summary>
    /// Default possible win amount
    /// </summary>
    public const decimal DefaultPossibleWin = 10000m;
    
    /// <summary>
    /// Maximum win amount
    /// </summary>
    public const decimal MaxWin = 350000m;
    
    /// <summary>
    /// Taxable win threshold
    /// </summary>
    public const decimal TaxableWin = 1000m;
    
    /// <summary>
    /// Win tax rate
    /// </summary>
    public const decimal WinTax = 0.15m;
    
    /// <summary>
    /// Total tax rate
    /// </summary>
    public const decimal TotTax = 0.1m;
    
    /// <summary>
    /// Minimum stake amount
    /// </summary>
    public const decimal MinStake = 10m;
    
    /// <summary>
    /// Cashout minimum threshold
    /// </summary>
    public const decimal CashoutMinThreshold = 5000m;
    
    /// <summary>
    /// Cashout won percentage
    /// </summary>
    public const decimal CashoutWonPercent = 70m;
    
    /// <summary>
    /// Cashout maximum odd
    /// </summary>
    public const decimal CashoutMaxOdd = 1.8m;
    
    /// <summary>
    /// Cashout minimum matches
    /// </summary>
    public const int CashoutMinMatches = 4;
    
    /// <summary>
    /// Last minute match number
    /// </summary>
    public const int LastMinuteMatchNum = 20;
    
    /// <summary>
    /// Award ticket after minutes
    /// </summary>
    public const int AwardTicketAfterMin = 5;
    
    /// <summary>
    /// Ticket settlable within minutes
    /// </summary>
    public const int TicketSettlableWithin = 360;
    
    /// <summary>
    /// Jackpot ended within minutes
    /// </summary>
    public const int JackpotEndedWithin = 60;
    
    /// <summary>
    /// Minimum allowed transfer amount
    /// </summary>
    public const decimal MinAllowedTransferAmount = 20m;
    
    /// <summary>
    /// Client dashboard default report days
    /// </summary>
    public const int ClientDashboardDefaultReportDay = 1;
    
    /// <summary>
    /// Insufficient balance error code
    /// </summary>
    public const int InsufficientBalanceCode = 0;
    
    /// <summary>
    /// Function return refund code
    /// </summary>
    public const int FuncReturnRefund = -1;
}