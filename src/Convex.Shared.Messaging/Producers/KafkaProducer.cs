using Convex.Shared.Messaging.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Convex.Shared.Messaging.Configuration;
using Convex.Shared.Messaging.Serialization;
using ProducerConfig = Confluent.Kafka.ProducerConfig;

namespace Convex.Shared.Messaging.Producers
{
    public class KafkaProducer<TKey, TValue> : IKafkaProducer<TKey, TValue>
    {
        private readonly IProducer<TKey, TValue> _producer;
        private readonly ILogger<KafkaProducer<TKey, TValue>> _logger;
        private bool _disposed = false;

        public KafkaProducer(
            IOptions<KafkaConfig> kafkaConfig,
            IMessageSerializer<TValue>? serializer = null,
            ILogger<KafkaProducer<TKey, TValue>>? logger = null)
        {
            _logger = logger;

            var config = new ProducerConfig
            {
                BootstrapServers = kafkaConfig.Value.BootstrapServers,
                ClientId = kafkaConfig.Value.Producer.ClientId,
                MessageSendMaxRetries = kafkaConfig.Value.Producer.MessageSendMaxRetries,
                BatchSize = kafkaConfig.Value.Producer.BatchSize,
                LingerMs = kafkaConfig.Value.Producer.LingerMs,
                Acks = (Acks)Enum.Parse(typeof(Acks), kafkaConfig.Value.Producer.Acks),
                RequestTimeoutMs = kafkaConfig.Value.RequestTimeoutMs,
                MessageTimeoutMs = kafkaConfig.Value.MessageTimeoutMs
            };

            var producerBuilder = new ProducerBuilder<TKey, TValue>(config);

            if (serializer != null)
            {
                producerBuilder.SetValueSerializer(new SerializerAdapter<TValue>(serializer));
            }

            _producer = producerBuilder.Build();
        }

        public async Task<DeliveryResult<TKey, TValue>> ProduceAsync(string topic, TValue message, TKey? key = default)
        {
            try
            {
                var deliveryResult = await _producer.ProduceAsync(topic, new Message<TKey, TValue>
                {
                    Key = key,
                    Value = message
                });

                _logger?.LogDebug("Message delivered to {Topic} [{Partition}] at offset {Offset}",
                    deliveryResult.Topic, deliveryResult.Partition, deliveryResult.Offset);

                return deliveryResult;
            }
            catch (ProduceException<TKey, TValue> ex)
            {
                _logger?.LogError(ex, "Failed to deliver message to {Topic}: {Error}", topic, ex.Error.Reason);
                throw;
            }
        }

        public async Task ProduceAsync(string topic, IEnumerable<TValue> messages, TKey? key = default)
        {
            var tasks = messages.Select(message => ProduceAsync(topic, message, key));
            await Task.WhenAll(tasks);
        }

        public void Produce(string topic, TValue message, TKey? key = default, Action<DeliveryReport<TKey, TValue>>? deliveryHandler = null)
        {
            _producer.Produce(topic, new Message<TKey, TValue>
            {
                Key = key,
                Value = message
            }, deliveryHandler);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _producer.Flush(TimeSpan.FromSeconds(10));
                _producer.Dispose();
                _disposed = true;
            }
        }
    }

    public class KafkaProducer<TValue> : KafkaProducer<Null, TValue>, IKafkaProducer<TValue>
    {
        public KafkaProducer(
            IOptions<KafkaConfig> kafkaConfig,
            IMessageSerializer<TValue>? serializer = null,
            ILogger<KafkaProducer<Null, TValue>>? logger = null)
            : base(kafkaConfig, serializer, logger)
        {
        }
    }
}
