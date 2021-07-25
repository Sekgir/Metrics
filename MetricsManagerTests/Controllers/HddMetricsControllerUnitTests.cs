using AutoMapper;
using MetricsManager;
using MetricsManager.Controllers;
using MetricsManager.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace MetricsManagerTests.Controllers
{
    public class HddMetricsControllerUnitTests
    {
        private HddMetricsController controller;
        private Mock<ILogger<HddMetricsController>> mockLogger;
        private Mock<IHddMetricsRepository> mockRepository;
        private Mock<IMapper> mockMapper;

        public HddMetricsControllerUnitTests()
        {
            mockLogger = new Mock<ILogger<HddMetricsController>>();
            mockRepository = new Mock<IHddMetricsRepository>();
            mockMapper = new Mock<IMapper>();
            controller = new HddMetricsController(mockLogger.Object, mockRepository.Object, mockMapper.Object);
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            var agentId = 1;
            var fromTime = DateTimeOffset.UtcNow.AddDays(-1);
            var toTime = DateTimeOffset.UtcNow;

            var result = controller.GetMetricsFromAgent(agentId, fromTime, toTime);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void GetMetricsFromAllCluster_ReturnsOk()
        {
            var fromTime = DateTimeOffset.UtcNow.AddDays(-1);
            var toTime = DateTimeOffset.UtcNow;

            var result = controller.GetMetricsFromAllCluster(fromTime, toTime);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
