using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudVisualizer
    {
        public CircularCloudLayouter Layouter { get; }

        public CircularCloudVisualizer(CircularCloudLayouter layouter)
        {
            Layouter = layouter;
        }

        public Bitmap GetCloudImage()
        {
            var imageSize = СalculateImageSize();
            var image = new Bitmap(imageSize.Width, imageSize.Height);
            var graphics = Graphics.FromImage(image);
            DrawRectangles(graphics, imageSize);

            return image;
        }

        private void DrawRectangles(Graphics graphics, Size imageSize)
        {
            var pen = new Pen(Brushes.Black, 2);

            graphics.FillRectangle(Brushes.White, 0, 0, imageSize.Width, imageSize.Height);

            foreach (var rectangle in Layouter.GetPlacedRectangles())
            {
                var shiftedRect = ShiftRectangleToImageCenter(rectangle, imageSize);
                graphics.DrawRectangle(pen, shiftedRect);
            }
        }

        private Rectangle ShiftRectangleToImageCenter(Rectangle rectangle, Size imageSize)
        {
            var newX = rectangle.X + Layouter.Center.X + imageSize.Width / 2;
            var newY = rectangle.Y + Layouter.Center.Y + imageSize.Height / 2;

            return new Rectangle(newX, newY, rectangle.Width, rectangle.Height);
        }

        private Size СalculateImageSize()
        {
            var imageHeight = (int)(2 * Layouter.CircleRadius);
            var imageWidth = (int)(imageHeight * 1.5);

            return new Size(imageWidth, imageHeight);
        }
    }
}