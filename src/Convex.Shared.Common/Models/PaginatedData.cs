using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convex.Shared.Common.Models
{
    // Supporting class for the paginated data structure
    public class PaginatedData<T>
    {
        public List<T> Items { get; set; } = new();
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasNextPage => PageNum * PageSize < TotalCount;
        public bool HasPreviousPage => PageNum > 1;
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;
    }
}
