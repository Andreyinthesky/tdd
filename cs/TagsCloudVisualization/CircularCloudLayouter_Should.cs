using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;
        private List<Rectangle> rectangles;
        private CircularCloudVisualizer visualizer;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter();
            rectangles = new List<Rectangle>();
            visualizer = new CircularCloudVisualizer(layouter);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Passed)
            {
                var directory = TestContext.CurrentContext.TestDirectory;
                var testName = TestContext.CurrentContext.Test.Name;
                var path = $"{directory}/{testName}.png";
                var image = visualizer.GetCloudImage();
                image.Save(path);
                TestContext.WriteLine($"Tag cloud visualization saved to file {path}");
            }
        }

        [Test]
        public void GetPlacedRectangles_AfterCreation_ListShouldBeEmpty()
        {
            layouter.GetPlacedRectangles()
                .Any()
                .Should()
                .BeFalse();
        }

        [TestCaseSource(nameof(MultipleRectanglesTestCase))]
        public void GetPlacedRectangles_WhenPutMultipleRectangles_ListShouldBeCorrect(Size[] rectangleSizes)
        {
            foreach (var size in rectangleSizes)
            {
                layouter.PutNextRectangle(size);
            }

            layouter.GetPlacedRectangles()
                .Count()
                .Should()
                .Be(rectangleSizes.Length);
        }

        [TestCase(1, 0, TestName = "width is positive, height is zero")]
        [TestCase(0, 1, TestName = "width is zero, height is positive")]
        [TestCase(0, 0, TestName = "width is zero, height is zero")]
        [TestCase(-1, 1, TestName = "width is negative, height is positive")]
        [TestCase(1, -1, TestName = "width is positive, height is negative")]
        [TestCase(-1, -1, TestName = "width is negative, height is negative")]
        public void PutNextRectangle_WhenSizeValuesLessOrEqualZero_ThrowsArgumentException(int sizeWidth,
            int sizeHeight)
        {
            var size = new Size(sizeWidth, sizeHeight);
            Action act = () => layouter.PutNextRectangle(size);
            act.ShouldThrow<ArgumentException>()
                .WithMessage($"rectangleSize must have only positive values: {size}");
        }

        [Test]
        public void PutNextRectangle_WhenSizeValuesArePositive_DoesNotThrowsArgumentException()
        {
            var size = new Size(1, 1);
            Action act = () => layouter.PutNextRectangle(size);
            act.ShouldNotThrow<ArgumentException>();
        }

        [TestCase(10, 2)]
        [TestCase(2, 10)]
        [TestCase(2, 2)]
        public void PutNextRectangle_WhenSizeIsCorrect_SizeValuesAreEqualReturnedRectangleValues(int sizeWidth,
            int sizeHeight)
        {
            var size = new Size(sizeWidth, sizeHeight);
            var rect = layouter.PutNextRectangle(size);
            rect.Width.Should()
                .Be(size.Width);
            rect.Height.Should()
                .Be(sizeHeight);
        }

        [Test]
        public void PutNextRectangle_WhenPutOneRectangle_RectangleShouldBeInCenter()
        {
            var firstRectangle = layouter.PutNextRectangle(new Size(128, 128));
            firstRectangle.GetCenterPoint()
                .Should()
                .Be(layouter.Center);
        }

        [Test]
        public void PutNextRectangle_WhenPutTwoRectangles_RectanglesDoNotIntersect()
        {
            var firstRectangle = layouter.PutNextRectangle(new Size(128, 128));
            var secondRectangle = layouter.PutNextRectangle(new Size(64, 64));
            firstRectangle.IntersectsWith(secondRectangle)
                .Should()
                .BeFalse();
        }

        [TestCaseSource(nameof(MultipleRectanglesTestCase))]
        public void PutNextRectangle_WhenPutMultipleRectangles_RectanglesDoNotIntersect(Size[] rectangleSizes)
        {
            foreach (var size in rectangleSizes)
            {
                var newRectangle = layouter.PutNextRectangle(size);
                rectangles.ForEach(rect => rect.IntersectsWith(newRectangle)
                    .Should()
                    .BeFalse());
                rectangles.Add(newRectangle);
            }
        }

        [TestCaseSource(nameof(MultipleRectanglesTestCase))]
        public void PutNextRectangle_WhenPutMultipleRectangles_RectanglesShouldBeTightly(Size[] rectangleSizes)
        {
            var cloudSquare = 0;

            foreach (var size in rectangleSizes)
            {
                var newRectangle = layouter.PutNextRectangle(size);
                rectangles.ForEach(rect => rect.IntersectsWith(newRectangle)
                    .Should()
                    .BeFalse());
                cloudSquare += newRectangle.Width * newRectangle.Height;

                rectangles.Add(newRectangle);
            }

            var circleSquare = Math.PI * Math.Pow(layouter.CircleRadius, 2);
            (cloudSquare / circleSquare).Should()
                .BeGreaterThan(0.6);
        }

        [TestCaseSource(nameof(PerformanceTestCase)), Timeout(3000)]
        public void PutNextRectangle_WhenPutTooManyRectangles(Size[] rectangleSizes)
        {
            foreach (var rectangleSize in rectangleSizes)
            {
                layouter.PutNextRectangle(rectangleSize);
            }
        }

        private static IEnumerable MultipleRectanglesTestCase
        {
            get
            {
                var random = new Random();
                var rectangleSizes = new Size[100];

                for (int i = 0; i < rectangleSizes.Length; i++)
                {
                    rectangleSizes[i] = new Size(random.Next(200) + 32, random.Next(100) + 32);
                }

                yield return rectangleSizes;
            }
        }

        private static IEnumerable PerformanceTestCase
        {
            get
            {
                var random = new Random();
                var rectangleSizes = new Size[1000];

                for (int i = 0; i < rectangleSizes.Length; i++)
                {
                    rectangleSizes[i] = new Size(random.Next(200) + 1, random.Next(100) + 1);
                }

                yield return rectangleSizes;
            }
        }
    }
}