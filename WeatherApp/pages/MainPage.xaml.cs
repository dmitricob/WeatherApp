using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using WeatherApp.pages;
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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace WeatherApp
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            mainPage.Navigate(typeof(WheatherPage));
            mainMenu.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mySplitView.IsPaneOpen = !mySplitView.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainItem.IsSelected)
            {
                mainPage.Navigate(typeof(WheatherPage));
            }
            else if (settingsItem.IsSelected)
            {
                mainPage.Navigate(typeof(SettingsPage));
            }

        }

        private void canvasControl_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {

        }

        private void canvasControl_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            var time = DateTime.Now;
            var session = args.DrawingSession;
            session.Clear(Colors.Transparent);

            var width = 36f;
            var radius = width * 0.45f;
            var xCenter = (float)width / 2;
            var yCenter = (float)width / 2;

            session.DrawEllipse(xCenter,yCenter,radius,radius,Colors.Black,2f);
            session.FillCircle(xCenter,yCenter,radius,Colors.BlueViolet);
            session.DrawEllipse(xCenter,yCenter,1,1,Colors.Black,2f);

            var secAngl = (time.Second + (time.Millisecond / 1000f)) * 6f;
            var minAngl = (time.Minute * 6f) + secAngl / 60f;
            var hourAngl = (time.Hour % 12 * 30f) + minAngl / 12f;

            Action<float, double, float> drawline = (angle, length, lineWidth) =>
            {
                var radAng = (Math.PI / 180) * angle;
                var x = xCenter + (Math.Cos(radAng) * length);
                var y = yCenter + (Math.Sin(radAng) * length);
                session.DrawLine(xCenter, yCenter, (int)x, (int)y, Colors.Black, lineWidth);
            };

            drawline(secAngl - 90, radius * 0.8, 1);
            drawline(minAngl - 90, radius * 0.9, 2);
            drawline(hourAngl - 90, radius * 0.5, 2.5f);

            canvasControl.Invalidate();


        }
    }
}
