using MetricsManager.Requests;
using MetricsManager.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Client
{
    public interface IMetricsAgentClient
    {
        AllCpuMetricsApiResponse GetAllCpuMetrics(GetAllCpuMetricsApiRequest request);
        AllDotNetMetricsApiResponse GetAllDotNetMetrics(GetAllDotNetMetricsApiRequest request);
        AllHddMetricsApiResponse GetAllHddMetrics(GetAllHddMetricsApiRequest request);
        AllNetworkMetricsApiResponse GetAllNetworkMetrics(GetAllNetworkMetricsApiRequest request);
        AllRamMetricsApiResponse GetAllRamMetrics(GetAllRamMetricsApiRequest request);
    }
}

