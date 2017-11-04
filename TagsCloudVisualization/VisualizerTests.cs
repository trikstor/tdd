using NUnit.Framework;
using System;
using System.Drawing;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class VisualizerTests
    {
        [Test]
        public void DynamicFrameConstructor_ZeroCenterPoint_ThrowException()
        {
            Action res = () => { new Visualizer("test.bmp", new Point(0, 0));};
            res.ShouldThrow<ArgumentException>().WithMessage("Центр должен быть больше нуля.");
        }

        [Test]
        public void DynamicFrameConstructor_PositiveCenterPoint_NotThrowException()
        {
            Action res = () => { new Visualizer("test.bmp", new Point(10, 20)); };
            res.ShouldNotThrow();
        }
        [Test]
        public void StaticFrameConstructor_ZeroCenterPoint_ThrowException()
        {
            Action res = () => { new Visualizer("test.bmp", new Point(10, 20), 0); };
            res.ShouldThrow<ArgumentException>().WithMessage("Центр должен быть больше нуля.");
        }

        [Test]
        public void StaticFrameConstructor_PositiveCenterPoint_NotThrowException()
        {
            Action res = () => { new Visualizer("test.bmp", new Point(0, 0), 100); };
            res.ShouldNotThrow();
        }
    }
}
