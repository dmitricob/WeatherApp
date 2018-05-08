using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    public class Condition
    {
        public Condition(string text, string icon, int code)
        {
            this.text = text;
            this.icon = icon;
            this.code = code;
        }

        public string text { get; set; }
        public string day { get; set; }
        public string night { get; set; }
        public string icon { get; set; }
        public int code { get; set; }

        public override string ToString()
        {
            return this.night;
        }
    }
}
