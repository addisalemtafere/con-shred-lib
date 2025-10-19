using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convex.Shared.Messaging.Serialization
{
    public interface IMessageSerializer<T>
    {
        byte[] Serialize(T data);
        T Deserialize(byte[] data);
    }

    // Adapter for Confluent.Kafka serialization
    internal class SerializerAdapter<T> : Confluent.Kafka.ISerializer<T>, Confluent.Kafka.IDeserializer<T>
    {
        private readonly IMessageSerializer<T> _serializer;

        public SerializerAdapter(IMessageSerializer<T> serializer)
        {
            _serializer = serializer;
        }

        public byte[] Serialize(T data, SerializationContext context)
        {
            return _serializer.Serialize(data);
        }

        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return _serializer.Deserialize(data.ToArray());
        }

      
    }
}
