using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void SaveTagsCloud(Point center, int expectedQuantity, string path)
        {
            var rectList = new List<Size>();
            var rnd = new Random();

            for (var i = 0; i < expectedQuantity; i++)
                rectList.Add(new Size(
                    rnd.Next(10, 50), rnd.Next(10, 50)));

            SetRectanglesToLayout(center, rectList, expectedQuantity, path);
        }

        public static void SaveTagsCloud(Point center, Size rectSize, int expectedQuantity, string path)
        {
            var rectList = new List<Size>
            {
                rectSize
            };
            SetRectanglesToLayout(center, rectList, expectedQuantity, path);
        }

        private static void SetRectanglesToLayout(Point center, List<Size> rectList, int expectedQuantity, string path)
        {
            var layout = new CircularCloudLayouter(center);

            if(rectList.Count > 1)
                foreach (var size in rectList)
                    layout.PutNextRectangle(size);
            else
                for (var i = 0; i < expectedQuantity; i++)
                    layout.PutNextRectangle(rectList[0]);

            var visualize = new Visualizer(center);
            visualize.Draw(layout.AllRectangles).Save(path);
        }

        private static void Main(string[] args)
        {
            SaveTagsCloud(new Point(500, 500), 40, "test.bmp");
        }
    }
}
