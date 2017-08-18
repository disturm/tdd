using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
	{
		private readonly Point center;
		private readonly HashSet<Point> points = new HashSet<Point>();
		private readonly List<Rectangle> rectangles = new List<Rectangle>();

		public CircularCloudLayouter(Point center)
		{
			this.center = center;
			points.Add(center);
		}

		public Rectangle PutNextRectangle(Size rectangleSize)
		{
			var result = BuildNextRectangle(rectangleSize);
			points.UnionWith(result.GetPoints());
			rectangles.Add(result);
			return result;
		}

		private Rectangle BuildNextRectangle(Size rectangleSize)
		{
		    var sortedPoints = points
		        .OrderBy(p => p.Subtract(center).GetLength())
		        .ThenBy(p => p.Subtract(center).GetAngle())
                .ToList();

		    foreach (var point in sortedPoints)
		        foreach (var rectangle in GetAdjacentRectangles(point, rectangleSize))
		            if (rectangles.All(r => !r.IntersectsWith(rectangle)))
		                return rectangle;
		    return new Rectangle();
		}

		private static IEnumerable<Rectangle> GetAdjacentRectangles(Point point, Size size)
		{
			yield return new Rectangle(point.X, point.Y, size.Width, size.Height);
			yield return new Rectangle(point.X, point.Y - size.Height, size.Width, size.Height);
			yield return new Rectangle(point.X - size.Width, point.Y - size.Height, size.Width, size.Height);
			yield return new Rectangle(point.X - size.Width, point.Y, size.Width, size.Height);
		}
	}
}
