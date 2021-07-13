using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.DTO
{
    public class RamMetricDto
    {
        public DateTimeOffset Date { get; set; }
        public int Value { get; set; }
    }
}
