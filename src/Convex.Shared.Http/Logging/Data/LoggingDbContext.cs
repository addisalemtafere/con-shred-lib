using Convex.Shared.Http.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Convex.Shared.Http.Logging.Data
{
    public class LoggingDbContext : BaseDbContext
    {
        public LoggingDbContext(DbContextOptions<LoggingDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<Log> Log { get; set; }
        public DbSet<Method> Methods { get; set; }
        public DbSet<SensitiveField> SensitiveFields { get; set; }
    }
}
