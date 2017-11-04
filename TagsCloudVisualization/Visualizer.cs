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
            if (GetFrameSize(center) <= 0)
                throw new ArgumentException("Центр должен быть больше нуля.");
            Center = center;
            Path = path;
            FrameSize = GetFrameSize(center);
        }

        public Visualizer(string path, Point center, int frameSize)
        {
            if (frameSize <= 0)
                throw new ArgumentException("Центр должен быть больше нуля.");
            Path = path;
            Center = center;
            FrameSize = frameSize;
        }

        private int GetFrameSize(Point center) => Math.Max(center.X, center.Y) * 2;

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
            using (var gr = Graphics.FromImage(bitmap))
            {
                DrawAxis(gr, FrameSize);
                DrawAllRectangles(gr, rectangles);
            }
            bitmap.Save(Path);
        }
    }
}
