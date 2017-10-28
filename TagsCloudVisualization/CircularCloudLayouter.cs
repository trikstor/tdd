using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public List<Rectangle> AllRectangles { get; private set; }
        private Point Center { get; set; }
        private List<Point> PossiblePos { get; set; }
        private int PossiblePosIndex = -1;

        public CircularCloudLayouter(Point center)
        {
            if(center.X < 0 || center.Y < 0)
                throw new ArgumentException("Coordinates must be positive or zero");
            Center = center;
            AllRectangles = new List<Rectangle>();
            PossiblePos = new List<Point>();

            VogelsModel(Center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if(rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Size must be positive");

            for(var i = 0; i < 1000; i++)
            {
                var currRect = new Rectangle(GetPoint(), rectangleSize);
                if (PossiblePosIndex == 0)
                {
                    AllRectangles.Add(currRect);
                    return currRect;
                }
                else if (!AllRectangles.Any(rect => rect.IntersectsWith(currRect)))
                {
                    AllRectangles.Add(currRect);
                    return currRect;
                }
            }
            return AllRectangles[0];
        }

        private Point GetPoint()
        {
            if (PossiblePosIndex == -1)
            {
                PossiblePosIndex++;
                return Center;
            }
            PossiblePosIndex++;
            return PossiblePos[PossiblePosIndex];
        }

        private void VogelsModel(Point center)
        {
            const int c = 10;

            Func<int, double> takeR = n => c * Math.Sqrt(n);
            Func<int, double> takeO = n => n * 137.5;

            for (var n = 0; n < 1000; n++)
            {
                var x = Convert.ToInt32(takeR(n) * Math.Cos(takeO(n))) + Center.X;
                var y = Convert.ToInt32(takeR(n) * Math.Sin(takeO(n))) + Center.Y;

                PossiblePos.Add(new Point(x, y));
            }
        }
    }
}
