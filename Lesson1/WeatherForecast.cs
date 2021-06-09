using System;

namespace Lesson1
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public float TemperatureC { get; set; }

        public float TemperatureF => (float)(32 + (TemperatureC / 0.5556));
    }
}
