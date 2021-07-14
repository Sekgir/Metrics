using Dapper;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Repositories
{
    public class NetworkMetricsRepository : INetworkMetricsRepository
    {
        private readonly IConnectionManager _connectionManager;
        public NetworkMetricsRepository(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }
        public void Create(NetworkMetric item)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                connection.Execute("INSERT INTO NetworkMetrics(agentId, value, time) VALUES(@agentId, @value, @time)",
                    new
                    {
                        agentId = item.AgentId,
                        value = item.Value,
                        time = item.Time
                    });
            }
        }

        public IList<NetworkMetric> GetByTimePeriodFromAgent(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                var returnList = connection.Query<NetworkMetric>("SELECT * FROM NetworkMetrics where agentId = @agentId" +
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

        public IList<NetworkMetric> GetByTimePeriodFromAllCluster(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                var returnList = connection.Query<NetworkMetric>("SELECT * FROM NetworkMetrics where" +
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
                var returnResult = connection.QuerySingle<long>("SELECT IFNULL(Max(time), 0) FROM NetworkMetrics where agentId = @agentId",
                    new
                    {
                        agentId = agentId
                    });
                return DateTimeOffset.FromUnixTimeSeconds(returnResult);
            }
        }
    }
}
