using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral : IEnumerable<Point>
    {
        /// <summary>
        /// Плотность спирали, при уменьшении коэффициента
        /// необходимо увеличивать possiblePosQuant.
        /// </summary>
        private double SpiralDensity { get; }

        private Point SpiralCenter { get; }

        public Spiral(double spiralDensity, Point spiralCenter)
        {
            if(spiralCenter == null)
                throw new ArgumentException("Точка центра спирали должна быть заданна.");
            if (spiralDensity < 0.1)
                throw new ArgumentException("Плотность спирали должна быть положительной.");
            SpiralDensity = spiralDensity;
            SpiralCenter = spiralCenter;
        }

        private const double GoldAngle = 137.508;
        private double TakeRPart(int n) => SpiralDensity * Math.Sqrt(n);
        private double TakeOPart(int n) => n * GoldAngle;

        /// <summary>
        /// Задает возможные точки для прямоугольников
        /// по модели Вогеля: точки располагаются по спирали Ферма.
        /// Угол 137.508° - золотой угол, который приближается 
        /// к соотношениям чисел Фибоначчи.
        /// </summary>
        public IEnumerator<Point> GetEnumerator()
        {
            var n = 0;
            while(true)
            {
                var x = Convert.ToInt32(TakeRPart(n) * Math.Cos(TakeOPart(n))) + SpiralCenter.X;
                var y = Convert.ToInt32(TakeRPart(n) * Math.Sin(TakeOPart(n))) + SpiralCenter.Y;

                yield return new Point(x, y);
                n++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
