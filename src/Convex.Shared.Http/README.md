HttpClient Logging
This library contains an HttpMessageHandler that can be used with the standard HttpClient to log request information to a database log table.

Data Model
The primary configuration of the handler is controlled at the database level:

Method
Contains the definition of a web service method that should be logged to the database.

Name - The friendly name of the web service method.
Url - The base URL of the method. This should be the full domain plus the application path and route of the method without any query string parameters.
HttpMethod - The HTTP verb (GET, POST, PUT, etc.) associated with the method. If the method allows for multiple verbs you may either create multiple "Method" entries for each HttpMethod (same URL) OR set the HttpMethod to be null, which will use the same method entry regardless of the supplied verb.
IsLoggedOnSuccess - Whether the method request instance is logged to the Logs table even on a success.
IsDataLoggedOnSuccess - Whether the raw request and response text should be logged even on a success.
If an error occurs during the request, whether it's a fatal error or an HTTP response code indicating an error (4xx or 5xx) the request will be logged along with the raw request and response data. IsLoggedOnSuccess and IsDataLoggedOnSuccess are not checked in this instance.

Log
A specific instance of a logged request.

MethodId - Method tied to the log entry.
HttpMethod - The HTTP verb used in the request.
ApplicationKey - Application key of the context when the request was executed. This field is set based on the configuration in Startup.
ParticipantId - Participant ID of the context when the request was executed. This field is set based on the configuration in Startup.
ResponseCode - The HTTP status code of the response.
IpAddress - IP address of the user invoking the request.
IsError - Whether an error occurred during the request.
ErrorDescription - Description of the error that occurred.
RawRequest - The full request body ncluding header information.
RawResponse - The full response body ncluding header information.
Sensitive Field
Some requests contain sensitive information in either the request body or response body and must be masked before being logged to the database. These entities specify which fields are sensitive and thus should be masked in either the request or response.

MethodId - Method tied to the sensitive field.
ElementName - The name of the JSON property or XML element that should be masked.
IsRequest - Whether the field is contained in the request body.
IsResponse - Whether the field is contained in the response body.
IHttpClientLogger
This interface defines the class that will perform the actual work of logging the request information to the database. The default implementation, HttpClientLogger, is automatically registered during configuration.


Task Log(HttpClientLogData logData)
Logs the information provided in the HttpClientLogData to the database. This is the method that should be used when this class is injected into service for the purpose of manually logging request data (i.e. when the HttpClient is in a 3rd party library and isn't handled by the HttpClientPool). HttpClientLogData's fields are the same as those defined on the Log entity except for the following:

RequestBodyFormat - The format of the data provided in the RawRequest property. This is used when the request is being parsed to mask sensitive fields. Can be either JSON or XML.
ResponseBodyFormat - The format of the data provided in the RawResponse property. This is used when the response is being parsed to mask sensitive fields. Can be either JSON or XML.

Task Log(HttpRequestMessage request, HttpResponseMessage response,
    bool success, int responseTime, Exception exception = null)
An overload the Log method that is used by the HttpLoggingHandler directly.

Configuration
There are 2 IServiceCollection extension methods that are used to configure the HttpClient logging framework:


ConfigureHttpClientLogging(this IServiceCollection services,
    string loggingConnectionString, 
    Func<IServiceProvider, int?> participantIdAccessor = null
    Func<IServiceProvider, Guid?> applicationKeyAccessor = null)
loggingConnectionString - The SQL Server connection string of the database that houses the Data Model.
participantIdAccessor - A delegate that is used when a request is logged to the database that populates the ParticipantId field on the Log entity.
ApplicationKeyAccessor - A delegate that is used when a request is logged to the database that populates the ParticipantId field on the Log entity.
This method configures the Data Model and registers the HttpLoggingHandler used within the HttpClient instance. It also registers the default implementation of the IHttpClientLogger service class that is used to log the request to the database.

NOTE: If you create your own implementation of IHttpLoggingHandler you must register it before calling the above method.


AddLoggingHttpClient<TClient>(this IServiceCollection services)
AddLoggingHttpClient<TClient, TImplementation>(this IServiceCollection services)
Registers a class, or an implementation of an interface, with an HttpClient that uses the HttpLoggingHandler. This method must be called when you want a service class to use HttpClient and have it logged to the database. This allows an instance of HttpClient to be injected into the constructor of the class. The HttpClient instance can be used normally and does not require any special handling in order for the logging to work.

Example
Below is an example of a Startup class that is configured to use the HttpClient logger.


services.ConfigureHttpClientLogging(Configuration.GetConnectionString(nameof(LoggingDbContext)), null,
    sp =>
    {
        var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
        string key = _httpContextAccessor.HttpContext.Request.Headers["X-ApplicationKey"];
        return Guid.TryParse(key, out Guid applicationId) ? applicationId : null;
    });
services.AddLoggingHttpClient<MyService>();
services.AddLoggingHttpClient<IServiceInterface, ServiceImplementation>();
services.AddHttpClient<NonLoggingService>();
In the above example we first call ConfigureHttpClientLogging passing in the connectionstring from the configuration file. We pass in null for the participantIdAccessor parameter indicating that the ParticipantId field should be left as null when logged to the database. For the applicationKeyAccessor we retrieve the IHttpContextAccessor from the service provider and get the value from the header.

After that, we call AddLoggingHttpClient on the service MyService directly and then on the implementation of IServiceInterface, ServiceImplementation. Finally, we call the built in AddHttpClient method to register NonLoggingService. The last line does the same as AddLoggingHttpClient except it injects an HttpClient that doesn't have the HttpLoggingHandler. For cases where we know we will not be logging any requests to the database we should use AddHttpClient to avoid the overhead of using HttpLoggingHandler.

In the services we register such as MyService we can inject an instance of HttpClient in the constructor and use it in the standard fashion.