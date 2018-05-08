
using Microsoft.Graphics.Canvas.Text;
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
    class Cloud : AnimParts
    {
        float maxHeight; // max cloud height
        float minHeight;
        public float minX;
        public float maxX;
        public float width;
        int maxElements; // cloudn cicles count
        float botom;
        List<Vector3> coords;
        public int id;

        public float getX0()
        {
            return x0;
        }
        public void set(float x0, float y0)
        {
            this.x0 = x0;
            this.y0 = y0;
        }

        public Cloud(float x0, float y0,int maxElements,float maxHeight,int id)
        {
            this.id = id;
            base.x0 = x0;
            base.y0 = y0;
            base.sender = sender;
            base.args = args;
            this.maxHeight = maxHeight;
            this.minHeight = maxHeight;
            this.maxX = x0;
            this.minX = x0;
            this.maxElements = maxElements-1;
            botom = y0 + maxHeight/2;
            this.makeCloud();
        }

        public Cloud(int maxElements, float maxHeight)
        {
            base.x0 =-50;
            base.y0 =-50;
            base.sender = sender;
            base.args = args;
            this.maxHeight = maxHeight;
            this.minHeight = maxHeight;
            this.maxX = x0;
            this.minX = x0;
            this.maxElements = maxElements - 1;
            botom = y0 + maxHeight / 2;
            this.coords = new List<Vector3>();
            this.makeCloud();
        }

        public void move(float dx,float dy)
        {
            this.x0 += dx;
            this.y0 += dy;

            this.maxX += dx;
            this.minX += dx;
        }

        public void move(float dx)
        {
            Debug.WriteLine(dx);
            this.move(dx,0);
        }


        public void draw(Color mainClaudColor, Color secondClaudColor, float dr)
        {
            session = args.DrawingSession;
            
            float lineWeight = 5;

            session.FillCircle(x0+dr, y0, maxHeight / 2 + lineWeight, secondClaudColor);
            foreach (Vector3 coord in coords.ToList<Vector3>())
            //foreach (Vector3 coord in coords)
            {
                session.FillCircle(coord.X+x0+dr, coord.Y+y0, coord.Z + lineWeight, secondClaudColor);
            }
            session.FillCircle(x0+dr, y0, maxHeight/2, mainClaudColor);
            foreach (Vector3 coord in coords.ToList<Vector3>())
            {
                session.FillCircle(coord.X+x0+dr, coord.Y+y0, coord.Z, mainClaudColor);
                
            }
        }

        public void draw(Color mainClaudColor, Color secondClaudColor)
        {
            this.draw(mainClaudColor,secondClaudColor,0);
        }

        public void makeCloud()
        {
            int leftElements = (int)rand(1,maxElements);
            //int leftElements = (int)maxElements / 2;
            int rightElements = maxElements - leftElements;
            this.coords = new List<Vector3>();

            botom = y0 + maxHeight / 2;
            setCoords(leftElements,180);
            setCoords(rightElements, 0);
            

        }
        public void makeCloud(float x0, float y0, int maxElements,int id)
        {
            base.x0 = x0;
            base.y0 = y0;
            this.maxX = x0;
            this.minX = x0;
            this.maxElements = maxElements - 1;
            makeCloud();
            //this.id = id;
        }

        private void setCoords(int elements,float angleDegre)
        {
            float[] coords = new float[2] { x0,y0};
            float x = x0;
            float y = y0;
            float r = maxHeight/2;
            float ang ;
            float radAng ;
            //Debug.WriteLine("");
            //Debug.WriteLine(id + "");
            //Debug.WriteLine("Center coords :" + (int)x + " : " + (int)y);
            //Debug.WriteLine("begin radius" + r + "  elsements" + elements );
            //Debug.WriteLine(" ");

            for (int i = 0; i < elements; i++)
            {
                ang = Math.Abs(randAng() - angleDegre);  // get current angle whith needed angle offset (0 or 180)
                radAng = base.rad(ang);

                x = coords[0];
                y = coords[1];

                coords = getCoord(coords[0],coords[1],r,ang);
                r = botom - coords[1];
                coords[0] = coords[0] + (base.rand(0,r/2) * ang > 90 ? -1 : 1);

               
                //Debug.WriteLine("begin coords :" + (int)x + " : " + (int)y);
                //Debug.WriteLine("angle: " + (int)ang + " new rad: " + (int)r + " = " + botom +" + " + coords[1] );
                //Debug.WriteLine("new coords   :" + +(int)coords[0] + " : " + (int)coords[1]);
                //Debug.WriteLine("");
                minHeight = r < minHeight ? r : minHeight;
                this.coords.Add(new Vector3( coords[0]-x0,coords[1]-y0,r));
            }
            minX = coords[0] < minX ? coords[0] : minX;
            maxX = coords[0] > maxX ? coords[0] : maxX;
            width = maxX - minX;

        }
        
        public float randAng()
        {
            if (base.rand(0, 10) < 6)
            {
                return base.rand(5, 15);

            }
            else if(base.rand(0, 10) < 4)
            {
                return base.rand(16, 30);

            }
            else
                return base.rand(-30, 4);
        }
    }
}
