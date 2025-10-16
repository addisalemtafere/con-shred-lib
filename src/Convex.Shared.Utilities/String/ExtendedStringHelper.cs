using System.Text.RegularExpressions;
using System.Globalization;

namespace Convex.Shared.Utilities.String;

/// <summary>
/// Extended string utility functions (additional to Common extensions)
/// </summary>
public static class ExtendedStringHelper
{
    /// <summary>
    /// Generates a random string
    /// </summary>
    /// <param name="length">Length of string</param>
    /// <param name="includeNumbers">Include numbers</param>
    /// <param name="includeSpecialChars">Include special characters</param>
    /// <returns>Random string</returns>
    public static string GenerateRandomString(int length, bool includeNumbers = true, bool includeSpecialChars = false)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        const string numbers = "0123456789";
        const string specialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";

        var characterSet = chars;
        if (includeNumbers)
            characterSet += numbers;
        if (includeSpecialChars)
            characterSet += specialChars;

        var random = new Random();
        return new string(Enumerable.Repeat(characterSet, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    /// <summary>
    /// Generates a random alphanumeric string
    /// </summary>
    /// <param name="length">Length of string</param>
    /// <returns>Random alphanumeric string</returns>
    public static string GenerateRandomAlphanumeric(int length)
    {
        return GenerateRandomString(length, true, false);
    }

    /// <summary>
    /// Generates a random numeric string
    /// </summary>
    /// <param name="length">Length of string</param>
    /// <returns>Random numeric string</returns>
    public static string GenerateRandomNumeric(int length)
    {
        const string numbers = "0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(numbers, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    /// <summary>
    /// Masks sensitive string (e.g., phone numbers, emails)
    /// </summary>
    /// <param name="value">String to mask</param>
    /// <param name="visibleChars">Number of visible characters at start</param>
    /// <param name="visibleCharsEnd">Number of visible characters at end</param>
    /// <param name="maskChar">Character to use for masking</param>
    /// <returns>Masked string</returns>
    public static string MaskString(string? value, int visibleChars = 3, int visibleCharsEnd = 3, char maskChar = '*')
    {
        if (string.IsNullOrEmpty(value) || value.Length <= visibleChars + visibleCharsEnd)
            return value ?? string.Empty;

        var start = value.Substring(0, visibleChars);
        var end = value.Substring(value.Length - visibleCharsEnd);
        var middle = new string(maskChar, value.Length - visibleChars - visibleCharsEnd);

        return start + middle + end;
    }

    /// <summary>
    /// Masks phone number
    /// </summary>
    /// <param name="phoneNumber">Phone number to mask</param>
    /// <returns>Masked phone number</returns>
    public static string MaskPhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
            return string.Empty;

        return MaskString(phoneNumber, 3, 3);
    }

    /// <summary>
    /// Masks email address
    /// </summary>
    /// <param name="email">Email to mask</param>
    /// <returns>Masked email</returns>
    public static string MaskEmail(string? email)
    {
        if (string.IsNullOrEmpty(email))
            return string.Empty;

        var atIndex = email.IndexOf('@');
        if (atIndex <= 0)
            return email;

        var username = email.Substring(0, atIndex);
        var domain = email.Substring(atIndex);

        var maskedUsername = MaskString(username, 2, 1);
        return maskedUsername + domain;
    }

    /// <summary>
    /// Removes all non-alphanumeric characters
    /// </summary>
    /// <param name="value">String to clean</param>
    /// <returns>Cleaned string</returns>
    public static string RemoveNonAlphanumeric(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return Regex.Replace(value, @"[^a-zA-Z0-9]", "");
    }

    /// <summary>
    /// Removes all non-numeric characters
    /// </summary>
    /// <param name="value">String to clean</param>
    /// <returns>Cleaned string</returns>
    public static string RemoveNonNumeric(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return Regex.Replace(value, @"[^0-9]", "");
    }

    /// <summary>
    /// Capitalizes first letter of each word (uses Common's ToTitleCase)
    /// </summary>
    /// <param name="value">String to capitalize</param>
    /// <returns>Capitalized string</returns>
    public static string CapitalizeWords(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        // Implement ToTitleCase directly
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
    }

    /// <summary>
    /// Converts string to camelCase
    /// </summary>
    /// <param name="value">String to convert</param>
    /// <returns>Camel case string</returns>
    public static string ToCamelCase(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        var words = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (words.Length == 0)
            return string.Empty;

        var result = words[0].ToLower();
        for (int i = 1; i < words.Length; i++)
        {
            result += CapitalizeWords(words[i]);
        }

        return result;
    }

    /// <summary>
    /// Converts string to PascalCase
    /// </summary>
    /// <param name="value">String to convert</param>
    /// <returns>Pascal case string</returns>
    public static string ToPascalCase(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return CapitalizeWords(value).Replace(" ", "");
    }
}
