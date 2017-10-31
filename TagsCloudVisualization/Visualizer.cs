using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Visualizer
    {
        private string Path { get; }
        private Point Center { get; }

        public Visualizer(string path, Point center)
        {
            Path = path;
            Center = center;
        }

        public void Draw(List<Rectangle> rectangles)
        {
            var frameSize = Math.Max(Center.X, Center.Y)*2;

            var bitmap = new Bitmap(frameSize, frameSize);
            var gr = Graphics.FromImage(bitmap);

            var rectPen = new Pen(Color.Blue);
            var centerPen = new Pen(Color.Gray);

            gr.DrawLine(centerPen, Center.X - frameSize / 2, Center.Y,
                Center.X + frameSize / 2, Center.Y);
            gr.DrawLine(centerPen, Center.X, Center.Y - frameSize / 2,
                Center.X, Center.Y + frameSize / 2);

            foreach (var rectangle in rectangles)
                gr.DrawRectangle(rectPen, rectangle);

            gr.Dispose();
            bitmap.Save(Path);
        }
    }
}
