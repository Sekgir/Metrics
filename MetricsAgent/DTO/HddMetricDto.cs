using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.DTO
{
    public class HddMetricDto
    {
        public DateTimeOffset Date { get; set; }
        public int Value { get; set; }
    }
}
