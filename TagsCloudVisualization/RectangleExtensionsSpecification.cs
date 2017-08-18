using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
	[TestFixture]
	class RectangleExtensionsSpecification
	{
		[TestCase(0, 1, ExpectedResult = 0, TestName = "Zero_WhenHeightIsZero")]
		[TestCase(1, 0, ExpectedResult = 0, TestName = "Zero_WhenWidthIsZero")]
		[TestCase(3, 1, ExpectedResult = 3, TestName = "Width_WhenHeightIsOne")]
		[TestCase(1, 3, ExpectedResult = 3, TestName = "Height_WhenWidthIsOne")]
		[TestCase(2, 3, ExpectedResult = 6, TestName = "MultiplicationOfWidthAndHeight_WhenPositive")]
		public double GetSquare_ShouldReturn(int width, int height)
		{
			return new Rectangle(0, 0, width, height).GetSquare();
		}

		[TestCase(-1, 1, TestName = "WhenWidthIsNegative")]
		[TestCase(1, -1, TestName = "WhenHeightIsNegative")]
		public void GetSquare_ShouldThrow(int width, int height)
		{
			Action action = () => new Rectangle(0, 0, width, height).GetSquare();
			action.ShouldThrow<InvalidOperationException>();
		}

		[TestCaseSource(nameof(IntersectWith_ShouldBe_Source))]
		public Rectangle? IntersectWith_ShouldBe(Rectangle thisRectangle, Rectangle thatRectangle)
		{
			return thisRectangle.IntersectWith(thatRectangle);
		}

		const int left = 10;
		const int top = 20;
		const int width = 30;
		const int height = 40;
		const int widthBig = 300;
		const int heightBig = 400;
		const int epsilon = 1;
		const int shift = 10;

        //NOTE: на самом деле эти тесты не нужны, т.к. есть встроенная реализация пересечения
        private static IEnumerable<TestCaseData> IntersectWith_ShouldBe_Source => GetSimpleCases()
			.Concat(GetHorizontalCases())
			.Concat(GetVerticalCases())
			.Concat(GetInsideCases())
			.Concat(GetIntersectionCases());

		//NOTE: после написания реализации под SimpleCases приходится сделать "прыжок веры" - написать много тестов и завершить реализацию
		private static IEnumerable<TestCaseData> GetSimpleCases()
		{
			yield return new TestCaseData(
					new Rectangle(left, top, 0, 0),
					new Rectangle(left + 1, top + 1, 0, 0))
				.Returns(null)
				.SetName("Null_WhenDifferentPoints");

			yield return new TestCaseData(
					new Rectangle(left, top, 0, 0),
					new Rectangle(left, top, 0, 0))
				.Returns(new Rectangle(left, top, 0, 0))
				.SetName("Point_WhenSamePoints");

			yield return new TestCaseData(
					new Rectangle(left, top, width, height),
					new Rectangle(left, top, width, height))
				.Returns(new Rectangle(left, top, width, height))
				.SetName("Rectangle_WhenSameRectangles");
		}

		private static IEnumerable<TestCaseData> GetInsideCases()
		{
			var smaller = new Rectangle(left + shift, top + shift, width, height);
			var bigger = new Rectangle(left, top, widthBig, heightBig);

			yield return new TestCaseData(smaller, bigger).Returns(smaller)
				.SetName("First_WhenFirstInsideSecond");

			yield return new TestCaseData(bigger, smaller).Returns(smaller)
				.SetName("Second_WhenSecondInsideFirst");
		}

		private static IEnumerable<TestCaseData> GetIntersectionCases()
		{
			var first = new Rectangle(left, top, width, height);
			var second = new Rectangle(left + width / 2, top + height / 2, widthBig, heightBig);
			var expected = new Rectangle(left + width / 2, top + height / 2, width - width / 2, height - height / 2);

			yield return new TestCaseData(first, second).Returns(expected)
				.SetName("IntersectionRectangle_WhenIntersected");

			yield return new TestCaseData(second, first).Returns(expected)
				.SetName("IntersectionRectangle_WhenIntersected");
		}

		private static IEnumerable<TestCaseData> GetHorizontalCases()
		{
			yield return new TestCaseData(
					new Rectangle(left, top, width, height),
					new Rectangle(left + width, top, widthBig, height))
				.Returns(new Rectangle(left + width, top, 0, height))
				.SetName("RightBorder_WhenSecondTouchRightSide");

			yield return new TestCaseData(
					new Rectangle(left, top, width, height),
					new Rectangle(left + width + epsilon, top, widthBig, height))
				.Returns(null)
				.SetName("Null_WhenSecondRighter");

			yield return new TestCaseData(
					new Rectangle(left, top, width, height),
					new Rectangle(left - widthBig, top, widthBig, height))
				.Returns(new Rectangle(left, top, 0, height))
				.SetName("LeftBorder_WhenSecondTouchLeftSide");

			yield return new TestCaseData(
					new Rectangle(left, top, width, height),
					new Rectangle(left - widthBig - epsilon, top, widthBig, height))
				.Returns(null)
				.SetName("Null_WhenSecondLefter");
		}

		private static IEnumerable<TestCaseData> GetVerticalCases()
		{
			yield return new TestCaseData(
					new Rectangle(left, top, width, height),
					new Rectangle(left, top + height, width, heightBig))
				.Returns(new Rectangle(left, top + height, width, 0))
				.SetName("BottomBorder_WhenSecondTouchBottomSide");

			yield return new TestCaseData(
					new Rectangle(left, top, width, height),
					new Rectangle(left, top + height + epsilon, width, heightBig))
				.Returns(null)
				.SetName("Null_WhenSecondBottomer");

			yield return new TestCaseData(
					new Rectangle(left, top, width, height),
					new Rectangle(left, top - heightBig, width, heightBig))
				.Returns(new Rectangle(left, top, width, 0))
				.SetName("TopBorder_WhenSecondTouchTopSide");


			yield return new TestCaseData(
					new Rectangle(left, top, width, height),
					new Rectangle(left, top - heightBig - epsilon, width, heightBig))
				.Returns(null)
				.SetName("Null_WhenSecondToper");
		}

		[Test]
		public void IsIntersectWith_ShouldBeTrue_WhenIntersected()
		{
			var first = new Rectangle(left, top, width, height);
			var second = new Rectangle(left + width / 2, top + height / 2, widthBig, heightBig);
			first.IntersectWith(second).Should().NotBeNull();

			first.IntersectsWith(second).Should().BeTrue();
		}

		[Test]
		public void IsIntersectWith_ShouldBeFalse_WhenNotIntersected()
		{
			var first = new Rectangle(left, top, width, height);
			var second = new Rectangle(left + 2*width, top + 2*height, widthBig, heightBig);
			first.IntersectWith(second).Should().BeNull();

			first.IntersectsWith(second).Should().BeFalse();
		}

		[Test]
		public void IsIntersectWith_ShouldBeFalse_WhenIntersectionWidthIsZero()
		{
			var first = new Rectangle(left, top, width, height);
			var second = new Rectangle(left + width, top , width, height);
			first.IntersectWith(second).Should().NotBeNull()
				.And.Match<Rectangle>(r => Math.Abs(r.Width) < double.Epsilon);

			first.IntersectsWith(second).Should().BeFalse();
		}

		[Test]
		public void GetPoints_ShouldReturnFourVertexes()
		{
			new Rectangle(1, 2, 3, 4).GetPoints().Should()
				.BeEquivalentTo(new Point(1, 2), new Point(4, 2), new Point(1, 6), new Point(4, 6));
		}
	}
}