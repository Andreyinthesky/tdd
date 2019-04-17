using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var rectanglesCount = 25;
            var bitmap = GetCloudImageFromRandomRectangles(rectanglesCount);
            bitmap.Save("2.png");
        }

        private static Bitmap GetCloudImageFromRandomRectangles(int rectanglesCount)
        {
            var visualizer = new CircularCloudVisualizer(new CircularCloudLayouter());
            var random = new Random();

            for (int i = 0; i < rectanglesCount; i++)
            {
                Size nextRectSize;

                do
                {
                    nextRectSize = new Size(random.Next(200) + 100, random.Next(100) + 100);
                } while (nextRectSize.Width < nextRectSize.Height * 2);

                visualizer.Layouter.PutNextRectangle(nextRectSize);
            }

            return visualizer.GetCloudImage();
        }
    }
}