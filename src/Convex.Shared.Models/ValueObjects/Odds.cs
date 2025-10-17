namespace Convex.Shared.Models.ValueObjects;

/// <summary>
/// Odds value object representing betting odds
/// </summary>
public readonly struct Odds
{
    /// <summary>
    /// The odds value
    /// </summary>
    public decimal Value { get; }

    /// <summary>
    /// Minimum allowed odds
    /// </summary>
    public const decimal MinOdds = 1.01m;

    /// <summary>
    /// Maximum allowed odds
    /// </summary>
    public const decimal MaxOdds = 1000.0m;

    /// <summary>
    /// Initializes a new instance of the Odds struct
    /// </summary>
    /// <param name="value">The odds value</param>
    /// <exception cref="ArgumentException">Thrown when odds value is invalid</exception>
    public Odds(decimal value)
    {
        if (value < MinOdds || value > MaxOdds)
            throw new ArgumentException($"Odds must be between {MinOdds} and {MaxOdds}", nameof(value));

        Value = Math.Round(value, 2); // Round to 2 decimal places
    }

    /// <summary>
    /// Creates odds from string representation
    /// </summary>
    /// <param name="oddsString">The odds as string</param>
    public static Odds Parse(string oddsString)
    {
        if (string.IsNullOrWhiteSpace(oddsString))
            throw new ArgumentException("Odds string cannot be null or empty", nameof(oddsString));

        if (!decimal.TryParse(oddsString, out var value))
            throw new ArgumentException($"Invalid odds format: {oddsString}", nameof(oddsString));

        return new Odds(value);
    }

    /// <summary>
    /// Tries to parse odds from string
    /// </summary>
    /// <param name="oddsString">The odds as string</param>
    /// <param name="odds">The parsed odds</param>
    /// <returns>True if parsing succeeded</returns>
    public static bool TryParse(string oddsString, out Odds odds)
    {
        odds = default;

        if (string.IsNullOrWhiteSpace(oddsString))
            return false;

        if (!decimal.TryParse(oddsString, out var value))
            return false;

        try
        {
            odds = new Odds(value);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Calculates potential win amount
    /// </summary>
    /// <param name="stake">The stake amount</param>
    /// <returns>Potential win amount</returns>
    public decimal CalculatePotentialWin(decimal stake)
    {
        return stake * Value;
    }

    /// <summary>
    /// Calculates potential profit (win - stake)
    /// </summary>
    /// <param name="stake">The stake amount</param>
    /// <returns>Potential profit</returns>
    public decimal CalculateProfit(decimal stake)
    {
        return CalculatePotentialWin(stake) - stake;
    }

    /// <summary>
    /// Converts to decimal odds
    /// </summary>
    public decimal ToDecimal() => Value;

    /// <summary>
    /// Converts to fractional odds (e.g., 3/1)
    /// </summary>
    public string ToFractional()
    {
        var numerator = Value - 1;
        var denominator = 1;

        // Simplify the fraction
        var gcd = GreatestCommonDivisor((int)(numerator * 100), (int)(denominator * 100));
        numerator = (numerator * 100) / gcd;
        denominator = (denominator * 100) / gcd;

        return $"{numerator}/{denominator}";
    }

    /// <summary>
    /// Converts to American odds
    /// </summary>
    public string ToAmerican()
    {
        if (Value >= 2.0m)
        {
            var american = (Value - 1) * 100;
            return $"+{american:F0}";
        }
        else
        {
            var american = -100 / (Value - 1);
            return $"{american:F0}";
        }
    }

    /// <summary>
    /// Calculates greatest common divisor
    /// </summary>
    private static int GreatestCommonDivisor(int a, int b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    /// <summary>
    /// Adds two odds (for accumulator bets)
    /// </summary>
    public static Odds operator *(Odds left, Odds right)
    {
        return new Odds(left.Value * right.Value);
    }

    /// <summary>
    /// Checks if odds are equal
    /// </summary>
    public static bool operator ==(Odds left, Odds right)
    {
        return Math.Abs(left.Value - right.Value) < 0.01m; // Allow small floating point differences
    }

    /// <summary>
    /// Checks if odds are not equal
    /// </summary>
    public static bool operator !=(Odds left, Odds right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Checks if left odds are greater than right odds
    /// </summary>
    public static bool operator >(Odds left, Odds right)
    {
        return left.Value > right.Value;
    }

    /// <summary>
    /// Checks if left odds are less than right odds
    /// </summary>
    public static bool operator <(Odds left, Odds right)
    {
        return left.Value < right.Value;
    }

    /// <summary>
    /// Checks if left odds are greater than or equal to right odds
    /// </summary>
    public static bool operator >=(Odds left, Odds right)
    {
        return left > right || left == right;
    }

    /// <summary>
    /// Checks if left odds are less than or equal to right odds
    /// </summary>
    public static bool operator <=(Odds left, Odds right)
    {
        return left < right || left == right;
    }

    /// <summary>
    /// Implicit conversion from decimal
    /// </summary>
    public static implicit operator Odds(decimal value)
    {
        return new Odds(value);
    }

    /// <summary>
    /// Implicit conversion to decimal
    /// </summary>
    public static implicit operator decimal(Odds odds)
    {
        return odds.Value;
    }

    /// <summary>
    /// Returns string representation
    /// </summary>
    public override string ToString()
    {
        return Value.ToString("F2");
    }

    /// <summary>
    /// Checks equality with another object
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is Odds odds && this == odds;
    }

    /// <summary>
    /// Gets hash code
    /// </summary>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}