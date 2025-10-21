using FluentValidation;

namespace Convex.Shared.Validation.Validators;

/// <summary>
/// Base validator class for Convex entities
/// </summary>
/// <typeparam name="T">The type to validate</typeparam>
public abstract class BaseValidator<T> : AbstractValidator<T>
{
    /// <summary>
    /// Validates email format
    /// </summary>
    /// <param name="email">The email to validate</param>
    /// <returns>True if valid email</returns>
    protected bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        return System.Text.RegularExpressions.Regex.IsMatch(email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    /// <summary>
    /// Validates phone number format
    /// </summary>
    /// <param name="phone">The phone number to validate</param>
    /// <returns>True if valid phone number</returns>
    protected bool IsValidPhoneNumber(string phone)
    {
        if (string.IsNullOrEmpty(phone))
            return false;

        return System.Text.RegularExpressions.Regex.IsMatch(phone,
            @"^\+?[1-9]\d{1,14}$");
    }

    /// <summary>
    /// Validates password strength
    /// </summary>
    /// <param name="password">The password to validate</param>
    /// <returns>True if strong password</returns>
    protected bool IsStrongPassword(string password)
    {
        if (string.IsNullOrEmpty(password) || password.Length < 8)
            return false;

        return System.Text.RegularExpressions.Regex.IsMatch(password,
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]");
    }

    /// <summary>
    /// Validates Ethiopian phone number format
    /// </summary>
    /// <param name="phone">The phone number to validate</param>
    /// <returns>True if valid Ethiopian phone number</returns>
    protected bool IsValidEthiopianPhoneNumber(string phone)
    {
        if (string.IsNullOrEmpty(phone))
            return false;

        // Ethiopian phone number patterns: +251XXXXXXXXX or 0XXXXXXXXX
        return System.Text.RegularExpressions.Regex.IsMatch(phone,
            @"^(\+251|0)(9[0-9]{8}|1[0-9]{8})$");
    }

    /// <summary>
    /// Validates betting stake amount
    /// </summary>
    /// <param name="stake">The stake amount to validate</param>
    /// <param name="minStake">Minimum stake amount</param>
    /// <param name="maxStake">Maximum stake amount</param>
    /// <returns>True if valid stake amount</returns>
    protected bool IsValidStakeAmount(decimal stake, decimal minStake = 1m, decimal maxStake = 100000m)
    {
        return stake >= minStake && stake <= maxStake;
    }

    /// <summary>
    /// Validates betting odds
    /// </summary>
    /// <param name="odds">The odds to validate</param>
    /// <param name="minOdds">Minimum odds</param>
    /// <param name="maxOdds">Maximum odds</param>
    /// <returns>True if valid odds</returns>
    protected bool IsValidOdds(decimal odds, decimal minOdds = 1.01m, decimal maxOdds = 1000m)
    {
        return odds >= minOdds && odds <= maxOdds;
    }

    /// <summary>
    /// Validates Ethiopian Birr amount
    /// </summary>
    /// <param name="amount">The amount to validate</param>
    /// <param name="minAmount">Minimum amount</param>
    /// <param name="maxAmount">Maximum amount</param>
    /// <returns>True if valid amount</returns>
    protected bool IsValidEthiopianBirrAmount(decimal amount, decimal minAmount = 0.01m, decimal maxAmount = 1000000m)
    {
        return amount >= minAmount && amount <= maxAmount;
    }

    /// <summary>
    /// Validates user ID format (GUID)
    /// </summary>
    /// <param name="userId">The user ID to validate</param>
    /// <returns>True if valid GUID</returns>
    protected bool IsValidUserId(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            return false;

        return Guid.TryParse(userId, out _);
    }

    /// <summary>
    /// Validates transaction reference format
    /// </summary>
    /// <param name="reference">The transaction reference to validate</param>
    /// <returns>True if valid transaction reference</returns>
    protected bool IsValidTransactionReference(string reference)
    {
        if (string.IsNullOrEmpty(reference))
            return false;

        // Transaction reference should be alphanumeric and 8-50 characters
        return System.Text.RegularExpressions.Regex.IsMatch(reference,
            @"^[A-Za-z0-9]{8,50}$");
    }

    /// <summary>
    /// Validates bet slip ID format
    /// </summary>
    /// <param name="slipId">The bet slip ID to validate</param>
    /// <returns>True if valid bet slip ID</returns>
    protected bool IsValidBetSlipId(string slipId)
    {
        if (string.IsNullOrEmpty(slipId))
            return false;

        // Bet slip ID should be alphanumeric and 10-20 characters
        return System.Text.RegularExpressions.Regex.IsMatch(slipId,
            @"^[A-Za-z0-9]{10,20}$");
    }

    /// <summary>
    /// Validates Ethiopian national ID format
    /// </summary>
    /// <param name="nationalId">The national ID to validate</param>
    /// <returns>True if valid Ethiopian national ID</returns>
    protected bool IsValidEthiopianNationalId(string nationalId)
    {
        if (string.IsNullOrEmpty(nationalId))
            return false;

        // Ethiopian national ID should be 10 digits
        return System.Text.RegularExpressions.Regex.IsMatch(nationalId,
            @"^\d{10}$");
    }

    /// <summary>
    /// Validates age for betting eligibility
    /// </summary>
    /// <param name="birthDate">The birth date to validate</param>
    /// <param name="minimumAge">Minimum age for betting (default 18)</param>
    /// <returns>True if eligible age</returns>
    protected bool IsEligibleBettingAge(DateTime birthDate, int minimumAge = 18)
    {
        var age = DateTime.UtcNow.Year - birthDate.Year;
        if (birthDate.Date > DateTime.UtcNow.AddYears(-age))
            age--;

        return age >= minimumAge;
    }

    /// <summary>
    /// Validates PIN format (4-6 digits)
    /// </summary>
    /// <param name="pin">The PIN to validate</param>
    /// <returns>True if valid PIN format</returns>
    protected bool IsValidPin(string pin)
    {
        if (string.IsNullOrEmpty(pin))
            return false;

        return System.Text.RegularExpressions.Regex.IsMatch(pin,
            @"^\d{4,6}$");
    }

    /// <summary>
    /// Validates account number format
    /// </summary>
    /// <param name="accountNumber">The account number to validate</param>
    /// <returns>True if valid account number</returns>
    protected bool IsValidAccountNumber(string accountNumber)
    {
        if (string.IsNullOrEmpty(accountNumber))
            return false;

        // Account number should be 10-20 digits
        return System.Text.RegularExpressions.Regex.IsMatch(accountNumber,
            @"^\d{10,20}$");
    }
}