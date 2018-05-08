using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace WeatherApp.src
{
    class Precipitation : AnimParts
    {
        float minHeight;
        float maxHeight;
        float width;

        public Precipitation(float minHeight,float maxHeight, float width)
        {
            this.minHeight = minHeight;
            this.maxHeight = maxHeight;
            this.width = width;
        }

        public void makeRain(int count,Vector2 wind,Color color)
        {
            session = args.DrawingSession;
            for (int i = 0; i < count; i++)
            {
                

                var x = rand(0, width);
                var y = rand(minHeight, maxHeight);

                session.DrawLine(x, y, x + wind.X, y + wind.Y, color);
            }
            

        }

        public void makeSnow(int count, Vector2 wind)
        {
            session = args.DrawingSession;
            for (int i = 0; i < count; i++)
            {
                var x = rand(0, width);
                var y = rand(minHeight, maxHeight);

                session.DrawLine(x, y, x + wind.X, y + wind.Y, Colors.Aqua, 2);
            }
        }


    }
}
