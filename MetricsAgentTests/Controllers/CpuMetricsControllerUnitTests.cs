using MetricsAgent;
using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests.Controllers
{
    public class CpuMetricsControllerUnitTests
    {
        private CpuMetricsController controller;

        public CpuMetricsControllerUnitTests()
        {
            controller = new CpuMetricsController();
        }

        [Fact]
        public void GiveMetricsToManager_ReturnsOk()
        {
            var fromTime = DateTimeOffset.UtcNow.AddDays(-1);
            var toTime = DateTimeOffset.UtcNow;

            var result = controller.GiveMetricsToManager(fromTime, toTime);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
