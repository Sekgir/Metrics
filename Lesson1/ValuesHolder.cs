using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lesson1
{
    public class ValuesHolder
    {
        public List<WeatherForecast> Values = new List<WeatherForecast>();
        
        public void Add(DateTime time, float temp)
        {
            if (Values.All(w => w.Date != time))
            {
                Values.Add(new WeatherForecast() { Date = time, TemperatureC = temp });
            }
        }

        public void Edit(DateTime time, float temp)
        {
            WeatherForecast wf = Values.Find(element => element.Date == time);
            if (wf != null)
            {
                wf.TemperatureC = temp;
            }
        }

        public void DeleteRange(DateTime time1, DateTime time2)
        {
            if (time1 > time2)
            {
                DateTime temp = time2;
                time2 = time1;
                time1 = temp;
            }
            Values = Values.Where(w => w.Date < time1 || w.Date > time2 ).ToList();
        }

        public List<WeatherForecast> GetRange(DateTime time1, DateTime time2)
        {
            if (time1 > time2)
            {
                DateTime temp = time2;
                time2 = time1;
                time1 = temp;
            }
            return Values.Where(w => w.Date >= time1 && w.Date <= time2).ToList();
        }
    }
}
