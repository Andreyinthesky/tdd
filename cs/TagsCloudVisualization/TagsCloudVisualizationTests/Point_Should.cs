using NUnit.Framework;
using System;
using System.Collections;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Point_Should
    {
        [TestCaseSource(nameof(GetCenterPointTestCases))]
        public double GetDistanceTo_ShouldBeCorrect(Point from, Point to)
        {
            return from.GetDistanceTo(to);
        }

        private static IEnumerable GetCenterPointTestCases
        {
            get
            {
                yield return new TestCaseData(new Point(0, 0), new Point(0, 0)).Returns(0);

                yield return new TestCaseData(new Point(0, 0), new Point(0, 1)).Returns(1);
                yield return new TestCaseData(new Point(0, 0), new Point(1, 0)).Returns(1);
                yield return new TestCaseData(new Point(0, 0), new Point(-1, 0)).Returns(1);
                yield return new TestCaseData(new Point(0, 0), new Point(0, -1)).Returns(1);

                yield return new TestCaseData(new Point(0, 0), new Point(1, 1)).Returns(Math.Sqrt(2));
                yield return new TestCaseData(new Point(0, 0), new Point(-1, 1)).Returns(Math.Sqrt(2));
                yield return new TestCaseData(new Point(0, 0), new Point(1, -1)).Returns(Math.Sqrt(2));
                yield return new TestCaseData(new Point(0, 0), new Point(-1, -1)).Returns(Math.Sqrt(2));

                yield return new TestCaseData(new Point(0, 0), new Point(1, 2)).Returns(Math.Sqrt(5));
                yield return new TestCaseData(new Point(0, 0), new Point(-1, 2)).Returns(Math.Sqrt(5));
                yield return new TestCaseData(new Point(0, 0), new Point(1, -2)).Returns(Math.Sqrt(5));
                yield return new TestCaseData(new Point(0, 0), new Point(-1, -2)).Returns(Math.Sqrt(5));
            }
        }
    }
}