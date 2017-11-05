﻿using NUnit.Framework;
using System;
using System.Drawing;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class VisualizerShould
    {
        public void ThrowException_ZeroCenterPoint()
        {
            Action res = () => { new Visualizer("test.bmp", new Point(0, 0));};
            res.ShouldThrow<ArgumentException>()
                .WithMessage("Координаты центра должны быть больше нуля.");
        }

        [Test]
        public void NotThrowException_PositiveCenterPoint()
        {
            Action res = () => { new Visualizer("test.bmp", new Point(5, 1)); };
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
