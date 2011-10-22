using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using RobotInitial.View;

namespace RobotInitial.ViewModel
{
	class LoopControlBlockViewModel : ControlBlockViewModel
	{
		private ObservableCollection<UIElement> _children = new ObservableCollection<UIElement>();
		private double _mainHeight = 100; // Default maximum 
		private double _mainWidth = 75;
		
		public double Height { get; set; } 
		public double Width { get; set; }
		public Thickness StackMargin { 
			get { 
				if(Children.Count == 0) return new Thickness(0,25,0,25);
				return new Thickness(25,25,25,25);
			} 
		}
		public double MainWidth {
			get {
				if (Children.Count == 0) return _mainWidth;
				return Double.NaN;
			}
			set {
				_mainWidth = value;
			}
		}
		
		public double MainHeight {
			get {
				if (Children.Count == 0) return _mainHeight;
				return Double.NaN;
			}
			set {
				_mainHeight = value;
			}
		}
		
		public LoopControlBlockViewModel()
		{
			Type = "Loop";

			// Set the default Height and Width
			Height = 75;
			Width = 75;
			
			Rectangle r = new Rectangle();
			r.Fill = Brushes.Black;
			r.Width = 75;
			r.Height = 75;
			r.Margin = new Thickness(25,0,25,0);
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

			//AddChildBlock(r);
			//AddChildBlock(v);
			//AddChildBlock(z);
		}
		
		public void ExpandControl() {
			MessageBox.Show("Count is currently: " + Children.Count);
		}

		public ObservableCollection<UIElement> Children
		{
			get { return _children; }
		}

		public void AddChildBlock(FrameworkElement element)
		{
			if (Children.Count == 0)
			{
				ArrowConnector connector1 = new ArrowConnector();
				connector1.Margin = new Thickness(25, 0, 0, 0);
				_children.Add(connector1);
				element.Margin = new Thickness(0, 0, 0, 0);
				_children.Add(element);
				ArrowConnector connector2 = new ArrowConnector();
				connector2.Margin = new Thickness(0, 0, 25, 0);
				_children.Add(connector2);
			}
			else
			{
				element.Margin = new Thickness(-25, 0, 50, 0);
				ArrowConnector connector = new ArrowConnector();
				connector.Margin = new Thickness(-75, 0, 0, 0);
				_children.Add(element);
				_children.Add(connector);
			}
		}

	}
}
