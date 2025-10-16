namespace Convex.Shared.Models.Constants;

/// <summary>
/// Payment-related constants
/// </summary>
public static class PaymentConstants
{
    /// <summary>
    /// Default transaction fee percentage
    /// </summary>
    public const decimal DefaultTransactionFeePercent = 0.02m; // 2%

    /// <summary>
    /// Default transaction fee fixed amount
    /// </summary>
    public const decimal DefaultTransactionFeeFixed = 5.00m;

    /// <summary>
    /// Minimum deposit amount
    /// </summary>
    public const decimal MinDepositAmount = 10.00m;

    /// <summary>
    /// Maximum deposit amount
    /// </summary>
    public const decimal MaxDepositAmount = 50000.00m;

    /// <summary>
    /// Minimum withdrawal amount
    /// </summary>
    public const decimal MinWithdrawalAmount = 20.00m;

    /// <summary>
    /// Maximum withdrawal amount
    /// </summary>
    public const decimal MaxWithdrawalAmount = 100000.00m;

    /// <summary>
    /// Transaction timeout in minutes
    /// </summary>
    public const int TransactionTimeoutMinutes = 30;

    /// <summary>
    /// Maximum retry attempts
    /// </summary>
    public const int MaxRetryAttempts = 3;

    /// <summary>
    /// Retry delay in seconds
    /// </summary>
    public const int RetryDelaySeconds = 5;

    /// <summary>
    /// Default currency
    /// </summary>
    public const string DefaultCurrency = "ETB";

    /// <summary>
    /// Supported currencies
    /// </summary>
    public static readonly string[] SupportedCurrencies = { "ETB", "USD", "EUR" };

    /// <summary>
    /// Payment statuses that are considered final
    /// </summary>
    public static readonly string[] FinalPaymentStatuses = { "completed", "failed", "cancelled", "refunded" };
}
