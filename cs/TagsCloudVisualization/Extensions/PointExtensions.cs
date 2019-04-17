using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class PointExtensions
    {
        public static double GetDistanceTo(this Point from, Point to)
        {
            var res = new Point(to.X - from.X, to.Y - from.Y);

            return Math.Sqrt(Math.Pow(res.X, 2) + Math.Pow(res.Y, 2));
        }
    }
}