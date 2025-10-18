using System.ComponentModel.DataAnnotations.Schema;

namespace Convex.Shared.Http.Logging.Data
{
    public class Method
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string Name { get; set; }

        [Column(TypeName = "VARCHAR(200)")]
        public string Url { get; set; }

        [Column(TypeName = "VARCHAR(15)")]
        public string HttpMethod { get; set; }

        public bool IsLoggedOnSuccess { get; set; }
        public bool IsDataLoggedOnSuccess { get; set; }
        public virtual ICollection<SensitiveField> SensitiveFields { get; set; } = new List<SensitiveField>();
    }
}
