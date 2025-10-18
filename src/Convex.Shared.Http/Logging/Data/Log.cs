using System.ComponentModel.DataAnnotations.Schema;

namespace Convex.Shared.Http.Logging.Data
{
    [Table("Log")]
    public class Log
    {
        public int Id { get; set; }
        public int MethodId { get; set; }
        public virtual Method Method { get; set; }
        public Guid? ApplicationKey { get; set; }
        public int? ParticipantId { get; set; }
        public int? ResponseCode { get; set; }
        public int ResponseTime { get; set; }

        [Column("IsMethodError")]
        public bool IsError { get; set; }

        [Column(TypeName = "VARCHAR(15)")]
        public string HttpMethod { get; set; }

        [Column(TypeName = "VARCHAR(45)")]
        public string IpAddress { get; set; }

        [Column(TypeName = "NVARCHAR(500)")]
        public string ErrorDescription { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string RawRequest { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string RawResponse { get; set; }
    }
}
