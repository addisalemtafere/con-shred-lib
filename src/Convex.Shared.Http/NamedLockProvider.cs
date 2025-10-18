using System.Collections.Concurrent;

namespace Convex.Shared.Http
{
    public class NamedLockProvider : INamedLockProvider
    {
        private readonly ConcurrentDictionary<string, object> _locks = new ConcurrentDictionary<string, object>();

        public object GetLock(string key)
        {
            return _locks.GetOrAdd(key, _ => new object());
        }
    }
}
