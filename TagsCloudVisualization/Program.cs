using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var expectedQuantity = 40;
            Size[] sizeOfRectangles = new Size[expectedQuantity];
            Random rnd = new Random();

            for (var i = 0; i < expectedQuantity; i++)
                sizeOfRectangles[i] = new Size(
                    rnd.Next(10, 50), rnd.Next(10, 50));

            var layout = new CircularCloudLayouter(new Point(500, 500));

            foreach (var size in sizeOfRectangles)
            {
                layout.PutNextRectangle(size);
            }

            layout.Drawer("test.bmp");
        }
    }
}
