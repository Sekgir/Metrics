using AutoMapper;
using MetricsAgent.Controllers;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace MetricsAgentTests.Controllers
{
    public class RamMetricsControllerUnitTests
    {
        private RamMetricsController controller;
        private Mock<IRamMetricsRepository> mockRepository;
        private Mock<ILogger<RamMetricsController>> mockLogger;
        private Mock<IMapper> mockMapper;

        public RamMetricsControllerUnitTests()
        {
            mockRepository = new Mock<IRamMetricsRepository>();
            mockLogger = new Mock<ILogger<RamMetricsController>>();
            mockMapper = new Mock<IMapper>();
            controller = new RamMetricsController(mockLogger.Object, mockRepository.Object, mockMapper.Object);
        }

        [Fact]
        public void GiveMetricsToManager_ShouldCall_GetByTimePeriod_From_Repository()
        {
            mockRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).Verifiable();

            var result = controller.GiveMetricsToManager(DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now);

            mockRepository.Verify(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()), Times.AtMostOnce());

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            mockRepository.Setup(repository => repository.Create(It.IsAny<RamMetric>())).Verifiable();

            var result = controller.Create(new MetricsAgent.Requests.RamMetricCreateRequest { Date = DateTimeOffset.Now, Value = 50 });

            mockRepository.Verify(repository => repository.Create(It.IsAny<RamMetric>()), Times.AtMostOnce());

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
