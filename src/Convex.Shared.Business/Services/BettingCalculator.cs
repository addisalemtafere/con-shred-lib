using Convex.Shared.Business.Interfaces;
using Microsoft.Extensions.Logging;

namespace Convex.Shared.Business.Services;

/// <summary>
/// Betting calculator service
/// </summary>
public sealed class BettingCalculator : IBettingCalculator
{
    private readonly ILogger<BettingCalculator> _logger;
    private readonly BettingConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the BettingCalculator
    /// </summary>
    /// <param name="logger">Logger for observability</param>
    /// <param name="configuration">Betting configuration</param>
    /// <exception cref="ArgumentNullException">Thrown when logger or configuration is null</exception>
    public BettingCalculator(ILogger<BettingCalculator> logger, BettingConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(configuration);

        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Calculate daily sportsbook GGR (Gross Gaming Revenue)
    /// </summary>
    /// <param name="lostBetTotal">Total amount of lost bets</param>
    /// <param name="wonBetTotal">Total amount of won bets</param>
    /// <returns>GGR value</returns>
    public decimal CalculateDailySportsbookGgr(decimal lostBetTotal, decimal wonBetTotal)
    {
        var ggr = lostBetTotal - wonBetTotal;

        _logger.LogDebug("Calculated GGR: {Ggr} from lost: {LostBetTotal}, won: {WonBetTotal}",
            ggr, lostBetTotal, wonBetTotal);

        return ggr;
    }

    /// <summary>
    /// Calculate possible win amount
    /// </summary>
    /// <param name="stake">Bet stake</param>
    /// <param name="totalOdds">Total odds</param>
    /// <returns>Possible win amount</returns>
    public decimal CalculatePossibleWin(decimal stake, decimal totalOdds)
    {
        if (stake <= 0)
            throw new ArgumentException("Stake must be greater than zero", nameof(stake));

        if (totalOdds <= 0)
            throw new ArgumentException("Total odds must be greater than zero", nameof(totalOdds));

        var possibleWin = stake * totalOdds;

        _logger.LogDebug("Calculated possible win: {PossibleWin} from stake: {Stake}, odds: {TotalOdds}",
            possibleWin, stake, totalOdds);

        return possibleWin;
    }

    /// <summary>
    /// Calculate win value with tax
    /// </summary>
    /// <param name="stake">Bet stake</param>
    /// <param name="totalOdds">Total odds</param>
    /// <param name="matchCount">Number of matches</param>
    /// <returns>Win value after tax</returns>
    public decimal CalculateWinValue(decimal stake, decimal totalOdds, int matchCount)
    {
        if (matchCount <= 0)
            throw new ArgumentException("Match count must be greater than zero", nameof(matchCount));

        var possibleWin = CalculatePossibleWin(stake, totalOdds);
        var tax = CalculateTax(possibleWin);
        var winValue = possibleWin - tax;

        _logger.LogDebug("Calculated win value: {WinValue} from possible win: {PossibleWin}, tax: {Tax}",
            winValue, possibleWin, tax);

        return winValue;
    }

    /// <summary>
    /// Calculate tax on winnings
    /// </summary>
    /// <param name="winAmount">Win amount</param>
    /// <returns>Tax amount</returns>
    public decimal CalculateTax(decimal winAmount)
    {
        if (winAmount <= 0)
            return 0;

        if (winAmount <= _configuration.TaxableWinThreshold)
            return 0;

        var tax = winAmount * _configuration.WinTaxRate;

        _logger.LogDebug("Calculated tax: {Tax} from win amount: {WinAmount}", tax, winAmount);

        return tax;
    }

    /// <summary>
    /// Check if bet is eligible for cashout
    /// </summary>
    /// <param name="stake">Bet stake</param>
    /// <param name="totalOdds">Total odds</param>
    /// <param name="matchCount">Number of matches</param>
    /// <returns>True if eligible for cashout</returns>
    public bool IsEligibleForCashout(decimal stake, decimal totalOdds, int matchCount)
    {
        if (matchCount <= 0)
            throw new ArgumentException("Match count must be greater than zero", nameof(matchCount));

        var possibleWin = CalculatePossibleWin(stake, totalOdds);
        var isEligible = possibleWin >= _configuration.CashoutMinThreshold &&
                        totalOdds <= _configuration.CashoutMaxOdd &&
                        matchCount >= _configuration.CashoutMinMatches;

        _logger.LogDebug("Cashout eligibility: {IsEligible} for stake: {Stake}, odds: {TotalOdds}, matches: {MatchCount}",
            isEligible, stake, totalOdds, matchCount);

        return isEligible;
    }

    /// <summary>
    /// Calculate cashout value
    /// </summary>
    /// <param name="stake">Bet stake</param>
    /// <param name="totalOdds">Total odds</param>
    /// <param name="matchCount">Number of matches</param>
    /// <returns>Cashout value</returns>
    public decimal CalculateCashoutValue(decimal stake, decimal totalOdds, int matchCount)
    {
        if (matchCount <= 0)
            throw new ArgumentException("Match count must be greater than zero", nameof(matchCount));

        if (!IsEligibleForCashout(stake, totalOdds, matchCount))
            return 0;

        var possibleWin = CalculatePossibleWin(stake, totalOdds);
        var cashoutValue = possibleWin * (_configuration.CashoutWonPercent / 100);

        _logger.LogDebug("Calculated cashout value: {CashoutValue} from possible win: {PossibleWin}",
            cashoutValue, possibleWin);

        return cashoutValue;
    }
}

/// <summary>
/// Configuration for betting calculations
/// </summary>
public sealed class BettingConfiguration
{
    /// <summary>
    /// Taxable win threshold
    /// </summary>
    public decimal TaxableWinThreshold { get; set; } = 1000m;

    /// <summary>
    /// Win tax rate
    /// </summary>
    public decimal WinTaxRate { get; set; } = 0.15m;

    /// <summary>
    /// Cashout minimum threshold
    /// </summary>
    public decimal CashoutMinThreshold { get; set; } = 5000m;

    /// <summary>
    /// Cashout maximum odd
    /// </summary>
    public decimal CashoutMaxOdd { get; set; } = 1.8m;

    /// <summary>
    /// Cashout minimum matches
    /// </summary>
    public int CashoutMinMatches { get; set; } = 4;

    /// <summary>
    /// Cashout won percentage
    /// </summary>
    public decimal CashoutWonPercent { get; set; } = 70m;
}