using System.Text.RegularExpressions;

namespace Convex.Shared.Models.ValueObjects;

/// <summary>
/// Phone number value object with validation
/// </summary>
public readonly struct PhoneNumber
{
    private static readonly Regex PhoneRegex = new(@"^(\+251|251)?0?((\d){8,9})$", RegexOptions.Compiled);

    /// <summary>
    /// The phone number value
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the PhoneNumber struct
    /// </summary>
    /// <param name="phoneNumber">The phone number</param>
    /// <exception cref="ArgumentException">Thrown when phone number is invalid</exception>
    public PhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be null or empty", nameof(phoneNumber));

        // Normalize the phone number
        var normalized = NormalizePhoneNumber(phoneNumber);

        if (!IsValidPhoneNumber(normalized))
            throw new ArgumentException($"Invalid phone number format: {phoneNumber}", nameof(phoneNumber));

        Value = normalized;
    }

    /// <summary>
    /// Normalizes phone number to standard format
    /// </summary>
    private static string NormalizePhoneNumber(string phoneNumber)
    {
        // Remove all non-digit characters except +
        var cleaned = Regex.Replace(phoneNumber, @"[^\d+]", "");

        // Handle different formats
        if (cleaned.StartsWith("+251"))
            return cleaned;

        if (cleaned.StartsWith("251"))
            return "+" + cleaned;

        if (cleaned.StartsWith("0"))
            return "+251" + cleaned.Substring(1);

        // If it's just digits, assume it's Ethiopian format
        if (cleaned.Length >= 8 && cleaned.Length <= 9)
            return "+251" + cleaned;

        return cleaned;
    }

    /// <summary>
    /// Validates phone number format
    /// </summary>
    private static bool IsValidPhoneNumber(string phoneNumber)
    {
        return PhoneRegex.IsMatch(phoneNumber);
    }

    /// <summary>
    /// Gets the local format (without country code)
    /// </summary>
    public string LocalFormat
    {
        get
        {
            if (Value.StartsWith("+251"))
                return "0" + Value.Substring(4);
            return Value;
        }
    }

    /// <summary>
    /// Gets the international format
    /// </summary>
    public string InternationalFormat => Value;

    /// <summary>
    /// Gets the country code
    /// </summary>
    public string CountryCode => "+251";

    /// <summary>
    /// Checks if phone numbers are equal
    /// </summary>
    public static bool operator ==(PhoneNumber left, PhoneNumber right)
    {
        return left.Value == right.Value;
    }

    /// <summary>
    /// Checks if phone numbers are not equal
    /// </summary>
    public static bool operator !=(PhoneNumber left, PhoneNumber right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Implicit conversion from string
    /// </summary>
    public static implicit operator PhoneNumber(string phoneNumber)
    {
        return new PhoneNumber(phoneNumber);
    }

    /// <summary>
    /// Implicit conversion to string
    /// </summary>
    public static implicit operator string(PhoneNumber phoneNumber)
    {
        return phoneNumber.Value;
    }

    /// <summary>
    /// Returns string representation
    /// </summary>
    public override string ToString()
    {
        return Value;
    }

    /// <summary>
    /// Checks equality with another object
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is PhoneNumber phoneNumber && this == phoneNumber;
    }

    /// <summary>
    /// Gets hash code
    /// </summary>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}