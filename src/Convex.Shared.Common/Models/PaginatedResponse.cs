using Microsoft.EntityFrameworkCore;

namespace Convex.Shared.Common.Models
{
    public class PaginatedResponse<T>
    {
        private PaginatedResponse(List<T> items, int pageNum, int pageSize, int totalCount)
        {
            Items = items;
            PageNum = pageNum;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public List<T> Items { get; }
        public int PageNum { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public bool HasNextPage => PageNum * PageSize < TotalCount;
        public bool HasPreviousPage => PageNum > 1;

        public static async Task<PaginatedResponse<T>> CreateAsync(IQueryable<T> query, int pageNum, int pageSize, CancellationToken cancellationToken)
        {
            if (pageNum == 0)
            {
                pageNum = 1;
            }

            if (pageSize == 0)
            {
                pageSize = query.Count();
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new(items, pageNum, pageSize, totalCount);
        }

    }
}
