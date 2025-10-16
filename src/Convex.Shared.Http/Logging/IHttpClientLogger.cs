namespace Convex.Shared.Http.Logging
{
    public interface IHttpClientLogger
    {
        Task Log(HttpClientLogData logData);
        Task Log(HttpRequestMessage request, HttpResponseMessage response, bool success, int responseTime, Exception exception = null);
    }
}
