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
            var expectedQuantity = 20;
            Size[] sizeOfRectangles = new Size[expectedQuantity];
            Random rnd = new Random();

            for (var i = 0; i < expectedQuantity; i++)
                sizeOfRectangles[i] = new Size(
                    i + 50, i + 50);

            var layout = new CircularCloudLayouter(new Point(500, 500));

            foreach (var size in sizeOfRectangles)
            {
                layout.PutNextRectangle(size);
            }

            layout.Drawer("test");
        }
    }
}
