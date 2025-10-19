using Convex.Shared.Messaging.Configuration;
using Convex.Shared.Messaging.Consumers;
using Convex.Shared.Messaging.Producers;
using Convex.Shared.Messaging.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Convex.Shared.Messaging.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        // Correct way to configure options with IConfiguration
        services.Configure<KafkaConfig>(configuration.GetSection("Kafka"));

        // Register generic producers and consumers
        services.AddTransient(typeof(IKafkaProducer<>), typeof(KafkaProducer<>));
        services.AddTransient(typeof(IKafkaProducer<,>), typeof(KafkaProducer<,>));
        services.AddTransient(typeof(IKafkaConsumer<>), typeof(KafkaConsumer<>));
        services.AddTransient(typeof(IKafkaConsumer<,>), typeof(KafkaConsumer<,>));

        return services;
    }

    public static IServiceCollection AddKafkaProducer<TValue>(this IServiceCollection services)
        where TValue : class
    {
        services.AddTransient<IKafkaProducer<TValue>, KafkaProducer<TValue>>();
        return services;
    }

    public static IServiceCollection AddKafkaProducer<TKey, TValue>(this IServiceCollection services)
        where TValue : class
    {
        services.AddTransient<IKafkaProducer<TKey, TValue>, KafkaProducer<TKey, TValue>>();
        return services;
    }

    public static IServiceCollection AddKafkaConsumer<TValue>(this IServiceCollection services)
        where TValue : class
    {
        services.AddTransient<IKafkaConsumer<TValue>, KafkaConsumer<TValue>>();
        return services;
    }

    public static IServiceCollection AddKafkaConsumer<TKey, TValue>(this IServiceCollection services)
        where TValue : class
    {
        services.AddTransient<IKafkaConsumer<TKey, TValue>, KafkaConsumer<TKey, TValue>>();
        return services;
    }

    public static IServiceCollection AddMessageSerializer<T>(this IServiceCollection services,
        Func<IServiceProvider, IMessageSerializer<T>> implementationFactory)
    {
        services.AddTransient(implementationFactory);
        return services;
    }
}