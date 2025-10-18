using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convex.Shared.Common.Repository.Interfaces
{
    public class PaginatedSpecification<T> : BaseSpecification<T>
    {
        public PaginatedSpecification(int pageNumber, int pageSize)
        {
            if (pageSize > 0 && pageNumber > 0)
            {
                var skip = (pageNumber - 1) * pageSize;
                ApplyPaging(skip, pageSize);
            }
        }
    }
}
