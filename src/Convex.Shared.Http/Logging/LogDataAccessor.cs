namespace Convex.Shared.Http.Logging
{
    public class LogDataAccessor
    {
        public Func<IServiceProvider, int?> GetParticipantId { get; set; }
        public Func<IServiceProvider, Guid?> GetApplicationKey { get; set; }
    }
}
