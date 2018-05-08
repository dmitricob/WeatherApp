using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.src
{
    class settingsParametrs
    {
        public settingsParametrs(string tempType, string windType, string presType, string precipType, string visType, int days, string cityName)
        {
            this.tempType = tempType;
            this.windType = windType;
            this.presType = presType;
            this.precipType = precipType;
            this.visType = visType;
            this.days = days;
            this.cityName = cityName;
        }

        public string tempType { get; set; }
        public string windType { get; set; }
        public string presType { get; set; }
        public string precipType { get; set; }
        public string visType { get; set; }
        public int days { get; set; }
        public string cityName { get; set; }
    }
}
