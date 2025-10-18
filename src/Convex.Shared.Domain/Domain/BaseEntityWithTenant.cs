using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convex.Shared.Domain.Domain
{
    public abstract class BaseEntityWithTenant : BaseEntity<int>
    {
        public required string TenantId { get; set; }
    }
}
