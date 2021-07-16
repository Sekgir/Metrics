using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class NetworkMetricJob : IJob
    {
        private INetworkMetricsRepository _repository;
        private PerformanceCounter _networkCounter;

        public NetworkMetricJob(INetworkMetricsRepository repository)
        {
            _repository = repository;
            _networkCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", new PerformanceCounterCategory("Network Interface").GetInstanceNames()[0]);
        }

        public Task Execute(IJobExecutionContext context)
        {
            var netWorkBytesSent = Convert.ToInt32(_networkCounter.NextValue());
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _repository.Create(new NetworkMetric { Time = time, Value = netWorkBytesSent });
            return Task.CompletedTask;
        }
    }
}
