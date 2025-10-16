using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Convex.Shared.Http.Logging
{
    public class HttpLoggingHandler : DelegatingHandler
    {
        private readonly ILogger<HttpLoggingHandler> _logger;
        private readonly IHttpClientLogger _httpLogger;

        public HttpLoggingHandler(ILogger<HttpLoggingHandler> logger
            , IHttpClientLogger httpLogger)
        {
            _logger = logger;
            _httpLogger = httpLogger;
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return SendAsync(request, cancellationToken).GetAwaiter().GetResult();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            Stopwatch timer = new();

            try
            {
                timer.Start();
                response = await base.SendAsync(request, cancellationToken);
                timer.Stop();
                int elapsedTime = Convert.ToInt32(timer.ElapsedMilliseconds);
                await _httpLogger.Log(request, response, response?.IsSuccessStatusCode ?? false, elapsedTime);
            }
            catch (Exception ex)
            {
                if (timer.IsRunning) timer.Stop();
                _logger.LogError(ex, "Error in HttpLoggingHandler.SendAsync");

                int elapsedTime = Convert.ToInt32(timer.ElapsedMilliseconds);
                await _httpLogger.Log(request, response, false, elapsedTime, ex);
                throw;
            }

            return response;
        }
    }
}
