using Convex.Shared.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convex.Shared.Common.Application
{
    public interface IBaseService<TEntity, TKey>
    {
        Task<Result<TEntity>> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
        Task<Result<PaginatedResponse<TEntity>>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
        Task<Result<TEntity>> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<Result<TEntity>> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(TKey id, CancellationToken cancellationToken = default);
    }

}
