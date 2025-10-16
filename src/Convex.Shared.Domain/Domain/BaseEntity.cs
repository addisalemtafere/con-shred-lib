using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convex.Shared.Domain.Domain
{
    public abstract class BaseEntity<TKey>
    {
        public required TKey Id { get; set; }

        public required string CreatedBy { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required string CreatedWs { get; set; }
        public required string LastModifiedBy { get; set; }
        public required DateTime LastModifiedAt { get; set; }
        public required string LastModifiedWs { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; } // used for optimistic concurrency
    }
}
