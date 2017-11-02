using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void SaveTagsCloudWithRandSizes(Point center, int expectedQuantity, string path)
        {
            var sizeOfRectangles = new Size[expectedQuantity];
            var rnd = new Random();

            for (var i = 0; i < expectedQuantity; i++)
                sizeOfRectangles[i] = new Size(
                    rnd.Next(10, 50), rnd.Next(10, 50));

            var layout = new CircularCloudLayouter(center);

            foreach (var size in sizeOfRectangles)
                layout.PutNextRectangle(size);

            var visualize = new Visualizer(path, center);
            visualize.Draw(layout.AllRectangles);
        }

        public static void SaveTagsCloudWithStaticSizes(Point center, Size rectSize, int expectedQuantity, string path)
        {
            var sizeOfRectangles = new Size[expectedQuantity];
            var rnd = new Random();

            var layout = new CircularCloudLayouter(center);

            foreach (var size in sizeOfRectangles)
                layout.PutNextRectangle(rectSize);

            var visualize = new Visualizer(path, center);
            visualize.Draw(layout.AllRectangles);
        }
        static void Main(string[] args)
        {
            SaveTagsCloudWithRandSizes(new Point(500, 500), 40, "test.bmp");
        }
    }
}
