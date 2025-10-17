namespace Convex.Shared.Models.Constants;

/// <summary>
/// Permission-related constants
/// </summary>
public static class PermissionConstants
{
    #region Agent Permissions

    /// <summary>
    /// Agent can create sales
    /// </summary>
    public const int AgentCreateSales = 1;

    /// <summary>
    /// Agent can update sales
    /// </summary>
    public const int AgentUpdateSales = 2;

    /// <summary>
    /// Agent can view sales
    /// </summary>
    public const int AgentViewSales = 3;

    /// <summary>
    /// Agent can create branch
    /// </summary>
    public const int AgentCreateBranch = 11;

    /// <summary>
    /// Agent can update branch
    /// </summary>
    public const int AgentUpdateBranch = 12;

    /// <summary>
    /// Agent can view branches
    /// </summary>
    public const int AgentViewBranches = 13;

    /// <summary>
    /// Agent can cancel deposit
    /// </summary>
    public const int AgentCancelDeposit = 21;

    /// <summary>
    /// Agent can deposit
    /// </summary>
    public const int AgentCanDeposit = 22;

    /// <summary>
    /// Agent access dashboard
    /// </summary>
    public const int AgentAccDashboard = 31;

    /// <summary>
    /// Agent manage ticket
    /// </summary>
    public const int AgentManageTicket = 41;

    /// <summary>
    /// Agent manage jackpot
    /// </summary>
    public const int AgentManageJackpot = 42;

    /// <summary>
    /// Agent list offline bets
    /// </summary>
    public const int AgentListOfflineBets = 51;

    /// <summary>
    /// Agent payout disabled
    /// </summary>
    public const int AgentPayoutDisabled = 61;

    /// <summary>
    /// Agent withdraw disabled
    /// </summary>
    public const int AgentWithdrawDisabled = 62;

    /// <summary>
    /// Agent deposit disabled
    /// </summary>
    public const int AgentDepositDisabled = 63;

    /// <summary>
    /// Agent view cash report
    /// </summary>
    public const int AgentViewCashReport = 71;

    #endregion Agent Permissions

    #region Client Permissions

    /// <summary>
    /// Client system admin
    /// </summary>
    public const int ClientSystemAdmin = 1000;

    /// <summary>
    /// Client can create agent
    /// </summary>
    public const int ClientCreateAgent = 1;

    /// <summary>
    /// Client can update agent
    /// </summary>
    public const int ClientUpdateAgent = 2;

    /// <summary>
    /// Client can list agents
    /// </summary>
    public const int ClientListAgents = 3;

    /// <summary>
    /// Client can create client
    /// </summary>
    public const int ClientCreateClient = 4;

    /// <summary>
    /// Client can update client
    /// </summary>
    public const int ClientUpdateClient = 5;

    /// <summary>
    /// Client can view clients
    /// </summary>
    public const int ClientViewClients = 6;

    /// <summary>
    /// Client can create branch
    /// </summary>
    public const int ClientCreateBranch = 7;

    /// <summary>
    /// Client can update branch
    /// </summary>
    public const int ClientUpdateBranch = 8;

    /// <summary>
    /// Client can view branches
    /// </summary>
    public const int ClientViewBranches = 9;

    /// <summary>
    /// Client can view agent cash report
    /// </summary>
    public const int ClientViewAgentCashReport = 10;

    /// <summary>
    /// Client can view branch cash report
    /// </summary>
    public const int ClientViewBranchCashReport = 11;

    /// <summary>
    /// Client can view sales cash report
    /// </summary>
    public const int ClientViewSalesCashReport = 12;

    /// <summary>
    /// Client can change admin password
    /// </summary>
    public const int ClientChangeAdminPassword = 13;

    #endregion Client Permissions

    #region Sales Permissions

    /// <summary>
    /// Sales can deposit
    /// </summary>
    public const int SalesCanDeposit = 1;

    /// <summary>
    /// Sales can cancel deposit
    /// </summary>
    public const int SalesCanCancelDeposit = 2;

