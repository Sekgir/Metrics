using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Interfaces
{
    public interface IMetricsAgentRepository
    {
        void Create(AgentInfo agentInfo);
        IList<AgentInfo> GetAll();
    }
}
