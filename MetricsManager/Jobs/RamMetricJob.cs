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
    public class RamMetricJob : IJob
    {
        private IRamMetricsRepository _ramMetricsRepository;
        private IMetricsAgentClient _metricsAgentClient;
        private IMetricsAgentRepository _metricsAgentRepository;
        private IMapper _mapper;

        public RamMetricJob(IMetricsAgentRepository metricsAgentRepository, IRamMetricsRepository ramMetricsrepository, IMetricsAgentClient metricsAgentClient, IMapper mapper)
        {
            _metricsAgentRepository = metricsAgentRepository;
            _ramMetricsRepository = ramMetricsrepository;
            _metricsAgentClient = metricsAgentClient;
            _mapper = mapper;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var metricsAgents = _metricsAgentRepository.GetAll();
            foreach (var agentInfo in metricsAgents)
            {
                DateTimeOffset timeOfLastRecord = _ramMetricsRepository.GetTimeOfLastRecordByAgent(agentInfo.AgentId);
                var request = new GetAllRamMetricsApiRequest() { ClientBaseAddress = agentInfo.AgentUrl, FromTime = timeOfLastRecord, ToTime = DateTimeOffset.UtcNow };
                var response = _metricsAgentClient.GetAllRamMetrics(request);
                response.Metrics.ForEach(metric => metric.AgentId = agentInfo.AgentId);
                foreach (var metric in response.Metrics)
                {
                    _ramMetricsRepository.Create(_mapper.Map<RamMetricDto, RamMetric>(metric));
                }
            }
            return Task.CompletedTask;
        }
    }
}