    /// <summary>
    /// Sales can withdraw
    /// </summary>
    public const int SalesCanWithdraw = 3;

    /// <summary>
    /// Sales can sell ticket
    /// </summary>
    public const int SalesCanSellTicket = 4;

    /// <summary>
    /// Sales can cancel ticket
    /// </summary>
    public const int SalesCanCancelTicket = 5;

    /// <summary>
    /// Sales can generate advanced report
    /// </summary>
    public const int SalesCanGenerateAdvReport = 6;

    /// <summary>
    /// Sales can sell jackpot
    /// </summary>
    public const int SalesCanSellJackpot = 7;

    /// <summary>
    /// Sales can pay jackpot
    /// </summary>
    public const int SalesCanPayJackpot = 8;

    /// <summary>
    /// Sales can cancel jackpot
    /// </summary>
    public const int SalesCanCancelJackpot = 9;

    /// <summary>
    /// Sales can date filter dashboard by date
    /// </summary>
    public const int SalesCanDateFilterDashboardDate = 10;

    /// <summary>
    /// Sales can request withdraw
    /// </summary>
    public const int SalesCanRequestWithdraw = 11;

    #endregion Sales Permissions

    #region Branch Permissions

    /// <summary>
    /// Branch payout disabled
    /// </summary>
    public const int BranchPayoutDisabled = 10;

    /// <summary>
    /// Branch withdraw disabled
    /// </summary>
    public const int BranchWithdrawDisabled = 11;

    /// <summary>
    /// Branch deposit disabled
    /// </summary>
    public const int BranchDepositDisabled = 12;

    #endregion Branch Permissions

    #region Casino Permissions

    /// <summary>
    /// Client view casino dashboard
    /// </summary>
    public const int ClientViewCasinoDashboard = 400;

    /// <summary>
    /// Client view casino dashboard full access
    /// </summary>
    public const int ClientViewCasinoDashboardFullAccess = 401;

    /// <summary>
    /// Client view casino dashboard partial access
    /// </summary>
    public const int ClientViewCasinoDashboardPartialAccess = 402;

    /// <summary>
    /// Client view casino data
    /// </summary>
    public const int ClientViewCasinoData = 410;

    /// <summary>
    /// Client manage casino game
    /// </summary>
    public const int ClientManageCasinoGame = 411;

    /// <summary>
    /// Client manage casino main category
    /// </summary>
    public const int ClientManageCasinoMainCategory = 412;

    /// <summary>
    /// Client manage casino provider
    /// </summary>
    public const int ClientManageCasinoProvider = 413;

    /// <summary>
    /// Client manage casino tag
    /// </summary>
    public const int ClientManageCasinoTag = 414;

    /// <summary>
    /// Client can view freebet
    /// </summary>
    public const int ClientCanViewFreebet = 415;

    /// <summary>
    /// Client can create freebet
    /// </summary>
    public const int ClientCanCreateFreebet = 416;

    #endregion Casino Permissions

    #region Bonus Permissions

    /// <summary>
    /// Client manage user bonus
    /// </summary>
    public const int ClientManageUserBonus = 420;

    /// <summary>
    /// Client manage bonus setting
    /// </summary>
    public const int ClientManageBonusSetting = 421;

    /// <summary>
    /// Client manage bonus description
    /// </summary>
    public const int ClientManageBonusDescription = 422;

    /// <summary>
    /// Client manage bonus wagering policy
    /// </summary>
    public const int ClientManageBonusWageringPolicy = 423;

    /// <summary>
    /// Client manage bonus description locale
    /// </summary>
    public const int ClientManageBonusDescriptionLocale = 424;

    /// <summary>
    /// Client can award free bet
    /// </summary>
    public const int ClientCanAwardFreeBet = 425;

    #endregion Bonus Permissions

    #region Referral Permissions

    /// <summary>
    /// Client can view referral info
    /// </summary>
    public const int ClientCanViewReferralInfo = 500;

    /// <summary>
    /// Client can manage referral payments
    /// </summary>
    public const int ClientCanManageReferralPayments = 501;

