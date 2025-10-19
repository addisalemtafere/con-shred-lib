using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Convex.Shared.Messaging.Producers
{
    public interface IKafkaProducer<TKey, TValue> : IDisposable
    {
        Task<DeliveryResult<TKey, TValue>> ProduceAsync(string topic, TValue message, TKey? key = default);
        Task ProduceAsync(string topic, IEnumerable<TValue> messages, TKey? key = default);
        void Produce(string topic, TValue message, TKey? key = default, Action<DeliveryReport<TKey, TValue>>? deliveryHandler = null);
    }

    public interface IKafkaProducer<TValue> : IKafkaProducer<Null, TValue>
    {
    }
}
