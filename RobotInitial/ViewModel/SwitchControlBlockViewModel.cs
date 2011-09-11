using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System.Windows.Media;

namespace RobotInitial.ViewModel
{
	class SwitchControlBlockViewModel : ControlBlockViewModel
	{
		
		public const int ORIENTATION_TOP = 1;
		public const int ORIENTATION_BOTTOM = 0;
		private ObservableCollection<UIElement> _childrenTop = new ObservableCollection<UIElement>();
		private ObservableCollection<UIElement> _childrenBottom = new ObservableCollection<UIElement>();

		public SwitchControlBlockViewModel() {
			Type = "Switch";


			Rectangle r = new Rectangle();
			r.Fill = Brushes.Black;
			r.Width = 75;
			r.Height = 75;
			r.Margin = new Thickness(25, 0, 25, 0);
			r.HorizontalAlignment = HorizontalAlignment.Center;
			r.VerticalAlignment = VerticalAlignment.Center;

			Rectangle v = new Rectangle();
			v.Fill = Brushes.Red;
			v.Width = 75;
			v.Height = 75;
			v.Margin = new Thickness(25, 0, 25, 0);
			v.HorizontalAlignment = HorizontalAlignment.Center;
			v.VerticalAlignment = VerticalAlignment.Center;

			Rectangle z = new Rectangle();
			z.Fill = Brushes.Green;
			z.Width = 75;
			z.Height = 75;
			z.Margin = new Thickness(25, 0, 25, 0);
			z.HorizontalAlignment = HorizontalAlignment.Center;
			z.VerticalAlignment = VerticalAlignment.Center;

			AddChildBlock(r,1);
			AddChildBlock(v,0);
			AddChildBlock(z,0);

		}

		public Thickness TopStackMargin
		{
			get
			{
				if (ChildrenTop.Count == 0 && ChildrenBottom.Count == 0) return new Thickness(0, 25, 0, 25);
				return new Thickness(25, 25, 25, 12.5);
			}
		}

		public Thickness BottomStackMargin
		{
			get
			{
				if (ChildrenTop.Count == 0 && ChildrenBottom.Count == 0) return new Thickness(0, 25, 0, 25);
				return new Thickness(25, 12.5, 25, 25);
			}
		}

		public ObservableCollection<UIElement> ChildrenTop
		{
			get { return _childrenTop; }
		}

		public ObservableCollection<UIElement> ChildrenBottom
		{
			get { return _childrenBottom; }
		}

		public void AddChildBlock(FrameworkElement element, int orientation)
		{
			if(orientation == ORIENTATION_TOP) {
				Console.Write("BLOCK ADD ");
				if (ChildrenTop.Count == 0)
				{
					element.Margin = new Thickness(50, 0, 50, 0);
				}
				else
				{
					element.Margin = new Thickness(-25, 0, 50, 0);
				}
				// Add a child to the collection
				_childrenTop.Add(element);
			}
			if (orientation == ORIENTATION_BOTTOM)
			{
				if (ChildrenBottom.Count == 0)
				{
					element.Margin = new Thickness(50, 0, 50, 0);
				}
				else
				{
					element.Margin = new Thickness(-25, 0, 50, 0);
				}
				// Add a child to the collection
				_childrenBottom.Add(element);
			}

			
		}
	}
}
