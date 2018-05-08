using Microsoft.Graphics.Canvas.UI.Xaml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using WeatherApp.src;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Numerics;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Brushes;
using Windows.System;
using System.Text.RegularExpressions;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace WeatherApp
    {
        /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class WheatherPage : Page
    {
        Sun sun;
        Moon moon;
        List<Cloud> clouds;
        Precipitation precip;


        string tempType;
        string windType;
        string presType;
        string precipType;
        string visType;
        int days;
        string cityName;


        CanvasBitmap Sun;
        CanvasBitmap Moon;
        float i = 0;
        float dk = 0;
        float dk2 = 0;
        float dk3 = 0;
        byte b = 0;
        float width ;
        float height;
        bool forward = true;
        float pos;
        float cloudHeight = 55;
        float k = 0.8461f;
        float frequency = 100;
        private bool snow = false;
        private bool rain = false;
        bool cheat = false;
        bool cheatTaggled = false;
        string cond;
        Color currentBackGroundColor ;
        DateTime date ;
        Color mainClcl;
        Color secClcl;
        Random rand;


        string respond;
        
        DispatcherTimer moveTimer = new DispatcherTimer();

        Windows.Storage.ApplicationDataContainer localSettings =
            Windows.Storage.ApplicationData.Current.LocalSettings;
        Windows.Storage.StorageFolder localFolder =
            Windows.Storage.ApplicationData.Current.LocalFolder;

        AllWeather allWeather;
        WeatherAPI weatherAPI;

        ObservableCollection<Condition> conditions2 ;
        string conditionsString;

        private ObservableCollection<ForecastDay> forecastDays = new ObservableCollection<ForecastDay>();
        private ObservableCollection<SelectedForecastDay> _forecastDays = new ObservableCollection<SelectedForecastDay>();
        private bool f1;

        public ObservableCollection<SelectedForecastDay> ForecastDays
        {
            get { return this._forecastDays; }
        }

        public VirtualKey Key { get; private set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            

        }

        public string getNeedProperty<T>(string propertyName, string value, object obj)
        {
            if (typeof(T).GetProperty(propertyName + value).GetValue(obj, null) is decimal)
            {
                return Decimal.ToInt32((decimal)typeof(T).GetProperty(propertyName + value).GetValue(obj, null)) + "";
            }
            return typeof(T).GetProperty(propertyName + value).GetValue(obj, null) + "";
        }

        public float random(float min, float max)
        {
            return (float)(this.rand.NextDouble() * (max - min) + min);
        }

        public WheatherPage()
        {

            this.InitializeComponent();

            canvasControl.Draw += CanvasControl_Draw1;
            canvasControl.Invalidate();
            Window.Current.VisibilityChanged += Current_VisibilityChanged;

            canvasWidth();
            canvasHeight();

            rand = new Random();

            sun = new Sun(0,0,44,12);

            moon = new Moon(150, 75, 44);

            precip = new Precipitation(cloudHeight * k, 500, 1600);



            tempType = localSettings.Values["tempType"] != null ? localSettings.Values["tempType"].ToString() : "c";
            windType = localSettings.Values["windType"] != null ? localSettings.Values["windType"].ToString() : "kph";
            presType = localSettings.Values["presType"] != null ? localSettings.Values["presType"].ToString() : "mb";
            precipType = localSettings.Values["precipType"] != null ? localSettings.Values["precipType"].ToString() : "mm";
            visType = localSettings.Values["visType"] != null ? localSettings.Values["visType"].ToString() : "km";
            days = localSettings.Values["days"] != null ? Convert.ToInt32(localSettings.Values["days"].ToString()) : 5;
            cityName = localSettings.Values["cityName"] != null ? localSettings.Values["cityName"].ToString() : "kiev";

            weatherAPI = new WeatherAPI(cityName, days);
            
            try
            {


                respond = weatherAPI.getRespond();
                allWeather = JsonConvert.DeserializeObject<AllWeather>(respond);

                JObject respondObject = JObject.Parse(respond);
                IList<JToken> respondForecastList = respondObject["forecast"]["forecastday"].Children().ToList();
                forecastDays = new ObservableCollection<ForecastDay>();
                foreach (JToken respondForecast in respondForecastList)
                {
                    ForecastDay forecastDay = respondForecast.ToObject<ForecastDay>();
                    forecastDay.makeFormatedDate();
                    forecastDays.Add(forecastDay);
                };


                foreach (ForecastDay item in forecastDays)
                {
                    SelectedForecastDay SelectedforecastDay = new SelectedForecastDay(
                        item.dateFormated,
                        getNeedProperty<Day>("maxtemp_", tempType, item.day) + "° ",
                        getNeedProperty<Day>("mintemp_", tempType, item.day) + "° ",
                        getNeedProperty<Day>("avgtemp_", tempType, item.day) + "° " + tempType.ToUpper(),
                        getNeedProperty<Day>("maxwind_", windType, item.day) + " " + windType,
                        getNeedProperty<Day>("totalprecip_", precipType, item.day) + " " + precipType,
                        getNeedProperty<Day>("avgvis_", visType, item.day) + "° " + visType,
                        item.day.avghumidity + "%",
                        item.day.condition.text,
                        "ms-appx:///Assets/weather/" + item.day.condition.icon.Substring(30),
                        item.day.condition.code + "",
                        item.astro.moonset,
                        item.astro.moonrise,
                        item.astro.sunrise,
                        item.astro.sunset);
                    _forecastDays.Add(SelectedforecastDay);

                }

                location.Text = allWeather.location.name + ", " + allWeather.location.country;
                currentTempreture.Text = getNeedProperty<CurrentWeather>("temp_", tempType, allWeather.current) + "° " + tempType;
                currentDate.Text = DateTime.Now.ToString("ddd d", new CultureInfo("en-US"));
                currentCondition.Text = allWeather.current.condition.text;
                currentWind.Text = "Wind" + getNeedProperty<CurrentWeather>("wind_", windType, allWeather.current) + " " + windType + " " + allWeather.current.wind_dir;
                currentPresure.Text = "Presure " + getNeedProperty<CurrentWeather>("pressure_", presType, allWeather.current) + " " + presType;
                currentHumidity.Text = "Humidity " + allWeather.current.humidity + "%" + " " + getNeedProperty<CurrentWeather>("precip_", precipType, allWeather.current) + " " + precipType;
                currentTempFeelslike.Text = "Feelslike " + getNeedProperty<CurrentWeather>("feelslike_", tempType, allWeather.current) + "° " + tempType;
                currentVis.Text = "Visibility " + getNeedProperty<CurrentWeather>("vis_", visType, allWeather.current) + " " + visType;

                conditionsString = weatherAPI.getConditions();
                conditions2 = new ObservableCollection<Condition>(JsonConvert.DeserializeObject<List<Condition>>(conditionsString));
                foreach (Condition cond in JsonConvert.DeserializeObject<List<Condition>>(conditionsString))
                {
                    conditionPicker.Items.Add(cond.night);
                }
                conditionPicker.SelectedItem = allWeather.current.condition.text;
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }



            getFrequency();
            clouds = new List<Cloud>();
            width = 1700;
            float x = width / frequency;
            for (float i = 0; i < frequency; i++)
            {
                var cl = new Cloud(x / 2 + i*x, cloudHeight * k + random(-1 * (cloudHeight * k) / 2, (cloudHeight * k) / 1.5f),
                                    (int)random(4, 12), cloudHeight, 0);
                clouds.Add(cl);

            }

            moveTimer.Interval = new TimeSpan(1);
            moveTimer.Tick += MoveTimer_Tick;
            moveTimer.Start();
        }

        private void canvasControl_CreateResources(CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(createResourcesAsync(sender).AsAsyncAction());
        }

        async Task createResourcesAsync(CanvasAnimatedControl sender)
        {
            Sun = await CanvasBitmap.LoadAsync(sender,new Uri("ms-appx:///Assets/icons/Sun.png"));
            Moon = await CanvasBitmap.LoadAsync(sender,new Uri("ms-appx:///Assets/icons/Moon.png"));
        }

        private void CanvasControl_Draw1(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var sesion = args.DrawingSession;


            sun.Args = args;
            sun.Sender = sender;

            moon.Args = args;
            moon.Sender = sender;

            precip.Args = args;
            precip.Sender = sender;

            sesion.Clear(currentBackGroundColor);

            pos = getCurrentAstroPositionSun(date);
            //sesion.DrawImage(Sun, pos, F(height * 0.7f, width / 2, pos, width / 2, height) - 43);
            sun.draw(pos, F(height * 0.7f, width / 2, pos, width / 2, height), dk);

            pos = getCurrentAstroPositionMoon(date);
            //sesion.DrawImage(Moon, pos, F(height * 0.7f, width / 2, pos, width / 2, height) - 43);
            moon.draw(pos, F(height * 0.7f, width / 2, pos, width / 2, height), currentBackGroundColor, dk2);

            if (rain)
            {
                precip.makeRain(1000,new Vector2(2,2),Colors.DeepSkyBlue);
            }
            if (snow)
            {
                precip.makeSnow(1000, new Vector2(2, 2));
            }

            foreach (Cloud cloud in clouds.ToList<Cloud>())
            {
                cloud.Args = args;
                cloud.Sender = sender;
                cloud.draw(mainClcl,secClcl);
            }
        }

        public float maxWidth()
        {
            float maxWidth = clouds[0].width;
            foreach(Cloud cloud in clouds)
            {
                if (cloud.width > maxWidth)
                {
                    maxWidth = cloud.width;
                }
            }
            return maxWidth;
        }

        private void MoveTimer_Tick(object sender, object e)
        {
            cheat = togleCheat.IsOn;
            canvasWidth();
            canvasHeight();
            date = cheat ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, timePicker.Time.Hours, timePicker.Time.Minutes, 0, 0) : DateTime.Now; //new DateTime(2018, 04, 26, 17, 11, 0, 0);
                b = getCurrentColor(date); 
                currentBackGroundColor = Color.FromArgb(255, (byte)(b / 2), (byte)(b / 2), b);
            mainClcl = Colors.WhiteSmoke;
            secClcl = Colors.Gray;

            getFrequency();
            if (!cheat && cheatTaggled)
            {
                clouds = new List<Cloud>();
                width = 1700;
                float x = width / frequency;
                for (float i = 0; i < frequency; i++)
                {
                    var cl = new Cloud(x / 2 + i * x, cloudHeight * k + random(-1 * (cloudHeight * k) / 2, (cloudHeight * k) / 1.5f),
                                        (int)random(4, 12), cloudHeight, 0);
                    clouds.Add(cl);
                }
                cheatTaggled = !cheatTaggled;
            }

            float maxWidth = (float)this.maxWidth();
            dk3 = -3;
            
            var lastId = clouds.Count - 1;
            if (clouds[0].maxX < -25)
            {
                clouds[0].makeCloud(width+100, cloudHeight * k + random(-1 * (cloudHeight * k) / 3, (cloudHeight * k) / 1.5f),
                                        (int)random(4, 12), lastId);
                clouds = Ofset(clouds);
            }

            foreach (Cloud cl in clouds)
            {
                cl.move(dk3);
            }


            float animSpeed = 0.15f;
            float animSpeed2 = 0.3f;
            if (forward && dk < 5)
            {
                dk += animSpeed;
            }
            else if (forward)
            {
                forward = !forward;
                dk -= animSpeed;
            }
            else if (!forward && dk >= 0)
            {
                dk -= animSpeed;
            }
            else
            {
                forward = !forward;
            }

            if (dk2 < (20 / animSpeed2 - 20 % animSpeed2) * animSpeed2) 
            {
                dk2 += animSpeed2;
            }
            else
            {
                dk2 = 0;
            }

            
        }

        private List<Cloud> Ofset(List<Cloud> list)
        {
            Cloud buffer = list[0];
            for (int i =0; i<list.Count-1;i++)
            {
                list[i] = list[i + 1];
                //list[i].id--;
            }
            list[list.Count - 1] = buffer;
            return list;
        }

        public float canvasWidth()
        {
            return width = (float)canvasControl.ActualWidth;
        }

        public float canvasHeight()
        {
            return height = (float)canvasControl.ActualHeight;
        }

        private void Current_VisibilityChanged(object sender, Windows.UI.Core.VisibilityChangedEventArgs e)
        {
            if (Window.Current.Visible)
                canvasControl.Invalidate();
        }

        public float getCurrentAstroPositionMoon( DateTime date)
        {

            var riseDate = DateTime.ParseExact(forecastDays[0].date + " " + forecastDays[0].astro.moonrise, "yyyy-M-d h:mm tt", CultureInfo.InvariantCulture);
            var setDate = DateTime.ParseExact(forecastDays[0].date + " " + forecastDays[0].astro.moonset, "yyyy-M-d h:mm tt", CultureInfo.InvariantCulture);
            if (date.Hour>=riseDate.Hour || date.Hour<=setDate.Hour)
            {
                //set for rise date next day
                if (setDate.Hour < riseDate.Hour)
                {
                    setDate = setDate.AddDays(1);
                }

                if (setDate.Day > date.Day)
                {
                    setDate = setDate.AddDays(-1);
                    riseDate = riseDate.AddDays(-1);
                }

                var h = Math.Abs((riseDate - date).Hours);
                float differ = Math.Abs((riseDate - setDate).Hours);

                return (width / (differ * 60 * 60)) * (h * 60 * 60 + date.Minute * 60 + date.Second);
            }
            return -20;
            
        }

        public float getCurrentAstroPositionSun(DateTime date)
        {
            var riseDate = DateTime.ParseExact(forecastDays[0].date + " " + forecastDays[0].astro.sunrise, "yyyy-M-d h:mm tt", CultureInfo.InvariantCulture);
            var setDate = DateTime.ParseExact(forecastDays[0].date + " " + forecastDays[0].astro.sunset, "yyyy-M-d h:mm tt", CultureInfo.InvariantCulture);
            if (date.Hour >= riseDate.Hour)
            {
                if (setDate.Hour < riseDate.Hour)
                {
                    setDate = setDate.AddDays(1);
                }


                var h = Math.Abs((riseDate - date).Hours);
                float differ = Math.Abs((riseDate - setDate).Hours);

                return (width / (differ * 60 * 60)) * (h * 60 * 60 + date.Minute * 60 + date.Second);
            }
            return -20;
            
        }

        public byte getCurrentColor(DateTime date)
        {
            double k = 720 / 200;
            if (date.Hour<12)
            {
                return (byte)((date.Hour * 60 + date.Minute) / k);
            }
            else
            {
                return (byte)(255 - ((date.Hour) * 60 + date.Minute) / k);
            }
        }

        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (!Window.Current.Visible) return;
            var sesion = args.DrawingSession;
            float width =  (float)canvasControl.ActualWidth;
            float height =  (float)canvasControl.ActualHeight;
            sesion.Clear(Colors.BlueViolet);
            if (i < width)
            {
                sesion.DrawCircle(i, F(height * 0.7f, width / 2, i, width / 2, height), 1, Colors.Crimson, 2);
            }
            else i = 0;
        }

        public float F(float a,float b, float x, float ofsetX,float ofsetY)
        {
            return -1*(float)(a / b * Math.Sqrt(b*b- (x - ofsetX)*(x-ofsetX))-ofsetY);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cheatPanel.IsPaneOpen = !cheatPanel.IsPaneOpen;
            //if (moveTimer.IsEnabled)
            //{
            //    moveTimer.Stop();

            //}
            //else
            //{
            //    moveTimer.Start();
            //}
        }

        public float getFrequency()
        {
            cond = cheat ? conditionPicker.SelectedItem.ToString() : allWeather.current.condition.text;

            if (cond == conditions2[0].day| cond == conditions2[0].night)
            {
                frequency = 2;
                rain = false;
                snow = false;
            }
            else if (cond == conditions2[1].night)
            {
                frequency = 5;
                rain = false;
                snow = false;
            }
            else if (cond == conditions2[2].night)
            {
                frequency = 10;
                rain = false;
                snow = false;
            }
            else if (cond == conditions2[3].night)
            {
                frequency = 15;
                rain = false;
                snow = false;
            }
            else if (Regex.IsMatch(cond, "rain") || Regex.IsMatch(cond, "Rain"))
            {
                frequency = 20;
                rain = true;
                snow = false;

            }
            else if (Regex.IsMatch(cond, "snow") || Regex.IsMatch(cond, "Snow"))
            {
                frequency = 20;
                snow = true;
                rain = false;
            }
            else if (Regex.IsMatch(cond, "sleet ") || Regex.IsMatch(cond, "Sleet "))
            {
                frequency = 20;
                rain = true;
                snow = true;
            }
            else
            {
                frequency = 7;
                rain = false;
                snow = false;
            }


            return frequency;
        }

        private void conditionPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            getFrequency();
            if (cheat)
            {              
                clouds = new List<Cloud>();
                width = 1700;
                float x = width / frequency;
                for (float i = 0; i < frequency; i++)
                {
                    var cl = new Cloud(x / 2 + i * x, cloudHeight * k + random(-1 * (cloudHeight * k) / 2, (cloudHeight * k) / 1.5f),
                                        (int)random(4, 12), cloudHeight, 0);
                    clouds.Add(cl);                }
            }
        }
        
        private void togleCheat_Tapped(object sender, TappedRoutedEventArgs e)
        {
            getFrequency();
            if (!cheat)
            {
                clouds = new List<Cloud>();
                width = 1700;
                float x = width / frequency;
                for (float i = 0; i < frequency; i++)
                {
                    var cl = new Cloud(x / 2 + i * x, cloudHeight * k + random(-1 * (cloudHeight * k) / 2, (cloudHeight * k) / 1.5f),
                                        (int)random(4, 12), cloudHeight, 0);
                    clouds.Add(cl);
                }
            }
        }

        private void togleCheat_Toggled(object sender, RoutedEventArgs e)
        {
            cheatTaggled = true;
        }
    }
}
