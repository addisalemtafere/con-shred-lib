namespace Convex.Shared.Models.ValueObjects;

/// <summary>
/// Money value object representing currency amounts
/// </summary>
public readonly struct Money
{
    /// <summary>
    /// Amount value
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// Currency code (e.g., "ETB", "USD")
    /// </summary>
    public string Currency { get; }

    /// <summary>
    /// Initializes a new instance of the Money struct
    /// </summary>
    /// <param name="amount">The amount</param>
    /// <param name="currency">The currency code</param>
    public Money(decimal amount, string currency = "ETB")
    {
        Amount = amount;
        Currency = currency;
    }

    /// <summary>
    /// Creates a zero amount
    /// </summary>
    public static Money Zero => new(0);

    /// <summary>
    /// Creates a zero amount with specific currency
    /// </summary>
    public static Money ZeroWithCurrency(string currency) => new(0, currency);

    /// <summary>
    /// Adds two money amounts
    /// </summary>
    public static Money operator +(Money left, Money right)
    {
        return new Money(left.Amount + right.Amount, left.Currency);
    }

    /// <summary>
    /// Subtracts two money amounts
    /// </summary>
    public static Money operator -(Money left, Money right)
    {
        return new Money(left.Amount - right.Amount, left.Currency);
    }

    /// <summary>
    /// Multiplies money by a decimal factor
    /// </summary>
    public static Money operator *(Money money, decimal factor)
    {
        return new Money(money.Amount * factor, money.Currency);
    }

    /// <summary>
    /// Multiplies money by a decimal factor
    /// </summary>
    public static Money operator *(decimal factor, Money money)
    {
        return money * factor;
    }

    /// <summary>
    /// Checks if money amounts are equal
    /// </summary>
    public static bool operator ==(Money left, Money right)
    {
        return left.Amount == right.Amount && left.Currency == right.Currency;
    }

    /// <summary>
    /// Checks if money amounts are not equal
    /// </summary>
    public static bool operator !=(Money left, Money right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Checks if left amount is greater than right amount
    /// </summary>
    public static bool operator >(Money left, Money right)
    {
        return left.Amount > right.Amount;
    }

    /// <summary>
    /// Checks if left amount is less than right amount
    /// </summary>
    public static bool operator <(Money left, Money right)
    {
        return left.Amount < right.Amount;
    }

    /// <summary>
    /// Checks if left amount is greater than or equal to right amount
    /// </summary>
    public static bool operator >=(Money left, Money right)
    {
        return left > right || left == right;
    }

    /// <summary>
    /// Checks if left amount is less than or equal to right amount
    /// </summary>
    public static bool operator <=(Money left, Money right)
    {
        return left < right || left == right;
    }

    /// <summary>
    /// Returns string representation of the money
    /// </summary>
    public override string ToString()
    {
        return $"{Amount:F2} {Currency}";
    }

    /// <summary>
    /// Checks equality with another object
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is Money money && this == money;
    }

    /// <summary>
    /// Gets hash code
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, Currency);
    }
}