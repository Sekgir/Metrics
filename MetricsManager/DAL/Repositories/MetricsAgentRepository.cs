using Dapper;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Repositories
{
    public class MetricsAgentRepository : IMetricsAgentRepository
    {
        private readonly IConnectionManager _connectionManager;
        public MetricsAgentRepository(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }
        public void Create(AgentInfo agentInfo)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                connection.Execute("INSERT INTO Agents(agentUrl) VALUES(@agentUrl)",
                    new
                    {
                        agentUrl = agentInfo.AgentUrl
                    });
            }
        }

        public IList<AgentInfo> GetAll()
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                var returnList = connection.Query<AgentInfo>("SELECT * FROM Agents");
                return returnList.ToList();
            };
        }
    }
}
