﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Requests
{
    public class DotNetMetricCreateRequest
    {
        public DateTimeOffset Date { get; set; }
        public int Value { get; set; }
    }

}
