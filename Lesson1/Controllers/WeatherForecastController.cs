using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ValuesHolder _holder;

        public WeatherForecastController(ValuesHolder holder)
        {
            _holder = holder;
        }

        [HttpPost("add")]
        public void AddWF([FromBody] WeatherForecast wf)
        {
            _holder.Add(wf.Date, wf.TemperatureC);
        }

        [HttpPut("edit")]
        public void EditWF([FromBody] WeatherForecast wf)
        {
            _holder.Edit(wf.Date, wf.TemperatureC);
        }

        [HttpDelete("deleteRange")]
        public void DeleteRange([FromBody] DateRange dateRange)
        {
            _holder.DeleteRange(dateRange.date1, dateRange.date2);
        }

        [HttpGet("getRange")]
        public IEnumerable<WeatherForecast> GetRange([FromQuery] DateTime date1, [FromQuery] DateTime date2)
        {
            return _holder.GetRange(date1.ToUniversalTime(), date2.ToUniversalTime());
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _holder.Values;
        }
    }
}
