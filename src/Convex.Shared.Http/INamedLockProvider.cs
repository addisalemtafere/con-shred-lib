namespace Convex.Shared.Http
{
    public interface INamedLockProvider
    {
        object GetLock(string key);
    }
}
