namespace Convex.Shared.Business.Interfaces;

/// <summary>
/// Interface for betting calculations following SOLID principles.
/// Interface Segregation: Only defines betting calculation contract.
/// Dependency Inversion: Depends on abstractions, not concretions.
/// </summary>
public interface IBettingCalculator
{
    /// <summary>
    /// Calculate daily sportsbook GGR (Gross Gaming Revenue)
    /// </summary>
    /// <param name="lostBetTotal">Total amount of lost bets</param>
    /// <param name="wonBetTotal">Total amount of won bets</param>
    /// <returns>GGR value</returns>
    decimal CalculateDailySportsbookGgr(decimal lostBetTotal, decimal wonBetTotal);

    /// <summary>
    /// Calculate possible win amount
    /// </summary>
    /// <param name="stake">Bet stake</param>
    /// <param name="totalOdds">Total odds</param>
    /// <returns>Possible win amount</returns>
    decimal CalculatePossibleWin(decimal stake, decimal totalOdds);

    /// <summary>
    /// Calculate win value with tax
    /// </summary>
    /// <param name="stake">Bet stake</param>
    /// <param name="totalOdds">Total odds</param>
    /// <param name="matchCount">Number of matches</param>
    /// <returns>Win value after tax</returns>
    decimal CalculateWinValue(decimal stake, decimal totalOdds, int matchCount);

    /// <summary>
    /// Calculate tax on winnings
    /// </summary>
    /// <param name="winAmount">Win amount</param>
    /// <returns>Tax amount</returns>
    decimal CalculateTax(decimal winAmount);

    /// <summary>
    /// Check if bet is eligible for cashout
    /// </summary>
    /// <param name="stake">Bet stake</param>
    /// <param name="totalOdds">Total odds</param>
    /// <param name="matchCount">Number of matches</param>
    /// <returns>True if eligible for cashout</returns>
    bool IsEligibleForCashout(decimal stake, decimal totalOdds, int matchCount);

    /// <summary>
    /// Calculate cashout value
    /// </summary>
    /// <param name="stake">Bet stake</param>
    /// <param name="totalOdds">Total odds</param>
    /// <param name="matchCount">Number of matches</param>
    /// <returns>Cashout value</returns>
    decimal CalculateCashoutValue(decimal stake, decimal totalOdds, int matchCount);
}
