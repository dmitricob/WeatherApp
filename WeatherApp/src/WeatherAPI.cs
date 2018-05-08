using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    class WeatherAPI
    {
        private int days ;
        private string cityName;
        private string url ;
        private string weatherRespondBody;
        private int cityCode;

        public WeatherAPI()
        {
            cityCode = 703448;
            days = 5;
            cityName = "kiev";
            setUrl();
        }

        public WeatherAPI(string cityName, int days)
        {
            setUrl(cityName,days);
        }

        public WeatherAPI(string cityName)
        {
            this.cityName = cityName;
            this.chekCity();
        }

        public void setUrl(string cityName, int days)
        {
            this.cityName = cityName;
            this.days = days;
            this.setUrl();
        }

        public void setUrl()
        {
            url = "http://api.apixu.com/v1/forecast.json?key=6805f6fedc944e37a01190408182004&q=" + cityName + "&days=" + days;

            //http://api.openweathermap.org/data/2.5/forecast/daily?id=524901&APPID=59879fbfd40eab89d94e6e0ccc4bfaab
            //string url = "http://api.openweathermap.org/data/2.5/forecast/daily?q=London&mode=xml&units=metric&cnt=7&appid=59879fbfd40eab89d94e6e0ccc4bfaab";
            //http://api.apixu.com/v1/current.json?key=6805f6fedc944e37a01190408182004&q=kiev&days=10
            //string url = "http://api.apixu.com/v1/forecast.json?key=6805f6fedc944e37a01190408182004&q="+cityName+"&days="+days;
        }

        public void chekCity()
        {
            this.chekCity(this.cityName);
        }

        public void chekCity(string cityName)
        {
            url = "http://api.apixu.com/v1/search.json?key=6805f6fedc944e37a01190408182004&q="+cityName;
        }

        public string getRespond()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponse = new HttpResponseMessage();

            try
            {
                Uri uri = new Uri(this.url);
                httpResponse = httpClient.GetAsync(uri).GetAwaiter().GetResult();
                httpResponse.EnsureSuccessStatusCode();
                this.weatherRespondBody = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                Debug.WriteLine("Respond: " + weatherRespondBody + " .");
                return this.weatherRespondBody;
            }
            catch (Exception ex)
            {
                this.weatherRespondBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                Debug.WriteLine(this.weatherRespondBody);
                return this.weatherRespondBody;
            }
        }

        public string getConditions()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponse = new HttpResponseMessage();

            try
            {
                Uri uri = new Uri("http://www.apixu.com/doc/Apixu_weather_conditions.json");
                httpResponse = httpClient.GetAsync(uri).GetAwaiter().GetResult();
                httpResponse.EnsureSuccessStatusCode();
                this.weatherRespondBody = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                Debug.WriteLine("Respond: " + weatherRespondBody + " .");
                return this.weatherRespondBody;
            }
            catch (Exception ex)
            {
                this.weatherRespondBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                Debug.WriteLine(this.weatherRespondBody);
                return this.weatherRespondBody;
            }
        }

    }
}
