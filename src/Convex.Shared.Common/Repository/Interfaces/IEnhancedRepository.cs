using Convex.Shared.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Convex.Shared.Common.Repository.Interfaces
{
    public interface IEnhancedRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class
    {
        Task<PaginatedResponse<TEntity>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<int> CountAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken cancellationToken = default);
    }
}
