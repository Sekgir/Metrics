﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Responses
{
    public class NetworkMetricDto
    {
        public DateTimeOffset Date { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
    }
}
