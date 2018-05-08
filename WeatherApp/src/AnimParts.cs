using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace WeatherApp.src
{
    class AnimParts
    {
        
        protected float x0;
        protected float y0;
        protected float r;
        protected Random randObject = new Random();

        protected ICanvasAnimatedControl sender;
        protected CanvasAnimatedDrawEventArgs args;
        protected CanvasDrawingSession session;

        public CanvasAnimatedDrawEventArgs Args { get => args; set => args = value; }
        public ICanvasAnimatedControl Sender { get => sender; set => sender = value; }

        public void draw(float x0, float y0, float dr)
        {
            this.x0 = x0;
            this.y0 = y0;
            this.draw(dr);
        }

        public void draw(float dR)
        {
        }

        public float[] getCoord(float x0, float y0, float length, float degre)
        {
            var radAng = (Math.PI / 180) * degre;
            float x = x0 + (float)(Math.Cos(radAng) * length);
            float y = y0 + (float)(Math.Sin(radAng) * length);
            return new float[] { x, y };
        }

        public float getLength(float x1,float y1,float x2, float y2)
        {
            return (float)Math.Sqrt((x1-x2)*(x1-x2)+(y1-y2)*(y1-y2));
        }

        public float rand(float min, float max)
        {
            return (float)(this.randObject.NextDouble() * (max - min) + min);
        }

        public float rad(float angle)
        {
            return (float)(Math.PI / 180) * angle;
        }
    }
}
