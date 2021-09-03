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

        /// <summary>
        /// Получение метрик DotNet на заданном диапазоне времени по определенному агенту
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET api/metrics/dotnet/agent/1/from/2021-07-15T00:00:00Z/to/2021-07-17T23:59:59Z
        ///
        /// </remarks>
        /// <param name="agentId">ID агентами</param>
        /// <param name="fromTime">начальная метка времени</param>
        /// <param name="toTime">конечная метка времени</param>
        /// <returns>Список метрик, которые были сохранены в заданном диапазоне времени по указанному агенту</returns>
        /// <returns>Success</returns>
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

        /// <summary>
        /// Получение метрик DotNet на заданном диапазоне времени по всем агентам
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET api/metrics/dotnet/from/2021-07-15T00:00:00Z/to/2021-07-17T23:59:59Z
        ///
        /// </remarks>
        /// <param name="fromTime">начальная метка времени</param>
        /// <param name="toTime">конечная метка времени</param>
        /// <returns>Список метрик, которые были сохранены в заданном диапазоне времени по всем агентам</returns>
        /// <returns>Success</returns>
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
