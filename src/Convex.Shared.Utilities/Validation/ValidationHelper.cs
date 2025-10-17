namespace Convex.Shared.Utilities.Validation;

/// <summary>
/// Validation utility functions
/// </summary>
public static class ValidationHelper
{
    /// <summary>
    /// Validates if stake is within minimum and maximum limits
    /// </summary>
    /// <param name="stake">Stake amount</param>
    /// <param name="minStake">Minimum stake</param>
    /// <param name="maxStake">Maximum stake</param>
    /// <returns>True if valid</returns>
    public static bool ValidateStakeLimits(decimal stake, decimal minStake, decimal maxStake)
    {
        return stake >= minStake && stake <= maxStake;
    }

    /// <summary>
    /// Validates if withdrawal amount is within daily limit
    /// </summary>
    /// <param name="amount">Withdrawal amount</param>
    /// <param name="dailyLimit">Daily withdrawal limit</param>
    /// <returns>True if valid</returns>
    public static bool ValidateDailyWithdrawalLimit(decimal amount, decimal dailyLimit)
    {
        if (dailyLimit == 0)
            return true;

        return amount <= dailyLimit;
    }

    /// <summary>
    /// Validates if deposit amount is within single deposit limit
    /// </summary>
    /// <param name="amount">Deposit amount</param>
    /// <param name="singleDepositLimit">Single deposit limit</param>
    /// <returns>True if valid</returns>
    public static bool ValidateSingleDepositLimit(decimal amount, decimal singleDepositLimit)
    {
        if (singleDepositLimit == 0)
            return true;

        return amount <= singleDepositLimit;
    }

    /// <summary>
    /// Validates if pending stake is within allowed limit
    /// </summary>
    /// <param name="newStake">New stake amount</param>
    /// <param name="currentPendingStake">Current pending stake</param>
    /// <param name="maxPendingStake">Maximum pending stake</param>
    /// <returns>True if valid</returns>
    public static bool ValidatePendingStakeLimit(decimal newStake, decimal currentPendingStake, decimal maxPendingStake)
    {
        if (maxPendingStake == 0)
            return true;

        return maxPendingStake > currentPendingStake + newStake;
    }

    /// <summary>
    /// Validates if number of pending tickets is within limit
    /// </summary>
    /// <param name="currentPendingCount">Current pending ticket count</param>
    /// <param name="maxPendingTickets">Maximum pending tickets</param>
    /// <returns>True if valid</returns>
    public static bool ValidatePendingTicketCount(int currentPendingCount, int maxPendingTickets)
    {
        if (maxPendingTickets == 0)
            return true;

        return maxPendingTickets > currentPendingCount;
    }

    /// <summary>
    /// Validates if duplicate slip count is within limit
    /// </summary>
    /// <param name="currentDuplicateCount">Current duplicate count</param>
    /// <param name="maxDuplicates">Maximum duplicates allowed</param>
    /// <returns>True if valid</returns>
    public static bool ValidateDuplicateSlipLimit(int currentDuplicateCount, int maxDuplicates)
    {
        if (maxDuplicates == 0)
            return true;

        return currentDuplicateCount < maxDuplicates;
    }

    /// <summary>
    /// Validates if amount is positive
    /// </summary>
    /// <param name="amount">Amount to validate</param>
    /// <returns>True if positive</returns>
    public static bool ValidatePositiveAmount(decimal amount)
    {
        return amount > 0;
    }

    /// <summary>
    /// Validates if amount is non-negative
    /// </summary>
    /// <param name="amount">Amount to validate</param>
    /// <returns>True if non-negative</returns>
    public static bool ValidateNonNegativeAmount(decimal amount)
    {
        return amount >= 0;
    }

    /// <summary>
    /// Validates if odds are within valid range
    /// </summary>
    /// <param name="odds">Odds to validate</param>
    /// <param name="minOdds">Minimum odds</param>
    /// <param name="maxOdds">Maximum odds</param>
    /// <returns>True if valid</returns>
    public static bool ValidateOddsRange(decimal odds, decimal minOdds, decimal maxOdds)
    {
        return odds >= minOdds && odds <= maxOdds;
    }

    /// <summary>
    /// Validates if string is not null or empty
    /// </summary>
    /// <param name="value">String to validate</param>
    /// <returns>True if valid</returns>
    public static bool ValidateNotNullOrEmpty(string? value)
    {
        return !string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Validates if string is not null or whitespace
    /// </summary>
    /// <param name="value">String to validate</param>
    /// <returns>True if valid</returns>
    public static bool ValidateNotNullOrWhiteSpace(string? value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Validates if string length is within limits
    /// </summary>
    /// <param name="value">String to validate</param>
    /// <param name="minLength">Minimum length</param>
    /// <param name="maxLength">Maximum length</param>
    /// <returns>True if valid</returns>
    public static bool ValidateStringLength(string? value, int minLength, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        return value.Length >= minLength && value.Length <= maxLength;
    }

    /// <summary>
    /// Validates if date is in the future
    /// </summary>
    /// <param name="date">Date to validate</param>
    /// <returns>True if in the future</returns>
    public static bool ValidateFutureDate(DateTime date)
    {
        return date > DateTime.UtcNow;
    }

    /// <summary>
    /// Validates if date is in the past
    /// </summary>
    /// <param name="date">Date to validate</param>
    /// <returns>True if in the past</returns>
    public static bool ValidatePastDate(DateTime date)
    {
        return date < DateTime.UtcNow;
    }

    /// <summary>
    /// Validates if date is within a time window
    /// </summary>
    /// <param name="date">Date to validate</param>
    /// <param name="windowMinutes">Time window in minutes</param>
    /// <returns>True if within window</returns>
    public static bool ValidateDateWithinWindow(DateTime date, int windowMinutes)
    {
        var now = DateTime.UtcNow;
        var windowStart = now.AddMinutes(-windowMinutes);
        var windowEnd = now.AddMinutes(windowMinutes);

        return date >= windowStart && date <= windowEnd;
    }
}