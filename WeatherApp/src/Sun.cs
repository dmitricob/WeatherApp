
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace WeatherApp.src
{
    class Sun : AnimParts
    {
        
        int rayNum;
        float degrePerRay;
        float degreeKoef = 1.125f;
        float radKoef = 0.16f;

        public Sun(float x0, float y0, float r, int rayNum)
        {
            base.x0 = x0;
            base.y0 = y0;
            base.r = r;
            this.rayNum = rayNum;

            degrePerRay = 360 / (float)rayNum;

        }

        public void draw(float x0, float y0, float dr)
        {
            this.x0 = x0;
            this.y0 = y0;
            this.draw(dr);
        }

        public void draw(float dR)
        {
            session = args.DrawingSession;
            session.FillCircle(x0,y0,r,Colors.Orange);
            session.DrawCircle(x0,y0,r,Colors.DarkOrange);
            Vector2[] coords = new Vector2[3];
            for (float i = 0; i < 360; i+=degrePerRay)
            {
                coords[0] = new Vector2(this.getCoord(x0, y0, dR + r * radKoef * 3 + r,i + dR)[0], 
                                        this.getCoord(x0, y0, dR + r * radKoef * 3 + r, i + dR)[1]);
                coords[1] = new Vector2(this.getCoord(x0, y0, dR + r * radKoef + r, i+dR - degrePerRay / (2*degreeKoef))[0], 
                                        this.getCoord(x0, y0, dR + r * radKoef + r, i + dR - degrePerRay / (2 * degreeKoef))[1]);
                coords[2] = new Vector2(this.getCoord(x0, y0, dR + r * radKoef + r, i + dR + degrePerRay / (2 * degreeKoef))[0], 
                                        this.getCoord(x0, y0, dR + r * radKoef + r, i + dR + degrePerRay / (2 * degreeKoef))[1]);

                session.FillGeometry(CanvasGeometry.CreatePolygon(Sender, coords), Colors.Orange);
            }
        }

        
    }
}
