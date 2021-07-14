using MetricsManager.DAL.Models;
using MetricsManager.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly ILogger<AgentsController> _logger;
        private readonly IMetricsAgentRepository _metricsAgentRepository;

        public AgentsController(ILogger<AgentsController> logger, IMetricsAgentRepository metricsAgentRepository)
        {
            _logger = logger;
            _metricsAgentRepository = metricsAgentRepository;
            _logger.LogDebug(1, "NLog встроен в AgentsController");
        }

        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            _logger.LogInformation($"Выполнение метода RegisterAgent(AgentInfo {JsonSerializer.Serialize(agentInfo)})");
            _metricsAgentRepository.Create(agentInfo);
            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation($"Выполнение метода EnableAgentById(agentId = {agentId})");
            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation($"Выполнение метода DisableAgentById(agentId = {agentId})");
            return Ok();
        }

        [HttpGet("registered")]
        public IActionResult RegisteredAgents()
        {
            _logger.LogInformation($"Выполнение метода RegisteredAgents()");
            return Ok();
        }
    }
}
