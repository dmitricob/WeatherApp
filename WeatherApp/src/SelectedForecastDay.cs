using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.src
{
    public class SelectedForecastDay
    {
        public SelectedForecastDay(string date, string maxtemp, string mintemp, string avgtemp, string maxwind, string totalprecip, string avgvis, string avghumidity, string conditionText, string conditionicon, string conditioncode, string moonset, string moonrise, string sunrise,string sunset)
        {
            this.date = date;
            this.maxtemp = maxtemp;
            this.mintemp = mintemp;
            this.avgtemp = avgtemp;
            this.maxwind = maxwind;
            this.totalprecip = totalprecip;
            this.avgvis = avgvis;
            this.avghumidity = avghumidity;
            this.conditionText = conditionText;
            this.conditionicon = conditionicon;
            this.conditioncode = conditioncode;
            this.sunrise = sunrise;
            this.sunset = sunset;
            this.moonrise = moonrise;
            this.moonset = moonset;
        }

        public string date { get; set; }
        public string maxtemp { get; set; }
        public string mintemp { get; set; }
        public string avgtemp { get; set; }
        public string maxwind { get; set; }
        public string totalprecip { get; set; }
        public string avgvis { get; set; }
        public string avghumidity { get; set; }
        public string conditionText { get; set; }
        public string conditionicon { get; set; }
        public string conditioncode { get; set; }
        public string sunrise{ get; set; }
        public string sunset{ get; set; }
        public string moonset{ get; set; }
        public string moonrise { get; set; }

    }
}
