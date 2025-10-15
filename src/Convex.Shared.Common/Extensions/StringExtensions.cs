using System.Text.RegularExpressions;

namespace Convex.Shared.Common.Extensions;

/// <summary>
/// String extension methods
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Checks if the string is null or empty
    /// </summary>
    /// <param name="value">The string to check</param>
    /// <returns>True if null or empty</returns>
    public static bool IsNullOrEmpty(this string? value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Checks if the string is null, empty, or whitespace
    /// </summary>
    /// <param name="value">The string to check</param>
    /// <returns>True if null, empty, or whitespace</returns>
    public static bool IsNullOrWhiteSpace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Converts string to title case
    /// </summary>
    /// <param name="value">The string to convert</param>
    /// <returns>Title case string</returns>
    public static string ToTitleCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
    }

    /// <summary>
    /// Truncates string to specified length
    /// </summary>
    /// <param name="value">The string to truncate</param>
    /// <param name="maxLength">Maximum length</param>
    /// <param name="suffix">Suffix to add if truncated</param>
    /// <returns>Truncated string</returns>
    public static string Truncate(this string value, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            return value;

        return value.Substring(0, maxLength - suffix.Length) + suffix;
    }

    /// <summary>
    /// Validates email format
    /// </summary>
    /// <param name="value">The email to validate</param>
    /// <returns>True if valid email</returns>
    public static bool IsValidEmail(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        return emailRegex.IsMatch(value);
    }

    /// <summary>
    /// Converts string to slug format
    /// </summary>
    /// <param name="value">The string to convert</param>
    /// <returns>Slug formatted string</returns>
    public static string ToSlug(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return value.ToLowerInvariant()
            .Replace(" ", "-")
            .Replace("_", "-")
            .Trim('-');
    }
}
