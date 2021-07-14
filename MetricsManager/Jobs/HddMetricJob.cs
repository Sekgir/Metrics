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
    public class HddMetricJob : IJob
    {
        private IHddMetricsRepository _hddMetricsRepository;
        private IMetricsAgentClient _metricsAgentClient;
        private IMetricsAgentRepository _metricsAgentRepository;
        private IMapper _mapper;

        public HddMetricJob(IMetricsAgentRepository metricsAgentRepository, IHddMetricsRepository hddMetricsrepository, IMetricsAgentClient metricsAgentClient, IMapper mapper)
        {
            _metricsAgentRepository = metricsAgentRepository;
            _hddMetricsRepository = hddMetricsrepository;
            _metricsAgentClient = metricsAgentClient;
            _mapper = mapper;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var metricsAgents = _metricsAgentRepository.GetAll();
            foreach (var agentInfo in metricsAgents)
            {
                DateTimeOffset timeOfLastRecord = _hddMetricsRepository.GetTimeOfLastRecordByAgent(agentInfo.AgentId);
                var request = new GetAllHddMetricsApiRequest() { ClientBaseAddress = agentInfo.AgentUrl, FromTime = timeOfLastRecord, ToTime = DateTimeOffset.UtcNow };
                var response = _metricsAgentClient.GetAllHddMetrics(request);
                response.Metrics.ForEach(metric => metric.AgentId = agentInfo.AgentId);
                foreach (var metric in response.Metrics)
                {
                    _hddMetricsRepository.Create(_mapper.Map<HddMetricDto, HddMetric>(metric));
                }
            }
            return Task.CompletedTask;
        }
    }
}