    /// <summary>
    /// Client can manage referral config
    /// </summary>
    public const int ClientCanManageReferralConfig = 502;

    #endregion Referral Permissions

    #region Configuration Permissions

    /// <summary>
    /// Client can view main configuration
    /// </summary>
    public const int ClientCanViewMainConfiguration = 600;

    /// <summary>
    /// Client can update main configuration
    /// </summary>
    public const int ClientCanUpdateMainConfiguration = 601;

    /// <summary>
    /// Client can configure operating hours
    /// </summary>
    public const int ClientCanConfigureOperatingHours = 602;

    /// <summary>
    /// Client can view payment configuration
    /// </summary>
    public const int ClientCanViewPaymentConfiguration = 700;

    /// <summary>
    /// Client can update payment configuration
    /// </summary>
    public const int ClientCanUpdatePaymentConfiguration = 701;

    #endregion Configuration Permissions

    #region Bet Recommendation Permissions

    /// <summary>
    /// Client can view bet recommendation config
    /// </summary>
    public const int ClientCanViewBetRecommendationConfig = 800;

    /// <summary>
    /// Client can update bet recommendation config
    /// </summary>
    public const int ClientCanUpdateBetRecommendationConfig = 801;

    #endregion Bet Recommendation Permissions

    #region Affiliate Permissions

    /// <summary>
    /// Client can view affiliate postback requests
    /// </summary>
    public const int ClientCanViewAffiliatePostbackRequests = 1100;

    /// <summary>
    /// Client can trigger affiliate postback tasks
    /// </summary>
    public const int ClientCanTriggerAffiliatePostbackTasks = 1101;

    /// <summary>
    /// Client can configure cashout
    /// </summary>
    public const int ClientCanConfigureCashout = 1102;

    /// <summary>
    /// Client can configure affiliate provider
    /// </summary>
    public const int ClientCanConfigureAffiliateProvider = 1103;

    /// <summary>
    /// Client can configure loyalty bonus
    /// </summary>
    public const int ClientCanConfigureLoyaltyBonus = 1104;

    #endregion Affiliate Permissions

    #region Cashback Permissions

    /// <summary>
    /// Client update branch cashback rule
    /// </summary>
    public const int ClientUpdateBranchCashbackRule = 3200;

    /// <summary>
    /// Client update agent cashback rule
    /// </summary>
    public const int ClientUpdateAgentCashbackRule = 3201;

    #endregion Cashback Permissions

    #region Tournament Permissions

    /// <summary>
    /// Client can view tournament
    /// </summary>
    public const int ClientCanViewTournament = 2001;

    /// <summary>
    /// Client can manage tournament
    /// </summary>
    public const int ClientCanManageTournament = 2002;

    #endregion Tournament Permissions

    #region Streak Permissions

    /// <summary>
    /// Client can view streak setting
    /// </summary>
    public const int ClientCanViewStreakSetting = 2101;

    /// <summary>
    /// Client can update streak setting
    /// </summary>
    public const int ClientCanUpdateStreakSetting = 2102;

    #endregion Streak Permissions

    #region Raffle Permissions

    /// <summary>
    /// Client can view raffle
    /// </summary>
    public const int ClientCanViewRaffle = 2201;

    /// <summary>
    /// Client can manage raffle configuration
    /// </summary>
    public const int ClientCanManageRaffleConfiguration = 2202;

    /// <summary>
    /// Client can manage raffle award
    /// </summary>
    public const int ClientCanManageRaffleAward = 2203;

    #endregion Raffle Permissions

    #region Casino Cashback Permissions

    /// <summary>
    /// Client manage casino cashback config
    /// </summary>
    public const int ClientManageCasinoCashbackConfig = 3400;

    #endregion Casino Cashback Permissions

    #region Wheel Spinning Permissions

    /// <summary>
    /// Client can manage wheel spinning
    /// </summary>
    public const int ClientCanManageWheelSpinning = 3500;

    /// <summary>
    /// Client can view wheel spinning
    /// </summary>
    public const int ClientCanViewWheelSpinning = 3501;

    #endregion Wheel Spinning Permissions
}