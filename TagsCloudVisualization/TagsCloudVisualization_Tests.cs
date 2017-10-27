using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
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
            var layout = new CircularCloudLayouter(new Point(12, 10));

            var actual = layout.PutNextRectangle(new Size(width, height));
            var expected = new Rectangle(new Point(10, 10), new Size(width, height));

            actual.ShouldBeEquivalentTo(expected, options =>
                options.ExcludingNestedObjects());
        }

        /*
        [TestCase(0, 5, "Size must be positive", 
            TestName = "Create a new rectangle with negative or zero size")]
        public void PutNextRectangle_ThrowException(int width, int height, string exMessage)
        {
            var layout = new CircularCloudLayouter(new Point(10, 10));

            Action res = () => { layout.PutNextRectangle(new Size(width, height));};
            res.ShouldThrow<ArgumentException>().WithMessage(exMessage);
        }
        */
    }
}
