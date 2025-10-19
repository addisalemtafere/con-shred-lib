using Convex.Shared.Messaging.Interfaces;
using Convex.Shared.Messaging.Configuration;
using Microsoft.Extensions.Options;
using Confluent.Kafka;
using System.Text.Json;
using ProducerConfig = Confluent.Kafka.ProducerConfig;
using ConsumerConfig = Confluent.Kafka.ConsumerConfig;

namespace Convex.Shared.Messaging.Services;

/// <summary>
/// Message bus implementation for Convex microservices using Kafka
/// </summary>
public class ConvexMessageBus : IConvexMessageBus, IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly IConsumer<string, string> _consumer;
    private readonly ConvexMessagingOptions _options;
    private readonly Dictionary<string, CancellationTokenSource> _subscriptions = new();

    public ConvexMessageBus(IOptions<ConvexMessagingOptions> options)
    {
        _options = options.Value;
        
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _options.BootstrapServers,
            SecurityProtocol = Enum.Parse<SecurityProtocol>(_options.SecurityProtocol),
            SaslMechanism = Enum.Parse<SaslMechanism>(_options.SaslMechanism),
            SaslUsername = _options.SaslUsername,
            SaslPassword = _options.SaslPassword,
            SslCaLocation = _options.SslCaLocation,
            RequestTimeoutMs = _options.RequestTimeoutMs,
            MessageTimeoutMs = _options.RequestTimeoutMs
        };

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _options.BootstrapServers,
            GroupId = _options.ConsumerGroup,
            SecurityProtocol = Enum.Parse<SecurityProtocol>(_options.SecurityProtocol),
            SaslMechanism = Enum.Parse<SaslMechanism>(_options.SaslMechanism),
            SaslUsername = _options.SaslUsername,
            SaslPassword = _options.SaslPassword,
            SslCaLocation = _options.SslCaLocation,
            AutoOffsetReset = Enum.Parse<AutoOffsetReset>(_options.AutoOffsetReset),
            EnableAutoCommit = _options.EnableAutoCommit,
            AutoCommitIntervalMs = _options.AutoCommitIntervalMs,
            SessionTimeoutMs = _options.SessionTimeoutMs
        };

        _producer = new ProducerBuilder<string, string>(producerConfig).Build();
        _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
    }

    public async Task<bool> PublishAsync<T>(string topic, T message)
    {
        try
        {
            var fullTopic = $"{_options.TopicPrefix}.{topic}";
            var json = JsonSerializer.Serialize(message);
            var kafkaMessage = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = json,
                Headers = new Headers
                {
                    { "message-type", System.Text.Encoding.UTF8.GetBytes(typeof(T).Name) },
                    { "timestamp", System.Text.Encoding.UTF8.GetBytes(DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()) }
                }
            };

            var result = await _producer.ProduceAsync(fullTopic, kafkaMessage);
            return result.Status == PersistenceStatus.Persisted;
        }
        catch
        {
            return false;
        }
    }

    public Task<string> SubscribeAsync<T>(string topic, Func<T, Task> handler)
    {
        try
        {
            var fullTopic = $"{_options.TopicPrefix}.{topic}";
            var subscriptionId = Guid.NewGuid().ToString();
            var cancellationTokenSource = new CancellationTokenSource();
            
            _subscriptions[subscriptionId] = cancellationTokenSource;
            
            _consumer.Subscribe(fullTopic);
            
            _ = Task.Run(async () =>
            {
                try
                {
                    while (!cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        var consumeResult = _consumer.Consume(cancellationTokenSource.Token);
                        if (consumeResult?.Message?.Value != null)
                        {
                            var message = JsonSerializer.Deserialize<T>(consumeResult.Message.Value);
                            if (message != null)
                            {
                                await handler(message);
                            }
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Expected when cancellation is requested
                }
            }, cancellationTokenSource.Token);
            
            // Add a small delay to ensure subscription is set up
            return Task.Delay(100, cancellationTokenSource.Token)
                .ContinueWith(_ => subscriptionId, TaskContinuationOptions.OnlyOnRanToCompletion);
        }
        catch
        {
            return Task.FromResult(string.Empty);
        }
    }

    public Task<bool> UnsubscribeAsync(string subscriptionId)
    {
        try
        {
            if (_subscriptions.TryGetValue(subscriptionId, out var cancellationTokenSource))
            {
                cancellationTokenSource.Cancel();
                _subscriptions.Remove(subscriptionId);
                
                // Add a small delay to ensure cleanup is complete
                return Task.Delay(50)
                    .ContinueWith(_ => true, TaskContinuationOptions.OnlyOnRanToCompletion);
            }
            return Task.FromResult(false);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public Task<bool> SendAsync<T>(string topic, T message)
    {
        // For Kafka, send is the same as publish
        return PublishAsync(topic, message);
    }

    public Task<bool> ReceiveAsync<T>(string topic, Func<T, Task> handler)
    {
        // For Kafka, receive is the same as subscribe
        return SubscribeAsync(topic, handler).ContinueWith(t => !string.IsNullOrEmpty(t.Result));
    }


    public void Dispose()
    {
        foreach (var subscription in _subscriptions.Values)
        {
            subscription.Cancel();
        }
        _subscriptions.Clear();
        
        _producer?.Dispose();
        _consumer?.Dispose();
    }
}
