using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class PointExtensionsSpecification
    {
        [Test]
        public void Subtract_ShouldWork()
        {
            var a = new Point(10, 20);
            var b = new Point(1, 2);
            a.Subtract(b).ShouldBeEquivalentTo(new Point(9, 18));
        }
		
        [Test]
        public void GetLength_ShouldWork()
        {
            new Point(3, 4).GetLength().ShouldBeEquivalentTo(5.0);
        }

        [TestCaseSource(nameof(GetAngle_Should_Source))]
        [DefaultFloatingPointTolerance(0.01)]
        public double GetAngle_Should(int x, int y)
        {
            return new Point(x, y).GetAngle();
        }

        private static IList<TestCaseData> GetAngle_Should_Source = new[]
        {
            new TestCaseData(0, 0).Returns(0),
            new TestCaseData(1, 0).Returns(0),
            new TestCaseData(0, 1).Returns(Math.PI / 2),
            new TestCaseData(-1, 0).Returns(Math.PI),
            new TestCaseData(0, -1).Returns(3 * Math.PI / 2),
            new TestCaseData(1, 1).Returns(Math.PI/4),
            new TestCaseData(-1, 1).Returns(3*Math.PI/4),
            new TestCaseData(-1, -1).Returns(5*Math.PI/4),
            new TestCaseData(1, -1).Returns(7*Math.PI/4),
            new TestCaseData((int)(1000*Math.Sqrt(3)/2), (int)(1000*1.0/2)).Returns(Math.PI/6),
            new TestCaseData((int)(1000*1.0/2), (int)(1000*Math.Sqrt(3)/2)).Returns(Math.PI/3),
        };
    }
}