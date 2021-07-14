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
    public class DotNetMetricJob : IJob
    {
        private IDotNetMetricsRepository _dotNetMetricsRepository;
        private IMetricsAgentClient _metricsAgentClient;
        private IMetricsAgentRepository _metricsAgentRepository;
        private IMapper _mapper;

        public DotNetMetricJob(IMetricsAgentRepository metricsAgentRepository, IDotNetMetricsRepository dotNetMetricsRepository, IMetricsAgentClient metricsAgentClient, IMapper mapper)
        {
            _metricsAgentRepository = metricsAgentRepository;
            _dotNetMetricsRepository = dotNetMetricsRepository;
            _metricsAgentClient = metricsAgentClient;
            _mapper = mapper;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var metricsAgents = _metricsAgentRepository.GetAll();
            foreach (var agentInfo in metricsAgents)
            {
                DateTimeOffset timeOfLastRecord = _dotNetMetricsRepository.GetTimeOfLastRecordByAgent(agentInfo.AgentId);
                var request = new GetAllDotNetMetricsApiRequest() { ClientBaseAddress = agentInfo.AgentUrl, FromTime = timeOfLastRecord, ToTime = DateTimeOffset.UtcNow };
                var response = _metricsAgentClient.GetAllDotNetMetrics(request);
                response.Metrics.ForEach(metric => metric.AgentId = agentInfo.AgentId);
                foreach (var metric in response.Metrics)
                {
                    _dotNetMetricsRepository.Create(_mapper.Map<DotNetMetricDto, DotNetMetric>(metric));
                }
            }
            return Task.CompletedTask;
        }
    }
}
