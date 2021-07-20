using Dapper;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Repositories
{
    public class HddMetricsRepository : IHddMetricsRepository
    {
        private readonly IConnectionManager _connectionManager;
        public HddMetricsRepository(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }
        public void Create(HddMetric item)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                connection.Execute("INSERT INTO HddMetrics(agentId, value, time) VALUES(@agentId, @value, @time)",
                    new
                    {
                        agentId = item.AgentId,
                        value = item.Value,
                        time = item.Time
                    });
            }
        }

        public IList<HddMetric> GetByTimePeriodFromAgent(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                var returnList = connection.Query<HddMetric>("SELECT * FROM HddMetrics where agentId = @agentId" +
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

        public IList<HddMetric> GetByTimePeriodFromAllCluster(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                var returnList = connection.Query<HddMetric>("SELECT * FROM HddMetrics where" +
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
                var returnResult = connection.QuerySingle<long>("SELECT IFNULL(Max(time), 0) FROM HddMetrics where agentId = @agentId",
                    new
                    {
                        agentId = agentId
                    });
                return DateTimeOffset.FromUnixTimeSeconds(returnResult);
            }
        }
    }
}