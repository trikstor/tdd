using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public List<Rectangle> AllRectangles { get; }
        /// <summary>
        /// Точки спирали, на которых потенциально можно
        /// расположить прямоугольник.
        /// </summary>
        private List<Point> PossiblePos { get; }
        /// <summary>
        /// Счетчик просмотренных PossiblePos,
        /// 1-ая точка гарантированно Center, по этому
        /// отсчет от -1.
        /// </summary>
        private int PossiblePosIndex = -1;
        /// <summary>
        /// Кол - во точек спирали, чем больше
        /// точек - тем больше прямоугольников можно разместить.
        /// </summary>
        private int PossiblePosQuant { get; }
        private Point Center { get; }

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("Coordinates must be positive or zero");

            Center = center;
            AllRectangles = new List<Rectangle>();
            PossiblePos = new List<Point>();
            PossiblePosQuant = 100000;

            var cloudSpiral = new Spiral(PossiblePosQuant, 0.5, Center);
            PossiblePos = cloudSpiral.Get().ToList();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Size must be positive");

            for (var i = PossiblePosIndex + 1; i < PossiblePosQuant; i++)
            {
                var currRect = new Rectangle(GetPossiblePointForRectCenter(rectangleSize), rectangleSize);

                if (!AllRectangles.Any(rect => rect.IntersectsWith(currRect)) || PossiblePosIndex == 0)
                {
                    AllRectangles.Add(currRect);
                    return currRect;
                }
            }
            throw new ArgumentException("Too many rectangles.");
        }

        private Point GetPossiblePointForRectCenter(Size rectangleSize)
        {
            Point currCenter;
            if (PossiblePosIndex == -1)
                currCenter = Center;
            else
                currCenter = PossiblePos[PossiblePosIndex];
            PossiblePosIndex++;
            return RectangleCenterOffset(currCenter, rectangleSize);
        }


        public Point RectangleCenterOffset(Point currPoint, Size rectangleSize)
        {
            var newPoint = new Point
            {
                X = currPoint.X - rectangleSize.Width / 2,
                Y = currPoint.Y - rectangleSize.Height / 2
            };

            return newPoint;
        }
    }
}
