using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        public Point Center { get; }

        public double CircleRadius { get; private set; }

        private List<Rectangle> placedRectangles = new List<Rectangle>();
        private IEnumerator<Point> spiralEnumerator;

        public CircularCloudLayouter()
        {
            Center = new Point(0, 0);
            spiralEnumerator = new Spiral(Center, 1).GetSpiralPoints()
                .GetEnumerator();
        }

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            spiralEnumerator = new Spiral(Center, 1).GetSpiralPoints()
                .GetEnumerator();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            {
                throw new ArgumentException($"rectangleSize must have only positive values: {rectangleSize}");
            }

            var newRectangle = GenerateRectangle(rectangleSize);
            placedRectangles.Add(newRectangle);
            UpdateCircleRadius(newRectangle);

            return newRectangle;
        }

        public IEnumerable<Rectangle> GetPlacedRectangles()
        {
            foreach (var rectangle in placedRectangles)
            {
                yield return rectangle;
            }
        }

        private Rectangle GenerateRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle();

            while (spiralEnumerator.MoveNext())
            {
                var spiralPoint = spiralEnumerator.Current;
                rectangle = GetRectangleByCenterPoint(spiralPoint, rectangleSize);

                if (!rectangle.IntersectWith(placedRectangles))
                {
                    rectangle = ShiftRectangleToCloudCenter(rectangle);

                    break;
                }
            }

            return rectangle;
        }

        private Rectangle GetRectangleByCenterPoint(Point centerPoint, Size rectangleSize)
        {
            var northWestCornerLocation = new Point(centerPoint.X - rectangleSize.Width / 2,
                centerPoint.Y - rectangleSize.Height / 2);

            return new Rectangle(northWestCornerLocation, rectangleSize);
        }

        private Rectangle ShiftRectangleToCloudCenter(Rectangle rect)
        {
            return ShiftRectangleToCloudCenterAlongY(ShiftRectangleToCloudCenterAlongX(rect));
        }

        private Rectangle ShiftRectangleToCloudCenterAlongX(Rectangle rect)
        {
            var rectCenter = rect.GetCenterPoint();
            var dx = rectCenter.X - Center.X > 0 ? -1 : 1;

            while (Math.Abs(rectCenter.X - Center.X) > 0)
            {
                var newRect = GetRectangleByCenterPoint(new Point(rectCenter.X + dx, rectCenter.Y), rect.Size);

                if (newRect.IntersectWith(placedRectangles))
                {
                    break;
                }

                rectCenter = newRect.GetCenterPoint();
                rect = newRect;
            }

            return rect;
        }

        private Rectangle ShiftRectangleToCloudCenterAlongY(Rectangle rect)
        {
            var rectCenter = rect.GetCenterPoint();
            var dy = rectCenter.Y - Center.Y > 0 ? -1 : 1;

            while (Math.Abs(rectCenter.Y - Center.Y) > 0)
            {
                var newRect = GetRectangleByCenterPoint(new Point(rectCenter.X, rectCenter.Y + dy), rect.Size);

                if (newRect.IntersectWith(placedRectangles))
                {
                    break;
                }

                rectCenter = newRect.GetCenterPoint();
                rect = newRect;
            }

            return rect;
        }

        private void UpdateCircleRadius(Rectangle newRectangle)
        {
            var rectCenterPoint = newRectangle.GetCenterPoint();
            var mostDistancePointFromCenterX =
                Center.X - rectCenterPoint.X > 0 ? newRectangle.Left : newRectangle.Right;
            var mostDistancePointFromCenterY =
                Center.Y - rectCenterPoint.Y > 0 ? newRectangle.Top : newRectangle.Bottom;

            var currentDistance =
                new Point(mostDistancePointFromCenterX, mostDistancePointFromCenterY).GetDistanceTo(Center);
            CircleRadius = Math.Max(currentDistance, CircleRadius);
        }
    }
}