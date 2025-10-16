using System.Text.RegularExpressions;

namespace Convex.Shared.Utilities.Phone;

/// <summary>
/// Phone number utility functions
/// </summary>
public static class PhoneNumberHelper
{
    /// <summary>
    /// Default country code
    /// </summary>
    public const string DefaultCountryCode = "251";

    /// <summary>
    /// Default plus country code
    /// </summary>
    public const string DefaultPlusCountryCode = "+251";

    /// <summary>
    /// Phone number regex pattern
    /// </summary>
    public const string PhoneRegexPattern = @"^(\+251|251)?0?((\d){8,9})$";

    private static readonly Regex PhoneRegex = new(PhoneRegexPattern, RegexOptions.Compiled);

    /// <summary>
    /// Validates phone number format
    /// </summary>
    /// <param name="phoneNumber">Phone number to validate</param>
    /// <returns>True if valid</returns>
    public static bool IsValidPhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        return PhoneRegex.IsMatch(phoneNumber);
    }

    /// <summary>
    /// Formats phone number to international format
    /// </summary>
    /// <param name="phoneNumber">Phone number to format</param>
    /// <param name="countryCode">Country code (default: 251)</param>
    /// <returns>Formatted phone number</returns>
    public static string FormatToInternational(string? phoneNumber, string countryCode = DefaultCountryCode)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return string.Empty;

        var match = PhoneRegex.Match(phoneNumber);
        if (!match.Success)
            return phoneNumber;

        var phoneDigits = match.Groups[2].Value;
        return $"+{countryCode}{phoneDigits}";
    }

    /// <summary>
    /// Formats phone number to local format (with leading zero)
    /// </summary>
    /// <param name="phoneNumber">Phone number to format</param>
    /// <returns>Formatted phone number</returns>
    public static string FormatToLocal(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return string.Empty;

        var match = PhoneRegex.Match(phoneNumber);
        if (!match.Success)
            return phoneNumber;

        var phoneDigits = match.Groups[2].Value;
        return $"0{phoneDigits}";
    }

    /// <summary>
    /// Formats phone number without country code
    /// </summary>
    /// <param name="phoneNumber">Phone number to format</param>
    /// <returns>Formatted phone number</returns>
    public static string FormatWithoutCountryCode(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return string.Empty;

        var match = PhoneRegex.Match(phoneNumber);
        if (!match.Success)
            return phoneNumber;

        return match.Groups[2].Value;
    }

    /// <summary>
    /// Normalizes phone number to standard format
    /// </summary>
    /// <param name="phoneNumber">Phone number to normalize</param>
    /// <param name="countryCode">Country code (default: 251)</param>
    /// <returns>Normalized phone number</returns>
    public static string NormalizePhoneNumber(string? phoneNumber, string countryCode = DefaultCountryCode)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return string.Empty;

        // Remove all non-digit characters except +
        var cleaned = Regex.Replace(phoneNumber, @"[^\d+]", "");

        // Handle different formats
        if (cleaned.StartsWith($"+{countryCode}"))
            return cleaned;

        if (cleaned.StartsWith(countryCode))
            return $"+{cleaned}";

        if (cleaned.StartsWith("0"))
            return $"+{countryCode}{cleaned.Substring(1)}";

        // If it's just digits, assume it's local format
        if (cleaned.Length >= 8 && cleaned.Length <= 9)
            return $"+{countryCode}{cleaned}";

        return cleaned;
    }

    /// <summary>
    /// Gets country code from phone number
    /// </summary>
    /// <param name="phoneNumber">Phone number</param>
    /// <returns>Country code or empty string</returns>
    public static string GetCountryCode(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return string.Empty;

        if (phoneNumber.StartsWith("+"))
            return phoneNumber.Substring(1, 3);

        if (phoneNumber.Length >= 3)
            return phoneNumber.Substring(0, 3);

        return string.Empty;
    }

    /// <summary>
    /// Gets local number from phone number
    /// </summary>
    /// <param name="phoneNumber">Phone number</param>
    /// <returns>Local number or empty string</returns>
    public static string GetLocalNumber(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return string.Empty;

        var match = PhoneRegex.Match(phoneNumber);
        if (!match.Success)
            return string.Empty;

        return match.Groups[2].Value;
    }
}
