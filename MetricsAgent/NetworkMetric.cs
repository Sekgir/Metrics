using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent
{
    public class NetworkMetric
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
