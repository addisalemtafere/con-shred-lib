using Convex.Shared.Http.Logging;
using Convex.Shared.Http.Logging.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Convex.Shared.Http.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the database where HttpClient's configured to log will save data.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="loggingConnectionString">PostgreSQL connection string of logging database.  If this is null then .AddDbContext()
        /// will be used and rely on the <see cref="IConnectionStringProvider"/> implementation.</param>
        public static IServiceCollection ConfigureHttpClientLogging(this IServiceCollection services, string loggingConnectionString,
            Func<IServiceProvider, int?> participantIdAccessor = null, Func<IServiceProvider, Guid?> applicationKeyAccessor = null)
        {
            if (!services.Any(x => x.ServiceType == typeof(LoggingDbContext)))
                services.AddDbContext<LoggingDbContext>(opts => opts.UseNpgsql(loggingConnectionString)); // Changed to UseNpgsql

            var dataAccessor = new LogDataAccessor
            {
                GetParticipantId = participantIdAccessor,
                GetApplicationKey = applicationKeyAccessor
            };

            services.TryAddSingleton(dataAccessor);
            services.TryAddTransient<HttpLoggingHandler>();
            services.TryAddScoped<Logging.IHttpClientLogger, HttpClientLogger>();
            services.AddHttpClient();
            return services;
        }

        /// <summary>
        /// Binds an <see cref="HttpClient"/> configured for logging to the type <typeparamref name="TClient"/>.
        /// </summary>
        /// <typeparam name="TClient">The type of the service that will be configured to be injected with a logging <see cref="HttpClient"/>.</typeparam>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        public static IHttpClientBuilder AddLoggingHttpClient<TClient>(this IServiceCollection services)
            where TClient : class
        {
            return services.AddHttpClient<TClient>()
                .AddHttpMessageHandler<HttpLoggingHandler>();
        }

        /// <summary>
        /// Binds an <see cref="HttpClient"/> configured for logging to the type <typeparamref name="TClient"/>.
        /// </summary>
        /// <typeparam name="TClient">The declared type of the service that will be configured to be injected with a logging <see cref="HttpClient"/>.</typeparam>
        /// <typeparam name="TImplementation">The implementation type of the service that will be configured to be injected with a logging <see cref="HttpClient"/>.</typeparam>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        public static IHttpClientBuilder AddLoggingHttpClient<TClient, TImplementation>(this IServiceCollection services)
            where TClient : class
            where TImplementation : class, TClient
        {
            return services.AddHttpClient<TClient, TImplementation>()
                .AddHttpMessageHandler<HttpLoggingHandler>();
        }
    }
}
