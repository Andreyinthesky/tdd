using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    /// <summary>
    /// Provides Archimedean spiral
    /// </summary>
    public class Spiral
    {
        private readonly double spiralStep;
        private readonly Point spiralCenter;

        public Spiral(Point spiralCenter, double spiralStep)
        {
            this.spiralStep = spiralStep;
            this.spiralCenter = spiralCenter;
        }

        public IEnumerable<Point> GetSpiralPoints()
        {
            double currentSpiralAngle = 0;

            while (true)
            {
                var p = spiralStep / (2 * Math.PI) * currentSpiralAngle;
                var dx = Math.Cos(currentSpiralAngle) * p;
                var dy = Math.Sin(currentSpiralAngle) * p;

                yield return new Point(spiralCenter.X + (int)dx, spiralCenter.Y + (int)dy);

                currentSpiralAngle += 0.05;
            }
        }
    }
}