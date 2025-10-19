using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convex.Shared.Messaging.Configuration
{
    public class ConsumerConfig
    {
        public string ClientId { get; set; } = "convex-consumer";
        public int MaxPollIntervalMs { get; set; } = 300000;
        public int FetchMaxBytes { get; set; } = 52428800;
        public int FetchWaitMaxMs { get; set; } = 500;
        public int MaxPartitionFetchBytes { get; set; } = 1048576;
    }
}
