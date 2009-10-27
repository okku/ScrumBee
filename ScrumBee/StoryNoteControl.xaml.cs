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
	public partial class StoryNoteControl : UserControl, IBoardObject
	{
		private long _lastTickCount;
		private bool _textEntered;

		public StoryNoteControl()
		{
			InitializeComponent();
			MouseLeftButtonDown += StoryNote_MouseLeftButtonDown;
		}

		public void BoardHasFocus()
		{
			if(IsInEditMode())
			{
				_textEntered = true;
				blockText.Opacity = 1;
				blockText.Text = tbText.Text;
				blockText.Visibility = Visibility.Visible;
				tbText.Visibility = Visibility.Collapsed;
			}
		}

		public bool IsDraggable()
		{
			return true;
		}

		private bool IsInEditMode()
		{
			return blockText.Visibility == Visibility.Collapsed;
		}

		void StoryNote_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			long tickCountNow = DateTime.Now.Ticks;
			Point p = e.GetPosition(this);

			if(tickCountNow - _lastTickCount < 2310000)
			{
				blockText.Visibility = Visibility.Collapsed;
				tbText.Visibility = Visibility.Visible;
				tbText.Focus();
			}
			_lastTickCount = tickCountNow;
		}
	}
}