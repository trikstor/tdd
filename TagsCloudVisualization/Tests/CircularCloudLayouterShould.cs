using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using System.IO;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterShould
    {
        private CircularCloudLayouter Layout {get; set;}
        private Point Center { get; set; }

        [SetUp]
        public void SetUp()
        {
            Center = new Point(500, 500);
            Layout = new CircularCloudLayouter(Center);
        }

        [TestCase(-10, 5, "Coordinates must be positive or zero", TestName = "Create a new layout with negative cordinate(s)")]
        public void ThrowException_UncorrectParams(int x, int y, string exMessage)
        {
            Action res = () => { new CircularCloudLayouter(new Point(x, y)); };
            res.ShouldThrow<ArgumentException>().WithMessage(exMessage);
        }

        [TestCase(15, 5, TestName = "Return right rectangle")]
        public void PutNextRectangle_ReturnRightRectangle(int width, int height)
        {
            var actual = Layout.PutNextRectangle(new Size(width, height));
            var expected = new Rectangle(
                Layout.GetRectangleCenterOffset(
                    new Point(500, 500), new Size(width, height)), new Size(width, height));

            actual.ShouldBeEquivalentTo(expected, options =>
                options.ExcludingNestedObjects());
        }

        [TestCase(0, 5, "Size must be positive", 
            TestName = "Create a new rectangle with negative or zero size")]
        public void PutNextRectangle_ThrowException(int width, int height, string exMessage)
        {
            Action res = () => { Layout.PutNextRectangle(new Size(width, height));};
            res.ShouldThrow<ArgumentException>().WithMessage(exMessage);
        }

        [Test]
        public void PutNextRectangle_QuantityOfRectangles_EqualsQuantity()
        {
            var expectedQuantity = 5;
            var sizeOfRectangles = new Size[expectedQuantity];

            for (var i = 0; i < expectedQuantity; i++)
                sizeOfRectangles[i] = new Size(i + 1, i + 2);

            foreach (var size in sizeOfRectangles)
            {
                Layout.PutNextRectangle(size);
            }

            Layout.AllRectangles.Count.Should().Be(expectedQuantity);
        }

        public static bool RectanglesNotOverlap(List<Rectangle> rectangles)
        {
            return rectangles.All(currRect => !rectangles
            .Any(rect => rect.IntersectsWith(currRect) && rect.Size != currRect.Size));
        }

        [TestCase(5, TestName = "Few rectangles")]
        public void PutNextRectangle_NotOverlapOfRectangles(int expectedQuantity)
        {
            FillCloudWithRandRect(5);
            RectanglesNotOverlap(Layout.AllRectangles).Should().BeTrue();
        }

        private Action FillCloudWithRandRect(int expectedQuantity)
        {
            var sizeOfRectangles = new Size[expectedQuantity];
            var rnd = new Random();

            for (var i = 0; i < expectedQuantity; i++)
                sizeOfRectangles[i] = new Size(
                    i + rnd.Next(10, 300), i + rnd.Next(10, 300));

            Action res = () =>
            {
                foreach (var size in sizeOfRectangles)
                {
                    Layout.PutNextRectangle(size);
                }
            };
            return res;
        }

        [Test]
        public void PutNextRectangle_OneRectangle_CenterOfRectСalibration()
        {
            Layout.PutNextRectangle(new Size(200, 100));

            Layout.AllRectangles[0].Location.Should().Be(new Point(400, 450));
        }

        private int DistanceBetweenPoints(Point p1, Point p2)
        {
            return (int)Math.Sqrt(((p1.X - p2.X) * (p1.X - p2.X))
                                  + ((p1.Y - p2.Y) * (p1.Y - p2.Y)));
        }

        private int MaxCenterEnvirons(List<Rectangle> rectangles)
        {
            return rectangles
                .Select(rect => DistanceBetweenPoints(rect.Location, Center))
                .Concat(new[] { 0 }).Max();
        }

        private int MaxRectDiagonal(List<Rectangle> rectangles)
        {
            return rectangles
                .Select(rect => DistanceBetweenPoints(
                    rect.Location, new Point(rect.X + rect.Width, rect.Y + rect.Height)))
                .Concat(new[] { 0 }).Max();
        }

        [Test]
        public void PutNextRectangle_ManyRectangles_CorrectLocation()
        {
            for(var i = 0; i < 7; i++)
            {
                Layout.PutNextRectangle(new Size(100, 100));
            }
            (MaxCenterEnvirons(Layout.AllRectangles) + MaxRectDiagonal(Layout.AllRectangles))
                .Should().BeLessThan(352);
        }

        [TearDown]
        public void TearDown()
        {
            var context = TestContext.CurrentContext;

            if (context.Result.Outcome.Status == TestStatus.Failed)
            {
                var visualize = new Visualizer(
                    Path.Combine(
                        context.TestDirectory, 
                        context.Test.Name +".bmp"
                        ), 
                    Center);
                visualize.Draw(Layout.AllRectangles);

                TestContext.Write("Tag cloud visualization saved to file {path}");
            }
        }
    }
}