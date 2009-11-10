using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Ria.Data;
using System.Windows.Shapes;
using ScrumBee.DomainServices.Web;
using ScrumBee.Model;
using Scrumboard.code;

namespace ScrumBee
{
	public partial class MainPage : UserControl
	{
		private UserControl _dragged = null;
		private Point _oldPosition = new Point(0, 0);
		private List<UserControl> _objects = new List<UserControl>();

		public MainPage()
		{
			InitializeComponent();

			MouseMove += Page_MouseMove;
			MouseLeftButtonUp += Page_MouseLeftButtonUp;
			MouseLeftButtonDown += new MouseButtonEventHandler(Page_MouseLeftButtonDown);
			
			// Testing RIA
			var context = new ScrumBeeContext();
			var query = from s in context.GetStoriesQuery()
						where s.Name.Length > 0
						select s;
			LoadOperation<Story> loadOperation = context.Load(query, StoriesLoadedCallback, null);
			
		}

		private void StoriesLoadedCallback(LoadOperation<Story> lo)
		{
			if(!lo.HasError)
			{
				foreach (var story in lo.Entities)
				{
					Debug.WriteLine(story.Name);
				}
			}
			else
			{
				throw lo.Error;
			}
		}

		void Page_MouseMove(object sender, MouseEventArgs e)
		{
			Point now = e.GetPosition(this);
			double xdiff = now.X - _oldPosition.X;
			double ydiff = now.Y - _oldPosition.Y;
			_oldPosition = now;

			if(_dragged != null)
			{
				if(((IBoardObject)_dragged).IsDraggable())
				{
					Canvas.SetLeft(_dragged, Canvas.GetLeft(_dragged) + xdiff);
					Canvas.SetTop(_dragged, Canvas.GetTop(_dragged) + ydiff);
				}
			}
		}

		void Page_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			foreach(UserControl uc in _objects)
			{
				Point p = e.GetPosition(this);
				if(!uc.ContainsPoint(p))
				{
					IBoardObject ib = (IBoardObject)uc;
					ib.BoardHasFocus();
				}
			}
		}

		void Page_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if(_dragged != null)
			{
				//Set correct Z-index
				Rect r = new Rect(Canvas.GetLeft(_dragged), Canvas.GetTop(_dragged), _dragged.Width, _dragged.Height);
				var t = from uc in _objects
				        where uc != _dragged && (
				                                	uc.ContainsPoint(r.TopLeft()) || uc.ContainsPoint(r.TopRight()) ||
				                                	uc.ContainsPoint(r.BottomLeft()) || uc.ContainsPoint(r.BottomRight()))
				        orderby Canvas.GetZIndex(uc) descending
				        select uc;

				int z;
				if(t.Count() > 0)
					z = Canvas.GetZIndex(t.FirstOrDefault()) + 1;
				else
					z = 0;

				Canvas.SetZIndex(_dragged, z);

			}
			_dragged = null;
		}

		private void MoveControlToFront(UserControl toFront)
		{
			Canvas.SetZIndex(toFront, 1000);
		}

		private void PostitIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var pi = new PostItControl();
			Point now = e.GetPosition(this);
			_objects.Add(pi);
			Canvas.SetLeft(pi, now.X);
			Canvas.SetTop(pi, now.Y);
			pi.MouseLeftButtonDown += pilapp_MouseLeftButtonDown;
			LayoutRoot.Children.Add(pi);
			_dragged = pi;
			MoveControlToFront(_dragged);
		}

		private void StoryIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var o = new StoryNoteControl();

			Point now = e.GetPosition(this);
			_objects.Add(o);
			Canvas.SetLeft(o, now.X);
			Canvas.SetTop(o, now.Y);
			o.MouseLeftButtonDown += pilapp_MouseLeftButtonDown;
			LayoutRoot.Children.Add(o);
			_dragged = o;
			MoveControlToFront(o);
		}

		void pilapp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			_oldPosition = e.GetPosition(this);
			UserControl uc = (UserControl)sender;
			_dragged = uc;
			MoveControlToFront(uc);
		}

		private void TrashIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{

		}

	}
}