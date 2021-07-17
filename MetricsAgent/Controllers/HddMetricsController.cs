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
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private readonly ILogger<HddMetricsController> _logger;
        private readonly IHddMetricsRepository _repository;
        private readonly IMapper _mapper;

        public HddMetricsController(ILogger<HddMetricsController> logger, IHddMetricsRepository repository, IMapper mapper)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в HddMetricsController");
            _repository = repository;
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получает метрики HDD на заданном диапазоне времени
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET api/metrics/hdd/from/2021-07-15T00:00:00Z/to/2021-07-17T23:59:59Z
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
            var response = new AllHddMetricsResponse()
            {
                Metrics = _mapper.Map<IList<HddMetric>, List<HddMetricDto>>(metrics)
            };
            return Ok(response);
        }

        /// <summary>
        /// Создание записи HDD метрики с указанными значениями
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST api/metrics/hdd/create
        ///
        /// </remarks>
        /// <returns>Success</returns>
        [HttpPost("create")]
        public IActionResult Create([FromBody] HddMetricCreateRequest request)
        {
            _logger.LogInformation($"Выполнение метода Create(HddMetricCreateRequest:{JsonSerializer.Serialize(request)})");
            _repository.Create(_mapper.Map<HddMetricCreateRequest, HddMetric>(request));
            return Ok();
        }
    }
}
