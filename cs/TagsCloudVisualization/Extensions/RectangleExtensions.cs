using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static bool IntersectWith(this Rectangle rectangle, IEnumerable<Rectangle> others)
        {
            return others.Any(rectangle.IntersectsWith);
        }

        public static Point GetCenterPoint(this Rectangle rect)
        {
            return new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        }
    }
}