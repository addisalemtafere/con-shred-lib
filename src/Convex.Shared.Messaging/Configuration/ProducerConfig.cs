using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convex.Shared.Messaging.Configuration
{
    public class ProducerConfig
    {
        public string ClientId { get; set; } = "convex-producer";
        public int MessageSendMaxRetries { get; set; } = 3;
        public int BatchSize { get; set; } = 16384;
        public int LingerMs { get; set; } = 5;
        public string Acks { get; set; } = "All";
    }
}
