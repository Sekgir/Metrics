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
    [Route("api/metrics/dotnet")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        private readonly IDotNetMetricsRepository _repository;
        private readonly ILogger<DotNetMetricsController> _logger;
        private readonly IMapper _mapper;

        public DotNetMetricsController(ILogger<DotNetMetricsController> logger, IDotNetMetricsRepository repository, IMapper mapper)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в DotNetMetricsController");
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"Выполнение метода GetMetricsFromAgent(agentId = {agentId}, fromTime = {fromTime}, toTime = {toTime})");
            var metrics = _repository.GetByTimePeriodFromAgent(agentId, fromTime, toTime);
            var response = new AllDotNetMetricsApiResponse()
            {
                Metrics = _mapper.Map<IList<DotNetMetric>, List<DotNetMetricDto>>(metrics)
            };
            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"Выполнение метода GetMetricsFromAllCluster(fromTime = {fromTime}, toTime = {toTime})");
            var metrics = _repository.GetByTimePeriodFromAllCluster(fromTime, toTime);
            var response = new AllDotNetMetricsApiResponse()
            {
                Metrics = _mapper.Map<IList<DotNetMetric>, List<DotNetMetricDto>>(metrics)
            };
            return Ok(response);
        }
    }
}
