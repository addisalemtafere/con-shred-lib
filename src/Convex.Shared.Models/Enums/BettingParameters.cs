namespace Convex.Shared.Models.Enums;

/// <summary>
/// Betting parameter enumeration
/// </summary>
public enum BettingParameter
{
    /// <summary>
    /// Under parameter
    /// </summary>
    Under,
    
    /// <summary>
    /// Over parameter
    /// </summary>
    Over,
    
    /// <summary>
    /// Yes parameter
    /// </summary>
    Yes,
    
    /// <summary>
    /// No parameter
    /// </summary>
    No,
    
    /// <summary>
    /// Away parameter
    /// </summary>
    Away,
    
    /// <summary>
    /// Home parameter
    /// </summary>
    Home,
    
    /// <summary>
    /// Draw parameter
    /// </summary>
    Draw,
    
    /// <summary>
    /// Home or Draw parameter
    /// </summary>
    HomeOrDraw,
    
    /// <summary>
    /// Away or Draw parameter
    /// </summary>
    AwayOrDraw,
    
    /// <summary>
    /// No Draw parameter
    /// </summary>
    NoDraw,
    
    /// <summary>
    /// Odd parameter
    /// </summary>
    Odd,
    
    /// <summary>
    /// Even parameter
    /// </summary>
    Even,
    
    /// <summary>
    /// First parameter
    /// </summary>
    First,
    
    /// <summary>
    /// Second parameter
    /// </summary>
    Second,
    
    /// <summary>
    /// Equal parameter
    /// </summary>
    Equal,
    
    /// <summary>
    /// Other parameter
    /// </summary>
    Other
}

/// <summary>
/// Betting group function enumeration
/// </summary>
public enum BettingGroupFunction
{
    /// <summary>
    /// Three way betting
    /// </summary>
    ThreeWay,
    
    /// <summary>
    /// Double chance betting
    /// </summary>
    DoubleChance,
    
    /// <summary>
    /// First half double chance
    /// </summary>
    FirstHalfDoubleChance,
    
    /// <summary>
    /// Second half double chance
    /// </summary>
    SecondHalfDoubleChance,
    
    /// <summary>
    /// First half three way
    /// </summary>
    FirstHalfThreeWay,
    
    /// <summary>
    /// Player sent off
    /// </summary>
    PlayerSentOff,
    
    /// <summary>
    /// Home team player sent off
    /// </summary>
    HomeTeamPlayerSentOff,
    
    /// <summary>
    /// Away team player sent off
    /// </summary>
    AwayTeamPlayerSentOff,
    
    /// <summary>
    /// Total corners under/over
    /// </summary>
    TotalCornersUO,
    
    /// <summary>
    /// Home team number of corners
    /// </summary>
    HomeTeamNumberOfCorners,
    
    /// <summary>
    /// Away team number of corners
    /// </summary>
    AwayTeamNumberOfCorners,
    
    /// <summary>
    /// Total corners
    /// </summary>
    TotalCorners,
    
    /// <summary>
    /// Total corners range
    /// </summary>
    TotalCornersRange,
    
    /// <summary>
    /// Total goals under/over
    /// </summary>
    TotalGoalsUO,
    
    /// <summary>
    /// Total goals aggregated
    /// </summary>
    TotalGoalsAggregated,
    
    /// <summary>
    /// Total goals exact
    /// </summary>
    TotalGoalsExact,
    
    /// <summary>
    /// Both teams to score
    /// </summary>
    BothTeamsToScore,
    
    /// <summary>
    /// Draw no bet
    /// </summary>
    DrawNoBet,
    
    /// <summary>
    /// Draw no bet first half
    /// </summary>
    DrawNoBetFirstHalf,
    
    /// <summary>
    /// Draw no bet second half
    /// </summary>
    DrawNoBetSecondHalf,
    
    /// <summary>
    /// Odd/Even goals
    /// </summary>
    OddEvenGoals,
    
    /// <summary>
    /// First team to score
    /// </summary>
    FirstTeamToScore,
    
    /// <summary>
    /// Total goals home team under/over
    /// </summary>
    TotalGoalsHomeTeamUO,
    
    /// <summary>
    /// Total goals home team exact
    /// </summary>
    TotalGoalsHomeTeamExact,
    
    /// <summary>
    /// Home to score in both halves
    /// </summary>
    HomeToScoreInBothHalves,
    
    /// <summary>
    /// Home to win both halves
    /// </summary>
    HomeToWinBothHalves,
    
    /// <summary>
    /// Home to win either half
    /// </summary>
    HomeToWinEitherHalf,
    
    /// <summary>
    /// Total goals away team under/over
    /// </summary>
    TotalGoalsAwayTeamUO,
    
    /// <summary>
    /// Total goals away team exact
    /// </summary>
    TotalGoalsAwayTeamExact,
    
    /// <summary>
    /// Away team to score in both halves
    /// </summary>
    AwayTeamToScoreInBothHalves,
    
    /// <summary>
    /// Away to win both halves
    /// </summary>
    AwayToWinBothHalves,
    
    /// <summary>
    /// Away to win either half
    /// </summary>
    AwayToWinEitherHalf,
    
    /// <summary>
    /// Handicap parameter
    /// </summary>
    Handicap,
    
    /// <summary>
    /// Correct score
    /// </summary>
    CorrectScore,
    
    /// <summary>
    /// First half result
    /// </summary>
    FirstHalfResult,
    
    /// <summary>
    /// First half total goals under/over
    /// </summary>
    FirstHalfTotalGoalsUO,
    
    /// <summary>
    /// First half both teams to score
    /// </summary>
    FirstHalfBothTeamsToScore,
    
    /// <summary>
    /// First half correct score
    /// </summary>
    FirstHalfCorrectScore,
    
    /// <summary>
    /// Second half result
    /// </summary>
    SecondHalfResult,
    
    /// <summary>
    /// Second half total goals under/over
    /// </summary>
    SecondHalfTotalGoalsUO,
    
    /// <summary>
    /// Second half total goals exact
    /// </summary>
    SecondHalfTotalGoalsExact,
    
    /// <summary>
    /// Second half both teams to score
    /// </summary>
    SecondHalfBothTeamsToScore,
    
    /// <summary>
    /// Highest scoring half
    /// </summary>
    HighestScoringHalf,
    
    /// <summary>
    /// Both halves under 1.5
    /// </summary>
    BothHalvesUnder15,
    
    /// <summary>
    /// Both halves over 1.5
    /// </summary>
    BothHalvesOver15,
    
    /// <summary>
    /// Halftime/Fulltime
    /// </summary>
    HalftimeFulltime,
    
    /// <summary>
    /// Match result plus total goals under/over
    /// </summary>
    MatchResultPlusTotalGoalsUO,
    
    /// <summary>
    /// Match result plus both teams to score
    /// </summary>
    MatchResultPlusBothTeamsToScore,
    
    /// <summary>
    /// Both teams to score plus total goals under/over
    /// </summary>
    BothTeamsToScorePlusTotalGoalsUO,
    
    /// <summary>
    /// First 10 minutes result
    /// </summary>
    First10MinutesResult
}
