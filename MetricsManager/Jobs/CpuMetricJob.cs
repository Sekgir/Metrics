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
    public class CpuMetricJob : IJob
    {
        private ICpuMetricsRepository _cpuMetricsRepository;
        private IMetricsAgentClient _metricsAgentClient;
        private IMetricsAgentRepository _metricsAgentRepository;
        private IMapper _mapper;

        public CpuMetricJob(IMetricsAgentRepository metricsAgentRepository, ICpuMetricsRepository cpuMetricsrepository, IMetricsAgentClient metricsAgentClient, IMapper mapper)
        {
            _metricsAgentRepository = metricsAgentRepository;
            _cpuMetricsRepository = cpuMetricsrepository;
            _metricsAgentClient = metricsAgentClient;
            _mapper = mapper;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var metricsAgents = _metricsAgentRepository.GetAll();
            foreach (var agentInfo in metricsAgents)
            {
                DateTimeOffset timeOfLastRecord = _cpuMetricsRepository.GetTimeOfLastRecordByAgent(agentInfo.AgentId);
                var request = new GetAllCpuMetricsApiRequest() { ClientBaseAddress = agentInfo.AgentUrl, FromTime = timeOfLastRecord, ToTime = DateTimeOffset.UtcNow };
                var response = _metricsAgentClient.GetAllCpuMetrics(request);
                response.Metrics.ForEach(metric => metric.AgentId = agentInfo.AgentId);
                foreach (var metric in response.Metrics)
                {
                    _cpuMetricsRepository.Create(_mapper.Map<CpuMetricDto, CpuMetric>(metric));
                }
            }
            return Task.CompletedTask;
        }
    }
}
