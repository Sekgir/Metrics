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
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private readonly IHddMetricsRepository _repository;
        private readonly ILogger<HddMetricsController> _logger;
        private readonly IMapper _mapper;

        public HddMetricsController(ILogger<HddMetricsController> logger, IHddMetricsRepository repository, IMapper mapper)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в HddMetricsController");
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"Выполнение метода GetMetricsFromAgent(agentId = {agentId}, fromTime = {fromTime}, toTime = {toTime})");
            var metrics = _repository.GetByTimePeriodFromAgent(agentId, fromTime, toTime);
            var response = new AllHddMetricsApiResponse()
            {
                Metrics = _mapper.Map<IList<HddMetric>, List<HddMetricDto>>(metrics)
            };
            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"Выполнение метода GetMetricsFromAllCluster(fromTime = {fromTime}, toTime = {toTime})");
            var metrics = _repository.GetByTimePeriodFromAllCluster(fromTime, toTime);
            var response = new AllHddMetricsApiResponse()
            {
                Metrics = _mapper.Map<IList<HddMetric>, List<HddMetricDto>>(metrics)
            };
            return Ok(response);
        }
    }
}
