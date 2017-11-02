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
            if(spiralCenter == null)
                throw new ArgumentException("Точка центра спирали должна быть задана.");
            if (possiblePosQuant < 1 || spiralDensity < 0.1)
                throw new ArgumentException("Кол-во точек и плотность спирали должны быть положительными.");

            PossiblePosQuant = possiblePosQuant;
            SpiralDensity = spiralDensity;
            SpiralCenter = spiralCenter;
        }

        private readonly double GoldAngle = 137.508;
        private double TakeRPart(int n) => SpiralDensity * Math.Sqrt(n);
        private double TakeOPart(int n) => n * GoldAngle;

        /// <summary>
        /// Задает возможные точки для прямоугольников
        /// по модели Вогеля: точки располагаются по спирали Ферма.
        /// Угол 137.508° - золотой угол, который приближается 
        /// к соотношениям чисел Фибоначчи.
        /// </summary>
        public IEnumerable<Point> Get()
        {
            for (var n = 0; n < PossiblePosQuant; n++)
            {
                var x = Convert.ToInt32(TakeRPart(n) * Math.Cos(TakeOPart(n))) + SpiralCenter.X;
                var y = Convert.ToInt32(TakeRPart(n) * Math.Sin(TakeOPart(n))) + SpiralCenter.Y;

                yield return new Point(x, y);
            }
        }
    }
}
