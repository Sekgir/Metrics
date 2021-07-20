using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IList<T> GetByTimePeriodFromAgent(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime);
        IList<T> GetByTimePeriodFromAllCluster(DateTimeOffset fromTime, DateTimeOffset toTime);
        DateTimeOffset GetTimeOfLastRecordByAgent(int agentId);
        void Create(T item);
    }
}
