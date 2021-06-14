﻿using MetricsAgent;
using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests.Controllers
{
    public class HddMetricsControllerUnitTests
    {
        private HddMetricsController controller;

        public HddMetricsControllerUnitTests()
        {
            controller = new HddMetricsController();
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