using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Convex.Shared.Http.EntityFramework
{
    /// <summary>
    /// Base class to be used for any DbContexts.  Supports soft deletes, lazy loading, and dynamically applying mappings based in the derived DbContext's assembly.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class BaseDbContext : DbContext
    {
        protected BaseDbContext(DbContextOptions dbContextOptions, IConnectionStringProvider connectionStringProvider)
            : this(dbContextOptions, connectionStringProvider?.GetConnectionString())
        {
        }

        protected BaseDbContext(DbContextOptions dbContextOptions, string connectionString)
            : base(dbContextOptions)
        {
            ConnectionString = connectionString;

#if NET5_0_OR_GREATER
            ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
#endif
        }

        protected BaseDbContext(DbContextOptions dbContextOptions)
            : this(dbContextOptions, (string)null)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseNpgsql(ConnectionString); // Changed from UseSqlServer to UseNpgsql
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(GetType()));
            ConfigureSoftDeleteFilter(modelBuilder);
        }

        public override int SaveChanges()
        {
            OnBeforeSave();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSave();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSave()
        {
            foreach (var entry in ChangeTracker.Entries<ISoftDeleteEntity>().Where(x => x.State == EntityState.Deleted))
            {
                entry.State = EntityState.Unchanged;
                entry.Entity.IsDeleted = true;
            }
        }

        private static void ConfigureSoftDeleteFilter(ModelBuilder builder)
        {
            foreach (var softDeletableTypeBuilder in builder.Model.GetEntityTypes()
                .Where(x => typeof(ISoftDeleteEntity).IsAssignableFrom(x.ClrType)))
            {
                var parameter = Expression.Parameter(softDeletableTypeBuilder.ClrType, "p");
                var filter = Expression.Lambda(
                        Expression.Equal(
                            Expression.Property(parameter, "IsDeleted"),
                            Expression.Constant(false)), parameter);

#if NETSTANDARD2_0
            softDeletableTypeBuilder.QueryFilter = filter;
#else
                softDeletableTypeBuilder.SetQueryFilter(filter);
#endif
            }
        }

        public string ConnectionString { get; }
    }
}
