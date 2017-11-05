using NUnit.Framework;
using System;
using System.Drawing;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class VisualizerShould
    {
        [TestCase(-1, 0, TestName = "X is negative")]
        [TestCase(0, -1, TestName = "Y is negative")]
        public void ThrowException_NegativeCenterPoint(int x, int y)
        {
            Action res = () => { new Visualizer("test.bmp", new Point(x, y));};
            res.ShouldThrow<ArgumentException>()
                .WithMessage("Координаты центра должны быть больше нуля либо равны нулю.");
        }

        [Test]
        public void NotThrowException_ZeroCenterPoint()
        {
            Action res = () => { new Visualizer("test.bmp", new Point(0, 0)); };
            res.ShouldNotThrow();
        }
        [Test]
        public void ThrowException_ZeroCenterPointAndZeroFrame()
        {
            Action res = () => { new Visualizer("test.bmp", new Point(0, 0), 0); };
            res.ShouldThrow<ArgumentException>().WithMessage("Размер изображения должен быть больше нуля.");
        }

        [Test]
        public void NotThrowException_ZeroCenterPointAndPositiveFrame()
        {
            Action res = () => { new Visualizer("test.bmp", new Point(0, 0), 100); };
            res.ShouldNotThrow();
        }
    }
}
