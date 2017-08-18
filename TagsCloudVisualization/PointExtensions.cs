using System;
using System.Drawing;

namespace TagsCloudVisualization
{
	static class PointExtensions
	{
		public static Point Subtract(this Point thisPoint, Point thatPoint)
		{
			return new Point(thisPoint.X - thatPoint.X, thisPoint.Y - thatPoint.Y);
		}
		
		public static double GetLength(this Point thisPoint)
		{
			return Math.Sqrt(thisPoint.X * thisPoint.X + thisPoint.Y * thisPoint.Y);
		}

		public static double GetAngle(this Point thisPoint)
		{
			if (Math.Abs(thisPoint.X) < double.Epsilon)
			{
				if (thisPoint.Y > 0)
					return Math.PI / 2;
				if (thisPoint.Y < 0)
					return 3 * Math.PI / 2;
				return 0;
			}
			
			var angle = Math.Atan2(thisPoint.Y, thisPoint.X);
			if (angle < 0)
				angle += 2*Math.PI;
			return angle;
		}
	}
}