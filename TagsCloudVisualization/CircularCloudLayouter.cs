using System;
using System.Drawing;
using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public List<Rectangle> AllRectangles { get; private set; }

        public CircularCloudLayouter(Point center)
        {
            if(center.X < 0 || center.Y < 0)
                throw new ArgumentException("Coordinates must be positive or zero");
            AllRectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if(rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Size must be positive");
            var currRect = new Rectangle(new Point(10, 10), rectangleSize);
            AllRectangles.Add(currRect);
            return currRect;
        }

        //private Point GetPoint()
        //{
        //    
        //}

        public static List<Point> VogelsModel(Point center)
        {
            const int c = 20;
            var resPoints = new List<Point>();

            Func<int, double> takeR = n => c * Math.Sqrt(n);
            Func<int, double> takeO = n => n * 137.5;

            for (var n = 0; n < 100; n++)
            {
                var x = Convert.ToInt32(takeR(n) * Math.Cos(takeO(n)));
                var y = Convert.ToInt32(takeR(n) * Math.Sin(takeO(n)));

                resPoints.Add(new Point(x, y));
            }
            return resPoints;
        }
    }
}
