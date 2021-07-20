using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Responses
{
    public class AllMetricsAgentsApiResponse
    {
        public List<AgentInfo> Agents { get; set; }
    }
}
