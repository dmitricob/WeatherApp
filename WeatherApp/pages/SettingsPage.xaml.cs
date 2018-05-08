using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using WeatherApp.src;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace WeatherApp.pages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        string tempType;
        string windType;
        string presType;
        string precipType;
        string visType;
        int days;
        string cityName;
        Windows.Storage.ApplicationDataContainer localSettings =
                    Windows.Storage.ApplicationData.Current.LocalSettings;
        Windows.Storage.StorageFolder localFolder =
            Windows.Storage.ApplicationData.Current.LocalFolder;

        public SettingsPage()
        {
            this.InitializeComponent();

            if (localSettings.Values != null)
            {
                tempType = localSettings.Values["tempType"] != null ? localSettings.Values["tempType"].ToString() : "c";
                windType = localSettings.Values["windType"] != null ? localSettings.Values["windType"].ToString() : "kph";
                presType = localSettings.Values["presType"] != null ? localSettings.Values["presType"].ToString() : "mb";
                precipType = localSettings.Values["precipType"] != null ? localSettings.Values["precipType"].ToString() : "mm";
                visType = localSettings.Values["visType"] != null ? localSettings.Values["visType"].ToString() : "km";
                days = localSettings.Values["days"] != null ? Convert.ToInt32(localSettings.Values["days"].ToString()) : 5;
                cityName = localSettings.Values["cityName"] != null ? localSettings.Values["cityName"].ToString() : "kiev";
            }
            else
            {
                tempType = "c";
                windType = "kph";
                presType = "mb";
                precipType = "mm";
                visType = "km";
                days = 5;
                cityName = "kiev";
            }

            switch (tempType)
            {
                case  "c" :
                    c.IsChecked = true;
                    break;
                default:
                    f.IsChecked = true;
                    break;
            }

            switch (windType)
            {
                case "kph":
                    km.IsChecked = true;
                    
                    break;
                default:
                    miles.IsChecked = true;
                    break;
            }

            switch (presType)
            {
                case "mb":
                    mb.IsChecked = true;

                    break;
                default:
                    inch.IsChecked = true;
                    break;
            }

            daySlider.Value = days;

            CityName.Text = cityName;


            
        }

        private void daySlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            days = Convert.ToInt32(((Slider)sender).Value);
            localSettings.Values["days"] = days;

        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                WeatherAPI a = new WeatherAPI(((TextBox)sender).Text.ToString());
                var b = a.getRespond();
                if (b == null || b.Length < 95)
                {
                }
                else
                {
                    localSettings.Values["cityName"] = ((TextBox)sender).Text.ToString();
                    cityName = ((TextBox)sender).Text.ToString();
                }
                Debug.WriteLine(b);
            }
            
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void tempretureRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            tempType = ((RadioButton)sender).Tag.ToString();
            localSettings.Values["tempType"] = tempType;

        }

        private void lengthRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            visType = ((RadioButton)sender).Tag.ToString();
            localSettings.Values["visType"] = visType;
            if ((bool)km.IsChecked)
            {
                windType = "kph";
                precipType = "mm";
            }else
            {
                windType = "mph";
                precipType = "in";
            }
            localSettings.Values["windType"] = windType;
            localSettings.Values["precipType"] = precipType;
        }

        private void presureRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            presType = ((RadioButton)sender).Tag.ToString();
            localSettings.Values["presType"] = presType;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WeatherAPI a = new WeatherAPI(CityName.Text.ToString());
            var b = a.getRespond();
            if (b == null || b.Length < 95)
            {
                MessageDialog message = new MessageDialog("some error with city");
                message.ShowAsync();
            }
            else
            {
                localSettings.Values["cityName"] = CityName.Text.ToString();
                cityName = CityName.Text.ToString();
            }
            Debug.WriteLine(b);

        }
    }
}
