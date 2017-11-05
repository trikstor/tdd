using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Visualizer
    {
        private Point Center { get; }
        private int FrameSize { get; set; }

        public Visualizer(Point center)
        {
            if (center.X <= 0 || center.Y <= 0)
                throw new ArgumentException("Координаты центра должны быть больше нуля.");
            Center = center;;
            FrameSize = 0;
        }

        public Visualizer(Point center, int frameSize)
        {
            if (center.X <= 0 || center.Y <= 0)
                throw new ArgumentException("Координаты центра должны быть больше нуля.");
            if (frameSize <= 0)
                throw new ArgumentException("Размер изображения должен быть больше нуля.");
            Center = center;
            FrameSize = frameSize;
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

        private void AddAllRectanglesToImage(Graphics gr, List<Rectangle> rectangles)
        {
            var rectPen = new Pen(Color.Blue);

            foreach (var rectangle in rectangles)
                gr.DrawRectangle(rectPen, rectangle);
        }

        public Bitmap Draw(List<Rectangle> rectangles)
        {
            if(FrameSize == 0)
                SetFrameSize();

            var bitmap = new Bitmap(FrameSize, FrameSize);
            using (var gr = Graphics.FromImage(bitmap))
            {
                DrawAxis(gr, FrameSize);
                AddAllRectanglesToImage(gr, rectangles);
            }
            return bitmap;
        }
    }
}
