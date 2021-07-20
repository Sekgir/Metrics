using Dapper;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Repositories
{
    public class RamMetricsRepository : IRamMetricsRepository
    {
        private readonly IConnectionManager _connectionManager;
        public RamMetricsRepository(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }
        public void Create(RamMetric item)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                connection.Execute("INSERT INTO RamMetrics(agentId, value, time) VALUES(@agentId, @value, @time)",
                    new
                    {
                        agentId = item.AgentId,
                        value = item.Value,
                        time = item.Time
                    });
            }
        }

        public IList<RamMetric> GetByTimePeriodFromAgent(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                var returnList = connection.Query<RamMetric>("SELECT * FROM RamMetrics where agentId = @agentId" +
                    " and time > @fromTime and time <= @toTime",
                    new
                    {
                        agentId = agentId,
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    });
                return returnList.ToList();
            }
        }

        public IList<RamMetric> GetByTimePeriodFromAllCluster(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                var returnList = connection.Query<RamMetric>("SELECT * FROM RamMetrics where" +
                    " and time > @fromTime and time <= @toTime",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    });
                return returnList.ToList();
            }
        }

        public DateTimeOffset GetTimeOfLastRecordByAgent(int agentId)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                var returnResult = connection.QuerySingle<long>("SELECT IFNULL(Max(time), 0) FROM RamMetrics where agentId = @agentId",
                    new
                    {
                        agentId = agentId
                    });
                return DateTimeOffset.FromUnixTimeSeconds(returnResult);
            }
        }
    }
}
