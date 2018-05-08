using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    public class Day
    {
        public int maxtemp_c { get; set; }
        public int maxtemp_f { get; set; }
        public int mintemp_c { get; set; }
        public int mintemp_f { get; set; }
        public int avgtemp_c { get; set; }
        public int avgtemp_f { get; set; }
        public int maxwind_mph { get; set; }
        public int maxwind_kph { get; set; }
        public int totalprecip_mm { get; set; }
        public int totalprecip_in { get; set; }
        public int avgvis_km { get; set; }
        public int avgvis_miles { get; set; }
        public int avghumidity { get; set; }
        public Condition condition { get; set; }

    }
}
