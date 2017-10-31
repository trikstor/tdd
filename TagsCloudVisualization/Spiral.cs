using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        /// <summary>
        /// Кол - во точек спирали, чем больше
        /// точек - тем больше прямоугольников можно разместить.
        /// </summary>
        private int PossiblePosQuant { get; }
        /// <summary>
        /// Плотность спирали, при уменьшении коэффициента
        /// необходимо увеличивать possiblePosQuant.
        /// </summary>
        private double SpiralDensity { get; }
        private Point SpiralCenter { get; }

        public Spiral(int possiblePosQuant, double spiralDensity, Point spiralCenter)
        {
            PossiblePosQuant = possiblePosQuant;
            SpiralDensity = spiralDensity;
            SpiralCenter = spiralCenter;
        }

        public List<Point> Get()
        {
            return VogelsModel();
        }
        /// <summary>
        /// Задает возможные точки для прямоугольников
        /// по модели Вогеля: точки располагаются по спирали Ферма.
        /// Угол 137.508° - золотой угол, который приближается 
        /// к соотношениям чисел Фибоначчи.
        /// </summary>
        /// <param name="center">Центр спирали</param>
        private List<Point> VogelsModel()
        {
            var possiblePos = new List<Point>();
            Func<int, double> takeR = n => SpiralDensity * Math.Sqrt(n);
            Func<int, double> takeO = n => n * 137.508;

            for (var n = 0; n < PossiblePosQuant; n++)
            {
                var x = Convert.ToInt32(takeR(n) * Math.Cos(takeO(n))) + SpiralCenter.X;
                var y = Convert.ToInt32(takeR(n) * Math.Sin(takeO(n))) + SpiralCenter.Y;

                possiblePos.Add(new Point(x, y));
            }
            return possiblePos;
        }
    }
}
