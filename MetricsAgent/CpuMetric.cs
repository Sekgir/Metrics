﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent
{
    public class CpuMetric
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
