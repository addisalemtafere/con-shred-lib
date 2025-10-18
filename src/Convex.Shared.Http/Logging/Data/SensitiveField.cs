using System.ComponentModel.DataAnnotations.Schema;

namespace Convex.Shared.Http.Logging.Data
{
    public class SensitiveField
    {
        public int Id { get; set; }
        public int MethodId { get; set; }
        public virtual Method Method { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string ElementName { get; set; }

        public bool IsRequest { get; set; }
        public bool IsResponse { get; set; }
    }
}
