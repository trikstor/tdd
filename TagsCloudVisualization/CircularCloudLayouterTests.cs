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
    public class CircularCloudLayouterTests
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
        public void CircularCloudLayouter_ThrowException_Constructor(int x, int y, string exMessage)
        {
            Action res = () => { new CircularCloudLayouter(new Point(x, y)); };
            res.ShouldThrow<ArgumentException>().WithMessage(exMessage);
        }

        [TestCase(15, 5, TestName = "Return right rectangle")]
        public void PutNextRectangle_ReturnRightRectangle(int width, int height)
        {
            var actual = Layout.PutNextRectangle(new Size(width, height));
            var expected = new Rectangle(
                Layout.RectangleCenterOffset(
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

        [Test]
        public void PutNextRectangle_TooManyRectangles_ThrowException()
        {
            var res = FillCloudWithRandRect(100);
            res.ShouldThrow<ArgumentException>().WithMessage("Too many rectangles.");
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

        private int DistanceToCurrRect(Rectangle currRect)
        {
            return (int)Math.Sqrt(((currRect.X - Center.X) * (currRect.X - Center.X))
                             + ((currRect.Y - Center.Y) * (currRect.Y - Center.Y)));
        }

        private int MaxEnvirons()
        {
            return Layout.AllRectangles
                    .Select(DistanceToCurrRect)
                    .Concat(new[] {0}).Max();
        }

        [Test]
        public void PutNextRectangle_ManyRectangles_CorrectLocation()
        {
            for(var i = 0; i < 7; i++)
            {
                Layout.PutNextRectangle(new Size(100, 100));
            }
            MaxEnvirons().Should().BeLessThan(220);
        }

        [Test]
        public void PutNextRectangle_VeryBigRectangles_ThrowException()
        {
            Action res = () =>
            {
                Layout.PutNextRectangle(new Size(100000, 100000));
                Layout.PutNextRectangle(new Size(1, 1));
            };
            res.ShouldThrow<ArgumentException>().WithMessage("Too many rectangles.");
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