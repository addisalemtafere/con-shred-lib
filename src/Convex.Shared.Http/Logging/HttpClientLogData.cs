namespace Convex.Shared.Http.Logging
{
    public class HttpClientLogData
    {
        public string WebServiceUrl { get; set; }
        public Guid? ApplicationKey { get; set; }
        public int? ParticipantId { get; set; }
        public string HttpMethod { get; set; }
        public string IpAddress { get; set; }
        public int? ResponseCode { get; set; }
        public int ResponseTime { get; set; }
        public string ErrorDescription { get; set; }
        public HttpBodyFormat RequestBodyFormat { get; set; } = HttpBodyFormat.Json;
        public HttpBodyFormat ResponseBodyFormat { get; set; } = HttpBodyFormat.Json;
        public string RawRequest { get; set; }
        public string RawResponse { get; set; }
    }
}
