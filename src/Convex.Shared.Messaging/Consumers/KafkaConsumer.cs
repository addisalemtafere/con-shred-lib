using Convex.Shared.Messaging.Configuration;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Convex.Shared.Messaging.Configuration;
using Convex.Shared.Messaging.Serialization;
using ConsumerConfig = Confluent.Kafka.ConsumerConfig;

namespace Convex.Shared.Messaging.Consumers
{
    public class KafkaConsumer<TKey, TValue> : IKafkaConsumer<TKey, TValue>
    {
        private readonly IConsumer<TKey, TValue> _consumer;
        private readonly ILogger<KafkaConsumer<TKey, TValue>> _logger;
        private bool _disposed = false;
        private Task? _consumingTask;

        public event EventHandler<ConsumeResult<TKey, TValue>>? OnMessageReceived;
        public event EventHandler<Error>? OnError;

        public KafkaConsumer(
            IOptions<KafkaConfig> kafkaConfig,
            IMessageSerializer<TValue>? serializer = null,
            ILogger<KafkaConsumer<TKey, TValue>>? logger = null)
        {
            _logger = logger;

            var config = new ConsumerConfig
            {
                BootstrapServers = kafkaConfig.Value.BootstrapServers,
                GroupId = kafkaConfig.Value.GroupId,
                ClientId = kafkaConfig.Value.Consumer.ClientId,
                EnableAutoCommit = kafkaConfig.Value.EnableAutoCommit,
                AutoOffsetReset = (AutoOffsetReset)Enum.Parse(typeof(AutoOffsetReset), kafkaConfig.Value.AutoOffsetReset),
                MaxPollIntervalMs = kafkaConfig.Value.Consumer.MaxPollIntervalMs,
                FetchMaxBytes = kafkaConfig.Value.Consumer.FetchMaxBytes,
                FetchWaitMaxMs = kafkaConfig.Value.Consumer.FetchWaitMaxMs,
                MaxPartitionFetchBytes = kafkaConfig.Value.Consumer.MaxPartitionFetchBytes,
                SessionTimeoutMs = kafkaConfig.Value.SessionTimeoutMs,
                ApiVersionRequestTimeoutMs = kafkaConfig.Value.RequestTimeoutMs
            };

            var consumerBuilder = new ConsumerBuilder<TKey, TValue>(config);

            if (serializer != null)
            {
                consumerBuilder.SetValueDeserializer(new SerializerAdapter<TValue>(serializer));
            }

            consumerBuilder.SetErrorHandler((_, error) => OnError?.Invoke(this, error));

            _consumer = consumerBuilder.Build();
        }

        public void Subscribe(string topic)
        {
            _consumer.Subscribe(topic);
            _logger?.LogInformation("Subscribed to topic: {Topic}", topic);
        }

        public void Subscribe(IEnumerable<string> topics)
        {
            _consumer.Subscribe(topics);
            _logger?.LogInformation("Subscribed to topics: {Topics}", string.Join(", ", topics));
        }

        public void Unsubscribe()
        {
            _consumer.Unsubscribe();
            _logger?.LogInformation("Unsubscribed from all topics");
        }

        public async Task StartConsumingAsync(CancellationToken cancellationToken = default)
        {
            _consumingTask = Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = _consumer.Consume(cancellationToken);

                        if (consumeResult?.Message != null)
                        {
                            OnMessageReceived?.Invoke(this, consumeResult);
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        _logger?.LogError(ex, "Error consuming message: {Error}", ex.Error.Reason);
                        OnError?.Invoke(this, ex.Error);
                    }
                    catch (OperationCanceledException)
                    {
                        _logger?.LogInformation("Consumption operation was cancelled");
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "Unexpected error during consumption");
                        await Task.Delay(1000, cancellationToken); // Prevent tight loop on persistent errors
                    }
                }
            }, cancellationToken);

            await _consumingTask;
        }

        public void Commit(ConsumeResult<TKey, TValue> result)
        {
            _consumer.Commit(result);
        }

        public void Commit(IEnumerable<TopicPartitionOffset> offsets)
        {
            _consumer.Commit(offsets);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _consumer?.Close();
                _consumer?.Dispose();
                _disposed = true;
            }
        }
    }

    public class KafkaConsumer<TValue> : KafkaConsumer<Null, TValue>, IKafkaConsumer<TValue>
    {
        public KafkaConsumer(
            IOptions<KafkaConfig> kafkaConfig,
            IMessageSerializer<TValue>? serializer = null,
            ILogger<KafkaConsumer<Null, TValue>>? logger = null)
            : base(kafkaConfig, serializer, logger)
        {
        }
    }
}
