namespace Convex.Shared.Models.Enums;

/// <summary>
/// User type enumeration
/// </summary>
public enum UserType
{
    /// <summary>
    /// Better user
    /// </summary>
    Better = 1,

    /// <summary>
    /// Agent user
    /// </summary>
    Agent = 2,

    /// <summary>
    /// Client user
    /// </summary>
    Client = 3,

    /// <summary>
    /// Sales user
    /// </summary>
    Sales = 4,

    /// <summary>
    /// Branch user
    /// </summary>
    Branch = 5,

    /// <summary>
    /// Online agent user
    /// </summary>
    OnlineAgent = 6
}

/// <summary>
/// Member type enumeration
/// </summary>
public enum MemberType
{
    /// <summary>
    /// Regular member
    /// </summary>
    Regular = 1,

    /// <summary>
    /// Agent member
    /// </summary>
    Agent = 2
}

/// <summary>
/// Deposit plan enumeration
/// </summary>
public enum DepositPlan
{
    /// <summary>
    /// Pre-paid plan
    /// </summary>
    Prepaid = 1,

    /// <summary>
    /// Post-paid plan
    /// </summary>
    Postpaid = 2
}

/// <summary>
/// Compatibility enumeration
/// </summary>
public enum Compatibility
{
    /// <summary>
    /// Legacy compatibility
    /// </summary>
    Legacy = 1,

    /// <summary>
    /// Betradar compatibility
    /// </summary>
    Betradar = 2
}