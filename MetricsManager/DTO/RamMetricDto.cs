using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DTO
{
    public class RamMetricDto
    {
        public int AgentId { get; set; }
        public DateTimeOffset Date { get; set; }
        public int Value { get; set; }
    }
}
