using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.src;

namespace WeatherApp
{
    public class ForecastDay
    {
        public string date { get; set; }

        public string dateFormated;

        public string makeFormatedDate()
        {
            return this.dateFormated = DateTime.ParseExact(this.date, "yyyy-MM-dd", null).ToString("ddd d", new CultureInfo("en-US"));
        }

        public Day day { get; set; }

        public Astro astro { get; set; }
    }
}
