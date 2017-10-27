using System;
using System.Drawing;
using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public CircularCloudLayouter(Point center)
        {
            if(center.X < 0 || center.Y < 0)
                throw new ArgumentException("Coordinates must be positive or zero");
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            return new Rectangle(new Point(10, 10), rectangleSize);
        }
    }
}
