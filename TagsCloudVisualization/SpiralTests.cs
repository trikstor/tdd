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

        [TestCase(0, "Плотность спирали должна быть положительной.", TestName = "spiralDensity is null")]
        public void SpiralConstructor_ThrowException(double spiralDensity, string exMessage)
        {
            Action res = () => { new Spiral(spiralDensity, Center); };
            res.ShouldThrow<ArgumentException>().WithMessage(exMessage);
        }

        [Test]
        public void SpiralGet_NormalData_CorrectSpiral()
        {
            var actualSpiral = new Spiral(1, Center).GetEnumerator();
            var actualPoints = new List<Point>();
            for (var i = 0; i < 5; i++)
            {
                actualSpiral.MoveNext();
                actualPoints.Add(actualSpiral.Current);
            }
            var expectedPoints = new List<Point>
            {
                new Point(500, 499),
                new Point(501, 499),
                new Point(500, 500),
                new Point(499, 499),
                new Point(498, 499)
            };
            actualPoints.ShouldBeEquivalentTo(expectedPoints);
        }
    }
}
