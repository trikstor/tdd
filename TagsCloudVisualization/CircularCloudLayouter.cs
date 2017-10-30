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
        private int possiblePosIndex = -1;
        /// <summary>
        /// Кол - во точек спирали, чем больше
        /// точек - тем больше прямоугольников можно разместить.
        /// </summary>
        private readonly int possiblePosQuant = 100000;
        /// <summary>
        /// Плотность спирали, при уменьшении коэффициента
        /// необходимо увеличивать possiblePosQuant.
        /// </summary>
        private double spiralDensity = 0.5;
        private Point Center { get; }

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("Coordinates must be positive or zero");

            Center = center;
            AllRectangles = new List<Rectangle>();
            PossiblePos = new List<Point>();

            VogelsModel(Center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Size must be positive");

            for (var i = possiblePosIndex + 1; i < possiblePosQuant; i++)
            {
                var currRect = new Rectangle(GetPoint(rectangleSize), rectangleSize);
                if (possiblePosIndex == 0)
                {
                    AllRectangles.Add(currRect);
                    return currRect;
                }
                if (!AllRectangles.Any(rect => rect.IntersectsWith(currRect)))
                {
                    AllRectangles.Add(currRect);
                    return currRect;
                }
            }
            throw new ArgumentException("Too many rectangles.");
        }

        private Point GetPoint(Size rectangleSize)
        {
            if (possiblePosIndex == -1)
            {
                possiblePosIndex++;
                return PointСalibration(Center, rectangleSize);
            }
            possiblePosIndex++;
            return
                PointСalibration(PossiblePos[possiblePosIndex], rectangleSize);
        }

        /// <summary>
        /// Позиционирование прямоугольника в центре точки, а не от
        /// левого верхнего угла, как по умолчанию.
        /// </summary>
        /// <returns>Возвращает новую точку со смещением к центру прямоугольнка.</returns>
        public Point PointСalibration(Point currPoint, Size rectangleSize)
        {
            var newPoint = new Point
            {
                X = currPoint.X - rectangleSize.Width / 2,
                Y = currPoint.Y - rectangleSize.Height / 2
            };

            return newPoint;
        }

        /// <summary>
        /// Задает возможные точки для прямоугольников
        /// по модели Вогеля: точки располагаются по спирали Ферма.
        /// </summary>
        /// <param name="center">Центр спирали</param>
        private void VogelsModel(Point center)
        {
            Func<int, double> takeR = n => spiralDensity * Math.Sqrt(n);
            Func<int, double> takeO = n => n * 137.5;

            for (var n = 0; n < possiblePosQuant; n++)
            {
                var x = Convert.ToInt32(takeR(n) * Math.Cos(takeO(n))) + Center.X;
                var y = Convert.ToInt32(takeR(n) * Math.Sin(takeO(n))) + Center.Y;

                PossiblePos.Add(new Point(x, y));
            }
        }

        public void Drawer(string path)
        {
            var bitmap = new Bitmap(1000, 1000);
            var gr = Graphics.FromImage(bitmap);

            var rectPen = new Pen(Color.Blue);
            var centerPen = new Pen(Color.Gray);

            gr.DrawLine(centerPen, Center.X-500, Center.Y, Center.X + 500, Center.Y);
            gr.DrawLine(centerPen, Center.X, Center.Y - 500, Center.X, Center.Y + 500);

            foreach (var rectangle in AllRectangles)
                gr.DrawRectangle(rectPen, rectangle);

            gr.Dispose();
            bitmap.Save(path);
        }
    }
}
