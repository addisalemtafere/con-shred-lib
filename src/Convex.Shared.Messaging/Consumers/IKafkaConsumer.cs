using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Convex.Shared.Messaging.Consumers
{
    public interface IKafkaConsumer<TKey, TValue> : IDisposable
    {
        event EventHandler<ConsumeResult<TKey, TValue>> OnMessageReceived;
        event EventHandler<Error> OnError;

        void Subscribe(string topic);
        void Subscribe(IEnumerable<string> topics);
        void Unsubscribe();
        void Commit(ConsumeResult<TKey, TValue> result);
        void Commit(IEnumerable<TopicPartitionOffset> offsets);
        Task StartConsumingAsync(CancellationToken cancellationToken = default);
    }

    public interface IKafkaConsumer<TValue> : IKafkaConsumer<Null, TValue>
    {
    }
}
