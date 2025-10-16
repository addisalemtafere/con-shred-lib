using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Convex.Shared.Http.Extensions
{
    public static partial class StringExtensions
    {
        public static string Right(this string value, int length)
        {
            return value.Length < length ? value : value.Substring(value.Length - length, length);
        }

        public static bool IsNumeric(this string value)
        {
            return double.TryParse(value, out _);
        }

        public static bool IsPositiveNumber(this string value)
        {
            return double.TryParse(value, out double parsedValue) && parsedValue > 0;
        }

        public static int TryParseInt(this string value)
        {
            return int.TryParse(value, out int parsedValue) ? parsedValue : default;
        }

        public static bool IsInt(this string value)
        {
            return int.TryParse(value, out _);
        }
        public static bool IsPositiveInteger(this string value)
        {
            return int.TryParse(value, out int parsedValue) && parsedValue > 0;
        }
        public static long TryParseLong(this string value)
        {
            return long.TryParse(value, out long parsedValue) ? parsedValue : default;
        }
        public static decimal TryParseDecimal(this string value)
        {
            return decimal.TryParse(value, out decimal parsedValue) ? parsedValue : default;
        }

        public static bool IsDecimal(this string value)
        {
            return decimal.TryParse(value, out _);
        }

        public static decimal? TryParseNullableDecimal(this string value)
        {
            return decimal.TryParse(value, out decimal parsedValue) ? parsedValue : default;
        }

        public static DateTime TryParseDate(this string date, string format)
        {
            return DateTime.TryParseExact(date, format, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out DateTime parsedValue)
                ? parsedValue : default;
        }

        public static DateTime? TryParseNullableDate(this string date, string format)
        {
            return DateTime.TryParseExact(date, format, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out DateTime parsedValue)
                ? parsedValue : default;
        }

        public static string Base64Encode(this string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }

        public static bool IsZipCode(this string zipCode)
        {
            string usaZipPattern = @"^\d{5}$";
            Regex regex = new(usaZipPattern);

            return regex.IsMatch(zipCode);
        }

        public static T? ToEnum<T>(this string value, T? defaultValue) where T : struct, IComparable
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return Enum.TryParse(value, true, out T result) ? result : defaultValue;
        }

        public static bool IsEnum<T>(this string value, T? defaultValue) where T : struct, IComparable
        {
            return Enum.TryParse(value, true, out T _);
        }


        public static bool IsBool(this string value)
        {
            return bool.TryParse(value, out _);
        }

        public static bool IsDefault<T>(this T value) where T : struct
        {
            return value.Equals(default(T));
        }

        public static string ToTitleCase(this string source)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(source);
        }


    }
}
