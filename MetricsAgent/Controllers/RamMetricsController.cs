using AutoMapper;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using MetricsAgent.DTO;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly IRamMetricsRepository _repository;
        private readonly ILogger<RamMetricsController> _logger;
        private readonly IMapper _mapper;

        public RamMetricsController(ILogger<RamMetricsController> logger, IRamMetricsRepository repository, IMapper mapper)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в RamMetricsController");
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получает метрики RAM на заданном диапазоне времени
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET api/metrics/ram/from/2021-07-15T00:00:00Z/to/2021-07-17T23:59:59Z
        ///
        /// </remarks>
        /// <param name="fromTime">начальная метка времени</param>
        /// <param name="toTime">конечная метка времени</param>
        /// <returns>Список метрик, которые были сохранены в заданном диапазоне времени</returns>
        /// <returns>Success</returns>
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GiveMetricsToManager([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"Выполнение метода GiveMetricsToManager(fromTime = {fromTime}, toTime = {toTime})");
            var metrics = _repository.GetByTimePeriod(fromTime, toTime);
            var response = new AllRamMetricsResponse()
            {
                Metrics = _mapper.Map<IList<RamMetric>, List<RamMetricDto>>(metrics)
            };
            return Ok(response);
        }

        /// <summary>
        /// Создание записи RAM метрики с указанными значениями
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST api/metrics/ram/create
        ///
        /// </remarks>
        /// <returns>Success</returns>
        [HttpPost("create")]
        public IActionResult Create([FromBody] RamMetricCreateRequest request)
        {
            _logger.LogInformation($"Выполнение метода Create(RamMetricCreateRequest:{JsonSerializer.Serialize(request)})");
            _repository.Create(_mapper.Map<RamMetricCreateRequest, RamMetric>(request));
            return Ok();
        }
    }
}
