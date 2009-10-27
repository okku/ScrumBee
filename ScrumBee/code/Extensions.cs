using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Scrumboard.code
{
	public static class Extensions
	{
		public static Point TopLeft(this Rect r)
		{
			return new Point(r.Left, r.Top);
		}
		public static Point TopRight(this Rect r)
		{
			return new Point(r.Right,r.Top);
		}
		public static Point BottomLeft(this Rect r)
		{
			return new Point(r.Left, r.Bottom);
		}
		public static Point BottomRight(this Rect r)
		{
			return new Point(r.Right, r.Bottom);
		}

		public static bool ContainsPoint(this UserControl uc,Point p)
		{
			double x = Canvas.GetLeft(uc);
			double y = Canvas.GetTop(uc);
			double width = uc.Width;
			double height = uc.Height;
			Rect r = new Rect(x, y, width, height);
			return r.Contains(p);
		}

	}
}