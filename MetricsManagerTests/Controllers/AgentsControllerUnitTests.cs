using MetricsManager;
using MetricsManager.Controllers;
using MetricsManager.DAL.Models;
using MetricsManager.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;
using System.Collections.Generic;

namespace MetricsManagerTests.Controllers
{
    public class AgentsControllerUnitTests
    {
        private AgentsController controller;
        private Mock<ILogger<AgentsController>> mockLogger;
        private Mock<IMetricsAgentRepository> mockRepository;

        public AgentsControllerUnitTests()
        {
            mockLogger = new Mock<ILogger<AgentsController>>();
            mockRepository = new Mock<IMetricsAgentRepository>();
            controller = new AgentsController(mockLogger.Object, mockRepository.Object);
        }

        [Fact]
        public void RegisterAgent_ReturnsOk()
        {
            var agentInfo = new AgentInfo() { AgentId = 1, AgentUrl = @"https://localhost:5001" };

            var result = controller.RegisterAgent(agentInfo);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void EnableAgentById_ReturnsOk()
        {
            var agentId = 1;

            var result = controller.EnableAgentById(agentId);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void DisableAgentById_ReturnsOk()
        {
            var agentId = 1;

            var result = controller.DisableAgentById(agentId);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void RegisteredAgents_ReturnsOk()
        {
            mockRepository.Setup(repo => repo.GetAll()).Returns(new List<AgentInfo>());
            var result = controller.RegisteredAgents();

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
