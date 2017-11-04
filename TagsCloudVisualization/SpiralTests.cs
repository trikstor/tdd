using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class SpiralTests
    {
        private Point Center { get; set; }

        [SetUp]
        public void SetUp()
        {
            Center = new Point(500, 500);
        }

        [TestCase(0, 10, "Кол-во точек и плотность спирали должны быть положительными.", TestName = "possiblePosQuant is 0")]
        [TestCase(10, 0, "Кол-во точек и плотность спирали должны быть положительными.", TestName = "spiralDensity is null")]
        public void SpiralConstructor_ThrowException(int possiblePosQuant, double spiralDensity, string exMessage)
        {
            Action res = () => { new Spiral(possiblePosQuant, spiralDensity, Center); };
            res.ShouldThrow<ArgumentException>().WithMessage(exMessage);
        }

        [Test]
        public void SpiralGet_NormalData_CorrectSpiral()
        {
            var actualSpiral = new Spiral(5, 1, Center).GetPoints().Take(5).ToList();
            var expectedSpiral = new List<Point>
            {
                new Point(500, 499),
                new Point(501, 499),
                new Point(500, 500),
                new Point(499, 499),
                new Point(498, 499)
            };
            actualSpiral.ShouldBeEquivalentTo(expectedSpiral);
        }
    }
}
