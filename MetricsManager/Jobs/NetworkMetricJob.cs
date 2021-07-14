using AutoMapper;
using MetricsManager.Client;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using MetricsManager.DTO;
using MetricsManager.Requests;
using MetricsManager.Responses;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Jobs
{
    public class NetworkMetricJob : IJob
    {
        private INetworkMetricsRepository _networkMetricsRepository;
        private IMetricsAgentClient _metricsAgentClient;
        private IMetricsAgentRepository _metricsAgentRepository;
        private IMapper _mapper;

        public NetworkMetricJob(IMetricsAgentRepository metricsAgentRepository, INetworkMetricsRepository networkMetricsrepository, IMetricsAgentClient metricsAgentClient, IMapper mapper)
        {
            _metricsAgentRepository = metricsAgentRepository;
            _networkMetricsRepository = networkMetricsrepository;
            _metricsAgentClient = metricsAgentClient;
            _mapper = mapper;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var metricsAgents = _metricsAgentRepository.GetAll();
            foreach (var agentInfo in metricsAgents)
            {
                DateTimeOffset timeOfLastRecord = _networkMetricsRepository.GetTimeOfLastRecordByAgent(agentInfo.AgentId);
                var request = new GetAllNetworkMetricsApiRequest() { ClientBaseAddress = agentInfo.AgentUrl, FromTime = timeOfLastRecord, ToTime = DateTimeOffset.UtcNow };
                var response = _metricsAgentClient.GetAllNetworkMetrics(request);
                response.Metrics.ForEach(metric => metric.AgentId = agentInfo.AgentId);
                foreach (var metric in response.Metrics)
                {
                    _networkMetricsRepository.Create(_mapper.Map<NetworkMetricDto, NetworkMetric>(metric));
                }
            }
            return Task.CompletedTask;
        }
    }
}
