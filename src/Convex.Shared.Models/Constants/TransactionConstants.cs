namespace Convex.Shared.Models.Constants;

/// <summary>
/// Transaction-related constants
/// </summary>
public static class TransactionConstants
{
    /// <summary>
    /// Balance transfer fee
    /// </summary>
    public const decimal BalanceTransFee = 5m;
    
    /// <summary>
    /// Withdraw request expiration period in hours
    /// </summary>
    public const int WithdrawRequestExpirationPeriodHours = 24;
    
    /// <summary>
    /// Sales online period
    /// </summary>
    public const int SalesOnlinePeriod = 3;
    
    /// <summary>
    /// No branch limit
    /// </summary>
    public const int NoBranchLimit = -1;
    
    /// <summary>
    /// No credit limit
    /// </summary>
    public const int NoCreditLimit = -1;
    
    /// <summary>
    /// Client setting cancel win pre-match
    /// </summary>
    public const int ClientSettingCancelWinPrematch = -1;
}
