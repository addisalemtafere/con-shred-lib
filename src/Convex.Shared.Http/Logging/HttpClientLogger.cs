using Convex.Shared.Http.Logging.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json.Nodes;
using System.Xml;

namespace Convex.Shared.Http.Logging
{
    public class HttpClientLogger : IHttpClientLogger
    {
        private readonly ILogger<HttpClientLogger> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _serviceProvider;
        private readonly LogDataAccessor _logDataAccessor;

        public HttpClientLogger(ILogger<HttpClientLogger> logger
            , IHttpContextAccessor httpContextAccessor
            , IServiceProvider serviceProvider
            , LogDataAccessor logDataAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _serviceProvider = serviceProvider;
            _logDataAccessor = logDataAccessor;
        }

        public async Task Log(HttpRequestMessage request, HttpResponseMessage response, bool success, int responseTime, Exception exception = null)
        {
            var logData = new HttpClientLogData()
            {
                WebServiceUrl = request.RequestUri.AbsoluteUri,
                ApplicationKey = _logDataAccessor.GetApplicationKey(_serviceProvider), //Guid.Parse("CCC6EDA7-12BB-E611-9592-005056B86062"),
                ParticipantId = _logDataAccessor.GetParticipantId(_serviceProvider),
                HttpMethod = request.Method.ToString().ToUpper(),
                IpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString(),
                ErrorDescription = success ? null : response?.ReasonPhrase ?? exception?.Message,
                ResponseTime = responseTime,
                ResponseCode = response != null ? (int?)response.StatusCode : null,
                RequestBodyFormat = request.Content?.Headers.ContentType?.MediaType.IndexOf("xml", StringComparison.OrdinalIgnoreCase) >= 0
                    ? HttpBodyFormat.Xml : HttpBodyFormat.Json,
                ResponseBodyFormat = response?.Content?.Headers.ContentType?.MediaType.IndexOf("xml", StringComparison.OrdinalIgnoreCase) >= 0
                    ? HttpBodyFormat.Xml : HttpBodyFormat.Json
            };

            if (request.Content != null)
                logData.RawRequest = await request.Content.ReadAsStringAsync();

            if (response?.Content != null)
                logData.RawResponse = await response.Content.ReadAsStringAsync();

            await LogInternal(logData, request.ToString(), response?.ToString());
        }

        public async Task Log(HttpClientLogData logData)
        {
            await LogInternal(logData, null, null);
        }

        private async Task LogInternal(HttpClientLogData logData, string preBodyRequest, string preBodyResponse)
        {
            try
            {
                // Even though LoggingDbContext is scoped it exists within a separate DI scope due to HttpClient pooling
                // This causes concurrency issues since the same DbContext will be used in separate request scopes
                // To solve these we create a separate scope to retrieve a new instance of LoggingDbContext
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<LoggingDbContext>();

                // Get method that matches most closely to the url and http method, falling back on the null (any) http method
                var method = await dbContext.Methods
                    .Include(x => x.SensitiveFields)
                    .OrderByDescending(x => x.Url.Length)
                    .ThenByDescending(x => x.HttpMethod)
                    .FirstOrDefaultAsync(x =>
                        logData.WebServiceUrl.StartsWith(x.Url)
                        && (x.HttpMethod == logData.HttpMethod || x.HttpMethod == null));

                if (method == null)
                    return;

                var log = new Log()
                {
                    MethodId = method.Id,
                    ApplicationKey = logData.ApplicationKey ?? _logDataAccessor.GetApplicationKey(scope.ServiceProvider),
                    ParticipantId = logData.ParticipantId ?? _logDataAccessor.GetParticipantId(scope.ServiceProvider),
                    HttpMethod = logData.HttpMethod,
                    IpAddress = logData.IpAddress,
                    ResponseCode = logData.ResponseCode,
                    ResponseTime = logData.ResponseTime,
                    IsError = !string.IsNullOrWhiteSpace(logData.ErrorDescription),
                    ErrorDescription = logData.ErrorDescription
                };

                if (log.IsError || method.IsDataLoggedOnSuccess)
                {
                    log.RawRequest = MaskBody(logData.RawRequest, preBodyRequest, logData.RequestBodyFormat,
                        method.SensitiveFields.Where(s => s.IsRequest).Select(s => s.ElementName));

                    log.RawResponse = MaskBody(logData.RawResponse, preBodyResponse, logData.ResponseBodyFormat,
                        method.SensitiveFields.Where(s => s.IsResponse).Select(s => s.ElementName));
                }

                if (log.IsError || method.IsLoggedOnSuccess)
                {
                    dbContext.Log.Add(log);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Don't want application to crash when logging fails
                _logger.LogError(ex, "Failed to save to log table.");
                Debug.WriteLine(ex.ToString());
            }
        }

        private string MaskBody(string body, string preBody, HttpBodyFormat bodyFormat, IEnumerable<string> fieldsToMask)
        {
            string logBody = preBody + (string.IsNullOrWhiteSpace(preBody) ? null : Environment.NewLine);

            if (!string.IsNullOrWhiteSpace(body))
            {
                if (fieldsToMask.Any())
                {
                    logBody += bodyFormat == HttpBodyFormat.Json
                        ? MaskJsonSensitiveData(body, fieldsToMask)
                        : MaskXmlSensitiveData(body, fieldsToMask);
                }
                else
                {
                    logBody += body;
                }
            }

            return string.IsNullOrWhiteSpace(logBody) ? null : logBody;
        }

        private string MaskXmlSensitiveData(string body, IEnumerable<string> fieldsToMask)
        {
            var doc = new XmlDocument();
            try
            {
                doc.LoadXml(body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to mask XML sensitive data.");
                return body;
            }

            foreach (var field in fieldsToMask)
            {
                foreach (XmlNode node in doc.SelectNodes("//*[translate(local-name(), " +
                    "'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='" + field.ToLower() + "']"))
                {
                    //foreach (XmlNode node in doc.GetElementsByTagName(field))
                    if (!string.IsNullOrEmpty(node.InnerText))
                        node.InnerText = new string('*', node.InnerText.Length);
                }
            }

            return doc.InnerXml;
        }

        private string MaskJsonSensitiveData(string body, IEnumerable<string> fieldsToMask)
        {
            JsonNode node;
            try
            {
                node = JsonNode.Parse(body);
                MaskJsonField(node, fieldsToMask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to mask JSON sensitive data.");
                return body;
            }

            return node?.ToJsonString() ?? body;
        }

        private static void MaskJsonField(JsonNode node, IEnumerable<string> fieldsToMask)
        {
            if (node == null || node is JsonValue)
                return;

            if (node is JsonArray array)
            {
                for (int i = 0; i < array.Count; i++)
                {
                    MaskJsonField(node[i], fieldsToMask);
                }
            }
            else if (node is IDictionary<string, JsonNode> item)
            {
                foreach (var childKey in item.Keys.ToArray())
                {
                    if (fieldsToMask.Contains(childKey, StringComparer.OrdinalIgnoreCase))
                    {
                        node[childKey] = "******";
                    }
                    else
                    {
                        MaskJsonField(node[childKey], fieldsToMask);
                    }
                }
            }
        }
    }
}
