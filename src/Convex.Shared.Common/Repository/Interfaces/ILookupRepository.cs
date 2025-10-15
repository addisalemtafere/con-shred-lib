using Convex.Shared.Common.Models;
using System.Linq.Expressions;

namespace Convex.Shared.Common.Repository.Interfaces
{
    public interface ILookupRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAllPaginated(int pageSize, int pageNumber);
        Task<PaginatedResponse<TEntity>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetActiveAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TLookup>> GetLookupAsync<TLookup>(
            Expression<Func<TEntity, TLookup>> selector,
            CancellationToken cancellationToken = default);
    }
}
