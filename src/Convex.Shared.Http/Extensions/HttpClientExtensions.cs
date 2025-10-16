using Microsoft.Net.Http.Headers;
using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace Convex.Shared.Http.Extensions
{
    public static class HttpClientExtensions
    {
        private static readonly string[] _contentHeaders = new[]
        {
            HeaderNames.Allow,
            HeaderNames.ContentDisposition,
            HeaderNames.ContentEncoding,
            HeaderNames.ContentLanguage,
            HeaderNames.ContentLength,
            HeaderNames.ContentLocation,
            HeaderNames.ContentMD5,
            HeaderNames.ContentRange,
            HeaderNames.ContentType,
            HeaderNames.Expires,
            HeaderNames.LastModified
        };
        private static readonly ConcurrentDictionary<Type, XmlSerializer> _serializers = new();
        private static readonly NamedLockProvider _locker = new();

        #region GET
        /// <summary>
        /// Sends an asynchronous GET HTTP request and deserializes the response as JSON.
        /// </summary>
        /// <typeparam name="TResponse">Type of the response object to deserialize.</typeparam>
        /// <param name="httpClient">The HttpClient sending the request.</param>
        /// <param name="url">Destination URL of the request.</param>
        /// <param name="headers">Dictionary of key/value pairs to send as headers.</param>
        /// <param name="options">The JsonSerializerOptions to use during deserialization.  Defaults to using camel-case naming convention.</param>
        /// <param name="cancellationToken">The cancellation token used in the HttpClient.SendAsync request.</param>
        public async static Task<TResponse> GetJsonAsync<TResponse>(this HttpClient httpClient,
            string url,
            Dictionary<string, string> headers = null,
            JsonSerializerOptions options = null,
            CancellationToken cancellationToken = default)
            where TResponse : class
        {
            return await httpClient.SendJsonAsync<TResponse>(HttpMethod.Get, url, null, headers, options, cancellationToken);
        }

        /// <summary>
        /// Sends an asynchronous GET HTTP request and deserializes the response as XML.
        /// </summary>
        /// <typeparam name="TResponse">Type of the response object to deserialize.</typeparam>
        /// <param name="httpClient">The HttpClient sending the request.</param>
        /// <param name="url">Destination URL of the request.</param>
        /// <param name="headers">Dictionary of key/value pairs to send as headers.</param>
        /// <param name="cancellationToken">The cancellation token used in the HttpClient.SendAsync request.</param>
        public async static Task<TResponse> GetXmlAsync<TResponse>(this HttpClient httpClient,
            string url,
            Dictionary<string, string> headers = null,
            CancellationToken cancellationToken = default)
            where TResponse : class
        {
            return await httpClient.SendXmlAsync<object, TResponse>(HttpMethod.Get, url, null, headers, cancellationToken);
        }

        /// <summary>
        /// Sends a synchronous GET HTTP request and deserializes the response as JSON.
        /// </summary>
        /// <typeparam name="TResponse">Type of the response object to deserialize.</typeparam>
        /// <param name="httpClient">The HttpClient sending the request.</param>
        /// <param name="url">Destination URL of the request.</param>
        /// <param name="headers">Dictionary of key/value pairs to send as headers.</param>
        /// <param name="options">The JsonSerializerOptions to use during deserialization.  Defaults to using camel-case naming convention.</param>
        public static TResponse GetJson<TResponse>(this HttpClient httpClient,
            string url,
            Dictionary<string, string> headers = null,
            JsonSerializerOptions options = null)
            where TResponse : class
        {
            return httpClient.SendJson<TResponse>(HttpMethod.Get, url, null, headers, options);
        }

        /// <summary>
        /// Sends a synchronous GET HTTP request and deserializes the response as XML.
        /// </summary>
        /// <typeparam name="TResponse">Type of the response object to deserialize.</typeparam>
        /// <param name="httpClient">The HttpClient sending the request.</param>
        /// <param name="url">Destination URL of the request.</param>
        /// <param name="headers">Dictionary of key/value pairs to send as headers.</param>
        public static TResponse GetXml<TResponse>(this HttpClient httpClient,
            string url,
            Dictionary<string, string> headers = null)
            where TResponse : class
        {
            return httpClient.SendXml<object, TResponse>(HttpMethod.Get, url, null, headers);
        }
        #endregion

        #region POST
        /// <summary>
        /// Serializes a request object as JSON and sends it in an asynchronous POST HTTP request.
        /// </summary>
        /// <typeparam name="TResponse">Type of the response object to deserialize.</typeparam>
        /// <param name="httpClient">The HttpClient sending the request.</param>
        /// <param name="url">Destination URL of the request.</param>
        /// <param name="value">Request object to serialize to JSON and send in the request.</param>
        /// <param name="headers">Dictionary of key/value pairs to send as headers.</param>
        /// <param name="options">The JsonSerializerOptions to use during deserialization.  Defaults to using camel-case naming convention.</param>
        /// <param name="cancellationToken">The cancellation token used in the HttpClient.SendAsync request.</param>
        public async static Task<TResponse> PostJsonAsync<TResponse>(this HttpClient httpClient,
            string url,
            object value = null,
            Dictionary<string, string> headers = null,
            JsonSerializerOptions options = null,
            CancellationToken cancellationToken = default)
            where TResponse : class
        {
            return await httpClient.SendJsonAsync<TResponse>(HttpMethod.Post, url, value, headers, options, cancellationToken);
        }

        /// <summary>
        /// Serializes a request object as XML and sends it in an asynchronous POST HTTP request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request object to serialize..</typeparam>
        /// <typeparam name="TResponse">Type of the response object to deserialize.</typeparam>
        /// <param name="httpClient">The HttpClient sending the request.</param>
        /// <param name="url">Destination URL of the request.</param>
        /// <param name="value">Request object to serialize to XML and send in the request.</param>
        /// <param name="headers">Dictionary of key/value pairs to send as headers.</param>
        /// <param name="cancellationToken">The cancellation token used in the HttpClient.SendAsync request.</param>
        public async static Task<TResponse> PostXmlAsync<TRequest, TResponse>(this HttpClient httpClient,
            string url,
            TRequest value = default,
            Dictionary<string, string> headers = null,
            CancellationToken cancellationToken = default)
            where TRequest : class
            where TResponse : class
        {
            return await httpClient.SendXmlAsync<TRequest, TResponse>(HttpMethod.Post, url, value, headers, cancellationToken);
        }

        /// <summary>
        /// Serializes a request object as JSON and sends it in a synchronous POST HTTP request.
        /// </summary>
        /// <typeparam name="TResponse">Type of the response object to deserialize.</typeparam>
        /// <param name="httpClient">The HttpClient sending the request.</param>
        /// <param name="url">Destination URL of the request.</param>
        /// <param name="value">Request object to serialize to JSON and send in the request.</param>
        /// <param name="headers">Dictionary of key/value pairs to send as headers.</param>
        /// <param name="options">The JsonSerializerOptions to use during deserialization.  Defaults to using camel-case naming convention.</param>
        public static TResponse PostJson<TResponse>(this HttpClient httpClient,
            string url,
            object value = null,
            Dictionary<string, string> headers = null,
            JsonSerializerOptions options = null)
            where TResponse : class
        {
            return httpClient.SendJson<TResponse>(HttpMethod.Post, url, value, headers, options);
        }

        /// <summary>
        /// Serializes a request object as XML and sends it in a synchronous POST HTTP request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request object to serialize..</typeparam>
        /// <typeparam name="TResponse">Type of the response object to deserialize.</typeparam>
        /// <param name="httpClient">The HttpClient sending the request.</param>
        /// <param name="url">Destination URL of the request.</param>
        /// <param name="value">Request object to serialize to XML and send in the request.</param>
        /// <param name="headers">Dictionary of key/value pairs to send as headers.</param>
        public static TResponse PostXml<TRequest, TResponse>(this HttpClient httpClient,
            string url,
            TRequest value = default,
            Dictionary<string, string> headers = null)
            where TRequest : class
            where TResponse : class
        {
            return httpClient.SendXml<TRequest, TResponse>(HttpMethod.Post, url, value, headers);
        }
        #endregion

        #region Send
        /// <summary>
        /// Serializes a request object as JSON and sends it in an asynchronous HTTP request.
        /// </summary>
        /// <typeparam name="TResponse">Type of the response object to deserialize.</typeparam>
        /// <param name="httpClient">The HttpClient sending the request.</param>
        /// <param name="method">The HTTP method of the request.</param>
        /// <param name="url">Destination URL of the request.</param>
        /// <param name="value">Request object to serialize to JSON and send in the request.</param>
        /// <param name="headers">Dictionary of key/value pairs to send as headers.</param>
        /// <param name="options">The JsonSerializerOptions to use during deserialization.  Defaults to using camel-case naming convention.</param>
        /// <param name="cancellationToken">The cancellation token used in the HttpClient.SendAsync request.</param>
        public async static Task<TResponse> SendJsonAsync<TResponse>(this HttpClient httpClient,
            HttpMethod method,
            string url,
            object value = null,
            Dictionary<string, string> headers = null,
            JsonSerializerOptions options = null,
            CancellationToken cancellationToken = default)
            where TResponse : class
        {
            return await SendAsync<TResponse>(httpClient, method, url, GetJsonContent(value, options), headers, options, true, cancellationToken);
        }

        /// <summary>
        /// Serializes a request object as XML and sends it in an asynchronous HTTP request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request object to serialize..</typeparam>
        /// <typeparam name="TResponse">Type of the response object to deserialize.</typeparam>
        /// <param name="httpClient">The HttpClient sending the request.</param>
        /// <param name="method">The HTTP method of the request.</param>
        /// <param name="url">Destination URL of the request.</param>
        /// <param name="value">Request object to serialize to XML and send in the request.</param>
        /// <param name="headers">Dictionary of key/value pairs to send as headers.</param>
        /// <param name="cancellationToken">The cancellation token used in the HttpClient.SendAsync request.</param>
        public async static Task<TResponse> SendXmlAsync<TRequest, TResponse>(this HttpClient httpClient,
            HttpMethod method,
            string url,
            TRequest value = default,
            Dictionary<string, string> headers = null,
            CancellationToken cancellationToken = default)
            where TRequest : class
            where TResponse : class
        {
            return await SendAsync<TResponse>(httpClient, method, url, GetXmlContent(value), headers, null, true, cancellationToken);
        }

        /// <summary>
        /// Serializes a request object as JSON and sends it in a synchronous HTTP request.
        /// </summary>
        /// <typeparam name="TResponse">Type of the response object to deserialize.</typeparam>
        /// <param name="httpClient">The HttpClient sending the request.</param>
        /// <param name="method">The HTTP method of the request.</param>
        /// <param name="url">Destination URL of the request.</param>
        /// <param name="value">Request object to serialize to JSON and send in the request.</param>
        /// <param name="headers">Dictionary of key/value pairs to send as headers.</param>
        /// <param name="options">The JsonSerializerOptions to use during deserialization.  Defaults to using camel-case naming convention.</param>
        public static TResponse SendJson<TResponse>(this HttpClient httpClient,
            HttpMethod method,
            string url,
            object value = null,
            Dictionary<string, string> headers = null,
            JsonSerializerOptions options = null)
            where TResponse : class
        {
            return GetResult(SendAsync<TResponse>(httpClient, method, url, GetJsonContent(value, options), headers, options, false));
        }

        /// <summary>
        /// Serializes a request object as XML and sends it in a synchronous HTTP request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request object to serialize..</typeparam>
        /// <typeparam name="TResponse">Type of the response object to deserialize.</typeparam>
        /// <param name="httpClient">The HttpClient sending the request.</param>
        /// <param name="method">The HTTP method of the request.</param>
        /// <param name="url">Destination URL of the request.</param>
        /// <param name="value">Request object to serialize to XML and send in the request.</param>
        /// <param name="headers">Dictionary of key/value pairs to send as headers.</param>
        public static TResponse SendXml<TRequest, TResponse>(this HttpClient httpClient,
            HttpMethod method,
            string url,
            TRequest value = default,
            Dictionary<string, string> headers = null)
            where TRequest : class
            where TResponse : class
        {
            return GetResult(SendAsync<TResponse>(httpClient, method, url, GetXmlContent(value), headers, null, false));
        }
        #endregion

        #region Private Methods
        private static async Task<TResponse> SendAsync<TResponse>(HttpClient httpClient,
            HttpMethod method,
            string url,
            HttpContent content,
            Dictionary<string, string> headers,
            JsonSerializerOptions options,
            bool doAsync,
            CancellationToken cancellationToken = default)
            where TResponse : class
        {
            options ??= new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true };
            using var request = new HttpRequestMessage(method, url);
            request.Content = content;
            AddHeaders(request, headers);

            HttpResponseMessage response = doAsync
                ? await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false)
                : httpClient.Send(request, cancellationToken);

            return await ValidateAndDeserializeResponse<TResponse>(response, options, doAsync, cancellationToken);
        }

        private static async Task<TResponse> ValidateAndDeserializeResponse<TResponse>(HttpResponseMessage response,
            JsonSerializerOptions options,
            bool doAsync,
            CancellationToken cancellationToken = default
            )
            where TResponse : class
        {
            var contentType = response.Content!.Headers.ContentType;
            TResponse responseObject = default;
            string responseBody = null;

            if (contentType != null)
            {
                responseBody = await ReadAsString(response, doAsync, cancellationToken);
                responseObject = GetResponseObject<TResponse>(responseBody, contentType.MediaType, options);
            }

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (responseBody != null)
                {
                    ex.Data.Add("ResponseBody", responseBody);
                    ex.Data.Add("ResponseObject", responseObject);
                }
                throw;
            }

            return responseObject;
        }

        private static TResponse GetResponseObject<TResponse>(string responseBody, string contentType, JsonSerializerOptions options)
            where TResponse : class
        {
            if (contentType.Contains("json", StringComparison.OrdinalIgnoreCase) || IsValidJson(responseBody))
                return JsonSerializer.Deserialize<TResponse>(responseBody, options);

            if (contentType.Contains("xml", StringComparison.OrdinalIgnoreCase))
                return DeserializeXml<TResponse>(responseBody);

            return responseBody as TResponse;
        }

        private static bool IsValidJson(string input)
        {
            return !string.IsNullOrWhiteSpace(input) &&
                ((input.StartsWith('{') && input.EndsWith('}'))
                || (input.StartsWith('[') && input.EndsWith(']')));
        }

        private static JsonContent GetJsonContent(object value, JsonSerializerOptions options)
        {
            return value == default ? null : JsonContent.Create(value, new MediaTypeWithQualityHeaderValue("application/json"), options);
        }

        private static StringContent GetXmlContent<T>(T value)
            where T : class
        {
            var content = new StringContent(SerializeXml(value));
            content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/xml");
            return content;
        }

        private static async Task<string> ReadAsString(HttpResponseMessage response, bool doAsync, CancellationToken cancellationToken)
        {
            return doAsync
                ? await response.Content.ReadAsStringAsync(cancellationToken)
                : response.Content.ReadAsStringAsync(cancellationToken).GetAwaiter().GetResult();
        }

        private static T DeserializeXml<T>(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return default;

            var serializer = GetSerializer<T>();
            using TextReader reader = new StringReader(xml);
            using XmlTextReader xmlTextReader = new(reader);
            xmlTextReader.XmlResolver = null;
            xmlTextReader.DtdProcessing = DtdProcessing.Ignore;
            return (T)serializer.Deserialize(xmlTextReader);
        }

        private static string SerializeXml<T>(T value)
            where T : class
        {
            if (value == default)
                return null;

            var xmlSerializer = GetSerializer<T>();
            using var stringWriter = new Utf8StringWriter();
            using XmlWriter writer = XmlWriter.Create(stringWriter);
            xmlSerializer.Serialize(writer, value);
            return stringWriter.ToString();
        }

        private static XmlSerializer GetSerializer<T>()
        {
            if (_serializers.TryGetValue(typeof(T), out XmlSerializer serializer))
            {
                return serializer;
            }

            lock (_locker.GetLock(typeof(T).FullName))
            {
                return _serializers.GetOrAdd(typeof(T), _ => new XmlSerializer(typeof(T)));
            }
        }

        /// <summary>
        /// Must check if header is a content header and add to appropriate header collection because Microsoft implemented this poorly.
        /// </summary>
        private static void AddHeaders(HttpRequestMessage request, Dictionary<string, string> headers)
        {
            if (headers == null)
                return;

            foreach (var header in headers)
            {
                if (_contentHeaders.Contains(header.Key))
                {
                    request.Content.Headers.Add(header.Key, header.Value);
                }
                else
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
        }

        /// <summary>
        /// Returns the result of a Task for synchronous operations.  Unwraps AggregateException if an exception occurs prior to completion.
        /// </summary>
        private static TResponse GetResult<TResponse>(Task<TResponse> task)
        {
            if (task.IsCompletedSuccessfully)
                return task.Result;

            if (task.IsFaulted)
                throw task.Exception.InnerException;

            return task.GetAwaiter().GetResult();
        }
        #endregion
    }
}
