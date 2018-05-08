using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace WeatherApp.src
{
    class Moon : AnimParts
    {

        public Moon(float x0, float y0, float r)
        {
            base.x0 = x0;
            base.y0 = y0;
            base.r = r;
        }

        public void draw(float x0,float y0, Color backgroundColor,float dr)
        {
            base.x0 = x0;
            base.y0 = y0;
            this.draw(backgroundColor,dr);
        }

        public void draw(Color backgroundColor,float dr)
        {
            session = args.DrawingSession;



            //session.FillCircle(x0, y0, r, Colors.Orange);
            //session.FillCircle(x0+r/2, y0-r/2, r, backgroundColor);

            //float R = r + dr;

            //session.FillCircle(x0, y0, R, Color.FromArgb(150,255,165,0));
            //session.FillCircle(x0 + (r / 2)*1.45f , y0 - (r / 2)*1.45f, r, backgroundColor);

            session.FillCircle(x0, y0, r, Colors.Orange);
            session.FillCircle(x0 + r / 2, y0 , r, backgroundColor); 

            float R = r + dr;

            byte minTransperent = 100;
            byte maxAnimLength = 20;

            //byte transperent = (byte)(minTransperent - dr * minTransperent / maxAnimLength);
            byte transperent = (byte)(0.5*(maxAnimLength- dr)*(maxAnimLength- dr));

            session.FillCircle(x0, y0, R, Color.FromArgb(transperent, 255, 165, 0));
            //Debug.WriteLine(230 - (dr) * 22);
            session.FillCircle(x0 +R- (r / 2), y0, r, backgroundColor);

            //session.DrawText(dr+"",200,50,Colors.Black);

        }
    }
}
