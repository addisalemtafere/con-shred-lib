using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convex.Shared.Messaging.Configuration
{
    public class KafkaConfig
    {
        public string BootstrapServers { get; set; } = string.Empty;
        public string GroupId { get; set; } = "convex-app";
        public bool EnableAutoCommit { get; set; } = false;
        public string AutoOffsetReset { get; set; } = "Earliest";
        public int MessageTimeoutMs { get; set; } = 5000;
        public int RequestTimeoutMs { get; set; } = 3000;
        public int SessionTimeoutMs { get; set; } = 10000;
        public ProducerConfig Producer { get; set; } = new();
        public ConsumerConfig Consumer { get; set; } = new();
    }
}
