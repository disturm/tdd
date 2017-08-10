using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DrawingRectangle = System.Drawing.Rectangle;

namespace TagsCloudVisualization
{
	class Program
	{
		static void Main()
		{
		    var center = new Point(500, 500);
		    var random = new Random();
		    var layouter = new CircularCloudLayouter(center);

		    var rectangles =
		        from index in Enumerable.Range(3, 100)
		        let size = new Size(
		            3*random.NextDouble()*500.0/index,
		            3*random.NextDouble()*500.0/index)
		        select layouter.PutNextRectangle(size);

		    var draw = BuildDraw(Color.AliceBlue, Color.DarkOrange, 1000, 1000);
		    var bitmap = draw(rectangles.ToList(), center);
            bitmap.Save("result.bmp");
		}

        private delegate Bitmap Draw(IList<Rectangle> rectangles, Point center);

        private static Draw BuildDraw(Color backgroundColor, Color foregroundColor, int width, int height)
	        => (rectangles, center) =>
	        {
	            var bitmap = new Bitmap(width, height);
	            using (var graphics = Graphics.FromImage(bitmap))
	            {
	                graphics.Clear(backgroundColor);
	                var pen = new Pen(foregroundColor, 2.0f);
	                foreach (var rectangle in rectangles)
	                    graphics.DrawRectangle(pen, ToDrawingRectangle(rectangle));
	                graphics.DrawEllipse(new Pen(foregroundColor),
	                    (float) center.X - 2, (float) center.Y - 2, 4f, 4f);
	            }
	            return bitmap;
	        };

	    private static DrawingRectangle ToDrawingRectangle(Rectangle rectangle)
	        => new DrawingRectangle(
	            (int) rectangle.X,
	            (int) rectangle.Y,
	            (int) rectangle.Width,
	            (int) rectangle.Height);
    }
}
