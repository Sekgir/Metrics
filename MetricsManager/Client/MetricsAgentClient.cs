using MetricsManager.Requests;
using MetricsManager.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MetricsManager.Client
{
    public class MetricsAgentClient : IMetricsAgentClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MetricsAgentClient> _logger;

        public MetricsAgentClient(HttpClient httpClient, ILogger<MetricsAgentClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public AllCpuMetricsApiResponse GetAllCpuMetrics(GetAllCpuMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.ToString("s");
            var toParameter = request.ToTime.ToString("s");
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metrics/cpu/from/{fromParameter}/to/{toParameter}");
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                return JsonSerializer.DeserializeAsync<AllCpuMetricsApiResponse>(responseStream, jsonOptions).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public AllDotNetMetricsApiResponse GetAllDotNetMetrics(GetAllDotNetMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.ToString("s");
            var toParameter = request.ToTime.ToString("s");
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metrics/dotnet/from/{fromParameter}/to/{toParameter}");
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                return JsonSerializer.DeserializeAsync<AllDotNetMetricsApiResponse>(responseStream, jsonOptions).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public AllHddMetricsApiResponse GetAllHddMetrics(GetAllHddMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.ToString("s");
            var toParameter = request.ToTime.ToString("s");
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metrics/hdd/from/{fromParameter}/to/{toParameter}");
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                return JsonSerializer.DeserializeAsync<AllHddMetricsApiResponse>(responseStream, jsonOptions).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public AllNetworkMetricsApiResponse GetAllNetworkMetrics(GetAllNetworkMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.ToString("s");
            var toParameter = request.ToTime.ToString("s");
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metrics/network/from/{fromParameter}/to/{toParameter}");
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                return JsonSerializer.DeserializeAsync<AllNetworkMetricsApiResponse>(responseStream, jsonOptions).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public AllRamMetricsApiResponse GetAllRamMetrics(GetAllRamMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.ToString("s");
            var toParameter = request.ToTime.ToString("s");
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metrics/ram/from/{fromParameter}/to/{toParameter}");
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                return JsonSerializer.DeserializeAsync<AllRamMetricsApiResponse>(responseStream, jsonOptions).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
    }
}