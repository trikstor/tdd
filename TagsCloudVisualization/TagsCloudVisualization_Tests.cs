using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class TagsCloudVisualization_Tests
    {
        [TestCase(-10, 5, "Coordinates must be positive or zero", TestName = "Create a new layout with negative cordinate(s)")]
        public void CircularCloudLayouter_ThrowException_Constructor(int x, int y, string exMessage)
        {
            Action res = () => { new CircularCloudLayouter(new Point(x, y)); };
            res.ShouldThrow<ArgumentException>().WithMessage(exMessage);
        }

        [TestCase(15, 5, TestName = "Return rectangle")]
        public void PutNextRectangle_ReturnRightRectangle(int width, int height)
        {
            var layout = new CircularCloudLayouter(new Point(10, 10));

            var actual = layout.PutNextRectangle(new Size(width, height));
            var expected = new Rectangle(new Point(10, 10), new Size(width, height));

            actual.ShouldBeEquivalentTo(expected, options =>
                options.ExcludingNestedObjects());
        }

        [TestCase(0, 5, "Size must be positive", 
            TestName = "Create a new rectangle with negative or zero size")]
        public void PutNextRectangle_ThrowException(int width, int height, string exMessage)
        {
            var layout = new CircularCloudLayouter(new Point(10, 10));

            Action res = () => { layout.PutNextRectangle(new Size(width, height));};
            res.ShouldThrow<ArgumentException>().WithMessage(exMessage);
        }

        [Test]
        public void PutNextRectangle_QuantityOfRectangles_EqualsQuantity()
        {
            var expectedQuantity = 5;
            Size[] sizeOfRectangles = new Size[expectedQuantity];

            for (var i = 0; i < expectedQuantity; i++)
                sizeOfRectangles[i] = new Size(i + 1, i + 2);
            
            var layout = new CircularCloudLayouter(new Point(12, 10));

            foreach (var size in sizeOfRectangles)
            {
                layout.PutNextRectangle(size);
            }

            layout.AllRectangles.Count.Should().Be(expectedQuantity);
        }

        public static bool RectanglesNotOverlap(List<Rectangle> rectangles)
        {
            foreach(var currRect in rectangles)
                if (rectangles.Any(rect => rect.IntersectsWith(currRect) && rect.Size != currRect.Size))
                    return false;
            return true;
        }

        [TestCase(5, TestName = "Немного прямоугольников")]
        public void PutNextRectangle_NotOverlapOfRectangles(int expectedQuantity)
        {
            Size[] sizeOfRectangles = new Size[expectedQuantity];
            Random rnd = new Random();

            for (var i = 0; i < expectedQuantity; i++)
                sizeOfRectangles[i] = new Size(
                    i + rnd.Next(10, 300), i + rnd.Next(10, 300));

            var layout = new CircularCloudLayouter(new Point(500, 500));

            foreach (var size in sizeOfRectangles)
            {
                layout.PutNextRectangle(size);
            }

            RectanglesNotOverlap(layout.AllRectangles).Should().BeTrue();
        }

        [Test]
        public void PutNextRectangle_TooManyRectangles_ThrowException()
        {
            const int expectedQuantity = 100;

            Size[] sizeOfRectangles = new Size[expectedQuantity];
            Random rnd = new Random();

            for (var i = 0; i < expectedQuantity; i++)
                sizeOfRectangles[i] = new Size(
                    i + rnd.Next(10, 300), i + rnd.Next(10, 300));

            var layout = new CircularCloudLayouter(new Point(500, 500));

            Action res = () =>
            {
                foreach (var size in sizeOfRectangles)
                {
                layout.PutNextRectangle(size);
                }
            };

            res.ShouldThrow<ArgumentException>().WithMessage("Too many rectangles.");
        }

        [Test]
        public void PutNextRectangle_OneRectangle_CenterOfRectСalibration()
        {
            var layout = new CircularCloudLayouter(new Point(500, 500));
            layout.PutNextRectangle(new Size(200, 100));

            layout.AllRectangles[0].Location.Should().Be(new Point(400, 450));
        }
    }
}
