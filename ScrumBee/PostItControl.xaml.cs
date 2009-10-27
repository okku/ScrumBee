using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Scrumboard.code;

namespace ScrumBee
{
	public partial class PostItControl : UserControl, IBoardObject
	{
		private double _SP;
		private long _lastTickCount;
		private int _lastZIndex;
		private bool _textEntered = false;

		public PostItControl()
		{
			InitializeComponent();

			var st = new ScaleTransform { ScaleX = 1, ScaleY = 1 };
			RenderTransform = st;
			MouseLeftButtonDown += PILapp_MouseLeftButtonDown;
		}

		public bool IsKeypadVisible()
		{
			return keypad.Visibility == Visibility.Visible;
		}

		public bool IsDraggable()
		{
			return !IsKeypadVisible() && !IsInEditMode();
		}

		private bool IsInEditMode()
		{
			return blockText.Visibility == Visibility.Collapsed;
		}

		void PILapp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			long tickCountNow = DateTime.Now.Ticks;
			Point p = e.GetPosition(this);
			if(p.X > 60 && p.Y > 80)
			{
				keypad.Visibility = Visibility.Visible;
				_lastZIndex = Canvas.GetZIndex(this);
				Canvas.SetZIndex(this, 1000);
			}
			else
			{
				keypad.Visibility = Visibility.Collapsed;

				if(tickCountNow - _lastTickCount < 2310000)
				{
					blockText.Visibility = Visibility.Collapsed;
					tbText.Visibility = Visibility.Visible;
					tbText.Focus();
				}
			}
			_lastTickCount = tickCountNow;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Button b = sender as Button;
			if(b != null)
			{
				string s = b.Content.ToString();
				if(s == "+1")
				{
					_SP++;
					SetSPText();
				}
				else if(s.Contains("/"))
				{
					_SP += 0.25;
					SetSPText();
				}
				else
				{
					_SP = int.Parse(s);
					SetSPText();
				}
			}

		}

		private void SetSPText()
		{
			tbSP.Text = string.Format("{0} SP", _SP);
		}

		#region IBoardObject Members

		public void BoardHasFocus()
		{
			if(IsKeypadVisible())
			{
				keypad.Visibility = Visibility.Collapsed;
				Canvas.SetZIndex(this, _lastZIndex);
			}
			if(IsInEditMode())
			{
				_textEntered = true;
				blockText.Opacity = 1;
				blockText.Text = tbText.Text;
				blockText.Visibility = Visibility.Visible;
				tbText.Visibility = Visibility.Collapsed;
			}
		}

		#endregion	
	}
}