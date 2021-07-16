using AutoMapper;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using MetricsManager.DTO;
using MetricsManager.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ICpuMetricsRepository _repository;
        private readonly ILogger<CpuMetricsController> _logger;
        private readonly IMapper _mapper;

        public CpuMetricsController(ILogger<CpuMetricsController> logger, ICpuMetricsRepository repository, IMapper mapper)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в CpuMetricsController");
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"Выполнение метода GetMetricsFromAgent(agentId = {agentId}, fromTime = {fromTime}, toTime = {toTime})");
            var metrics = _repository.GetByTimePeriodFromAgent(agentId, fromTime, toTime);
            var response = new AllCpuMetricsApiResponse()
            {
                Metrics = _mapper.Map<IList<CpuMetric>, List<CpuMetricDto>>(metrics)
            };
            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"Выполнение метода GetMetricsFromAllCluster(fromTime = {fromTime}, toTime = {toTime})");
            var metrics = _repository.GetByTimePeriodFromAllCluster(fromTime, toTime);
            var response = new AllCpuMetricsApiResponse()
            {
                Metrics = _mapper.Map<IList<CpuMetric>, List<CpuMetricDto>>(metrics)
            };
            return Ok(response);
        }
    }
}
