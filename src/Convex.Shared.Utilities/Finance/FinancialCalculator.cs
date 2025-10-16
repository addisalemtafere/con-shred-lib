namespace Convex.Shared.Utilities.Finance;

/// <summary>
/// Financial calculation utilities
/// </summary>
public static class FinancialCalculator
{
    /// <summary>
    /// Calculates TOT (Turnover Tax) - 10% of stake
    /// </summary>
    /// <param name="stake">Stake amount</param>
    /// <returns>TOT amount</returns>
    public static decimal CalculateTot(decimal stake)
    {
        return stake * 0.10m;
    }

    /// <summary>
    /// Calculates winning tax
    /// </summary>
    /// <param name="maxWin">Maximum win amount</param>
    /// <param name="taxableWinThreshold">Taxable win threshold</param>
    /// <param name="winTaxRate">Win tax rate</param>
    /// <returns>Winning tax amount</returns>
    public static decimal CalculateWinTax(decimal maxWin, decimal taxableWinThreshold, decimal winTaxRate)
    {
        if (maxWin <= taxableWinThreshold)
            return 0;

        return maxWin * winTaxRate;
    }

    /// <summary>
    /// Calculates maximum win amount
    /// </summary>
    /// <param name="stake">Stake amount</param>
    /// <param name="oddTotal">Total odds</param>
    /// <param name="maxWinLimit">Maximum win limit</param>
    /// <returns>Maximum win amount</returns>
    public static decimal CalculateMaxWin(decimal stake, decimal oddTotal, decimal maxWinLimit)
    {
        var tot = CalculateTot(stake);
        var netStake = stake;
        var tentedStake = maxWinLimit / oddTotal;

        var possibleWin = netStake * oddTotal;

        if (possibleWin > maxWinLimit)
        {
            var extraStake = netStake - tentedStake;
            possibleWin = maxWinLimit + extraStake;
        }

        return possibleWin - tot;
    }

    /// <summary>
    /// Calculates won value (net win after taxes)
    /// </summary>
    /// <param name="stake">Stake amount</param>
    /// <param name="oddTotal">Total odds</param>
    /// <param name="maxWinLimit">Maximum win limit</param>
    /// <param name="taxableWinThreshold">Taxable win threshold</param>
    /// <param name="winTaxRate">Win tax rate</param>
    /// <returns>Net won value</returns>
    public static decimal CalculateWonValue(decimal stake, decimal oddTotal, decimal maxWinLimit, decimal taxableWinThreshold, decimal winTaxRate)
    {
        var maxWin = CalculateMaxWin(stake, oddTotal, maxWinLimit);
        var winTax = CalculateWinTax(maxWin, taxableWinThreshold, winTaxRate);
        
        return maxWin - winTax;
    }

    /// <summary>
    /// Calculates transaction fee (percentage)
    /// </summary>
    /// <param name="amount">Transaction amount</param>
    /// <param name="feeRate">Fee rate (e.g., 0.02 for 2%)</param>
    /// <returns>Transaction fee</returns>
    public static decimal CalculateTransactionFee(decimal amount, decimal feeRate)
    {
        return amount * feeRate;
    }

    /// <summary>
    /// Calculates transaction fee (fixed amount)
    /// </summary>
    /// <param name="fixedFee">Fixed fee amount</param>
    /// <returns>Transaction fee</returns>
    public static decimal CalculateFixedTransactionFee(decimal fixedFee)
    {
        return fixedFee;
    }

    /// <summary>
    /// Calculates net amount after fees
    /// </summary>
    /// <param name="amount">Original amount</param>
    /// <param name="fee">Fee amount</param>
    /// <returns>Net amount</returns>
    public static decimal CalculateNetAmount(decimal amount, decimal fee)
    {
        return amount - fee;
    }

    /// <summary>
    /// Calculates potential win amount
    /// </summary>
    /// <param name="stake">Stake amount</param>
    /// <param name="odds">Odds</param>
    /// <returns>Potential win amount</returns>
    public static decimal CalculatePotentialWin(decimal stake, decimal odds)
    {
        return stake * odds;
    }

    /// <summary>
    /// Calculates potential profit (win - stake)
    /// </summary>
    /// <param name="stake">Stake amount</param>
    /// <param name="odds">Odds</param>
    /// <returns>Potential profit</returns>
    public static decimal CalculatePotentialProfit(decimal stake, decimal odds)
    {
        return CalculatePotentialWin(stake, odds) - stake;
    }

    /// <summary>
    /// Calculates total odds for accumulator bet
    /// </summary>
    /// <param name="odds">List of odds</param>
    /// <returns>Total odds</returns>
    public static decimal CalculateTotalOdds(IEnumerable<decimal> odds)
    {
        return odds.Aggregate(1m, (total, odd) => total * odd);
    }

    /// <summary>
    /// Rounds amount to 2 decimal places
    /// </summary>
    /// <param name="amount">Amount to round</param>
    /// <returns>Rounded amount</returns>
    public static decimal RoundAmount(decimal amount)
    {
        return Math.Round(amount, 2, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// Validates if amount is within limits
    /// </summary>
    /// <param name="amount">Amount to validate</param>
    /// <param name="minAmount">Minimum amount</param>
    /// <param name="maxAmount">Maximum amount</param>
    /// <returns>True if within limits</returns>
    public static bool IsAmountWithinLimits(decimal amount, decimal minAmount, decimal maxAmount)
    {
        return amount >= minAmount && amount <= maxAmount;
    }
}
