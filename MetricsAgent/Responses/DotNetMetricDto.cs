using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Responses
{
    public class DotNetMetricDto
    {
        public DateTimeOffset Date { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
    }
}
