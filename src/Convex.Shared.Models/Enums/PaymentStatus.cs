namespace Convex.Shared.Models.Enums;

/// <summary>
/// Payment status enumeration
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// Pending payment
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Declined payment
    /// </summary>
    Declined = 2,

    /// <summary>
    /// Successful payment
    /// </summary>
    Successful = 3
}

/// <summary>
/// Payment type enumeration
/// </summary>
public enum PaymentType
{
    /// <summary>
    /// Regular payment
    /// </summary>
    RegularPay = 1,

    /// <summary>
    /// Cashback payment
    /// </summary>
    Cashback = 2,

    /// <summary>
    /// Cashout payment
    /// </summary>
    Cashout = 3
}

/// <summary>
/// Channel enumeration
/// </summary>
public enum Channel
{
    /// <summary>
    /// Web channel
    /// </summary>
    Web,

    /// <summary>
    /// Desktop channel
    /// </summary>
    Desktop,

    /// <summary>
    /// Mobile channel
    /// </summary>
    Mobile,

    /// <summary>
    /// Telegram bot channel
    /// </summary>
    TelegramBot,

    /// <summary>
    /// SMS channel
    /// </summary>
    Sms,

    /// <summary>
    /// Agent channel
    /// </summary>
    Agent
}

/// <summary>
/// Activity type enumeration
/// </summary>
public enum ActivityType
{
    /// <summary>
    /// Deposit activity
    /// </summary>
    Deposit,

    /// <summary>
    /// Withdrawal activity
    /// </summary>
    Withdrawal,

    /// <summary>
    /// Sport bet activity
    /// </summary>
    SportBet
}