using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Visualizer
    {
        private string Path { get; }
        private Point Center { get; }
        private int FrameSize { get; }

        public Visualizer(string path, Point center)
        {
            Path = path;
            Center = center;
            FrameSize = GetFrameSize;
        }

        public Visualizer(string path, Point center, int frameSize)
        {
            Path = path;
            Center = center;
            FrameSize = frameSize;
        }

        private int GetFrameSize => Math.Max(Center.X, Center.Y) * 2;

        private void DrawAxis(Graphics gr, int frameSize)
        {
            var centerPen = new Pen(Color.Gray);

            gr.DrawLine(centerPen, Center.X - frameSize / 2, Center.Y,
             Center.X + frameSize / 2, Center.Y);
            gr.DrawLine(centerPen, Center.X, Center.Y - frameSize / 2,
                Center.X, Center.Y + frameSize / 2);
        }

        private void DrawAllRectangles(Graphics gr, List<Rectangle> rectangles)
        {
            var rectPen = new Pen(Color.Blue);

            foreach (var rectangle in rectangles)
                gr.DrawRectangle(rectPen, rectangle);
        }

        public void Draw(List<Rectangle> rectangles)
        {
            var bitmap = new Bitmap(FrameSize, FrameSize);
            var gr = Graphics.FromImage(bitmap);

            DrawAxis(gr, FrameSize);
            DrawAllRectangles(gr, rectangles);
            
            gr.Dispose();
            bitmap.Save(Path);
        }
    }
}
