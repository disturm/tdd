﻿namespace TagsCloudVisualization
{
	struct Point
	{
		public Point(double x, double y)
		{
			X = x;
			Y = y;
		}

		public double X { get; }
		public double Y { get; }

        public override string ToString()
        {
            return $"{nameof(Point)} {nameof(X)}={X}, {nameof(Y)}={Y}";
        }
    }
}