using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Visualizer
    {
        private string Path { get; }
        private Point Center { get; }
        private int FrameSize { get; set; }

        public Visualizer(string path, Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("Координаты центра должны быть больше нуля либо равны нулю.");
            Center = center;
            Path = path;
            FrameSize = 0;
        }

        public Visualizer(string path, Point center, int frameSize)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("Координаты центра должны быть больше нуля либо равны нулю.");
            if (frameSize <= 0)
                throw new ArgumentException("Размер изображения должен быть больше нуля.");
            Path = path;
            Center = center;
            FrameSize = frameSize;
        }

        private int DistanceBetweenPoints(Point p1, Point p2)
        {
            return (int)Math.Sqrt(((p1.X - p2.X) * (p1.X - p2.X))
                                  + ((p1.Y - p2.Y) * (p1.Y - p2.Y)));
        }

        private void SetFrameSize()
        {
           FrameSize = Math.Max(Center.X, Center.Y) * 2;
        }

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
            if(FrameSize == 0)
                SetFrameSize();

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
