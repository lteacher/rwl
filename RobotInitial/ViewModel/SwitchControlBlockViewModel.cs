using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System.Windows.Media;
using System.ComponentModel;

namespace RobotInitial.ViewModel
{
	class SwitchControlBlockViewModel : ControlBlockViewModel, INotifyPropertyChanged
	{
		
		public const int ORIENTATION_TOP = 1;
		public const int ORIENTATION_BOTTOM = 0;
		private double _stackHeight = 225;
		private double topMaxChildHeight = 0;
		private double bottomMaxChildHeight = 0;
		private ObservableCollection<FrameworkElement> _childrenTop = new ObservableCollection<FrameworkElement>();
		private ObservableCollection<FrameworkElement> _childrenBottom = new ObservableCollection<FrameworkElement>();

		public SwitchControlBlockViewModel() {
			Type = "Switch";
		}

		public Thickness TopStackMargin
		{
			get
			{
				// If the bottom children stack is empty OR both child heights are equal
				// OR if the top margin is the max, set the default margin
				if (ChildrenBottom.Count == 0 ||
					bottomMaxChildHeight == topMaxChildHeight || 
					topMaxChildHeight > bottomMaxChildHeight) return new Thickness(0, 25, 0, 12.5);

				// The bottom is not empty. If its max child height is greater than
				// the top max child height, use that child for setting the margin
				if (bottomMaxChildHeight > topMaxChildHeight) {
					double margin = (bottomMaxChildHeight - topMaxChildHeight) / 2 + 25;
					Console.WriteLine("===>> TopStack: Bottom greater, margin is: {0}", margin);
					return new Thickness(0, margin, 0, margin - 12.5);
				}
				// Otherwise return the margin using the top child height
				else
				{
					double margin = (topMaxChildHeight - bottomMaxChildHeight) / 2 + 25;
					Console.WriteLine("===>> TopStack: Top greater, margin is: {0}", margin);
					return new Thickness(0, margin, 0, margin - 12.5);
				}
			}
		}

		public Thickness BottomStackMargin
		{
			get
			{
				// If the top children stack is empty OR both child heights are equal
				// OR if the bottom margin is the max, set the default margin
				if (ChildrenTop.Count == 0 || 
					bottomMaxChildHeight == topMaxChildHeight ||
					bottomMaxChildHeight > topMaxChildHeight) return new Thickness(0, 12.5, 0, 25);

				// The top is not empty. If its max child height is greater than
				// the top max child height, use that child for setting the margin
				if (topMaxChildHeight > bottomMaxChildHeight) {
					double margin = (topMaxChildHeight - bottomMaxChildHeight) / 2 + 25;
					//Console.WriteLine("===>> BottomStack: Top greater, margin is: {0}", margin);
					//Console.WriteLine("topMaxChildHeight: {0}, bottomMaxChildHeight: {1}", topMaxChildHeight, bottomMaxChildHeight);
					return new Thickness(0, margin - 12.5, 0, margin);
				}

				// Otherwise return the margin using the bottom child height
				else {
					double margin = (bottomMaxChildHeight - topMaxChildHeight) / 2 + 25;
					//Console.WriteLine("===>> BottomStack: Bottom greater, margin is: {0}", margin);
					return new Thickness(0, margin - 12.5, 0, margin);
				}
			}
		}

		public double StackHeight {
			get
			{
				return _stackHeight;
			}
			set
			{
				_stackHeight = value;
				NotifyPropertyChanged("StackHeight");
			}
		}

		public ObservableCollection<FrameworkElement> ChildrenTop
		{
			get { return _childrenTop; }
		}

		public ObservableCollection<FrameworkElement> ChildrenBottom
		{
			get { return _childrenBottom; }
		}

		public void AddChildBlock(FrameworkElement element, int orientation)
		{
			element.Measure(new Size(element.MaxWidth, element.MaxHeight));

			if(orientation == ORIENTATION_TOP) {
				if (ChildrenTop.Count == 0)
				{
					element.Margin = new Thickness(50, 0, 50, 0);
				}
				else
				{
					element.Margin = new Thickness(-25, 0, 50, 0);
				}

				// Check the new element's size to possibly set a new max child height
				if (element.DesiredSize.Height > topMaxChildHeight) topMaxChildHeight = element.DesiredSize.Height;

				// Add a child to the collection
				ChildrenTop.Add(element);
				NotifyPropertyChanged("BottomStackMargin");
				NotifyPropertyChanged("TopStackMargin");
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

				// Check the new element's size to possibly set a new max child height
				if (element.DesiredSize.Height > bottomMaxChildHeight) bottomMaxChildHeight = element.DesiredSize.Height;

				// Add a child to the collection
				ChildrenBottom.Add(element);
				NotifyPropertyChanged("TopStackMargin");
				NotifyPropertyChanged("BottomStackMargin");
			}
		}

		// This horrible method checks children sizes to update when
		// internal collections change (called in ExpandingDrop, ONE of these 
		// sets WILL be updating, and it should only be once)
		public void CheckChildrenSizes() {
			// Check the top children
			foreach (FrameworkElement element in ChildrenTop)
			{
				Console.WriteLine("** Top ** Max Height: {0}, Replacement: {1}", topMaxChildHeight, element.RenderSize.Height);

				if(element.RenderSize.Height > topMaxChildHeight) {

					topMaxChildHeight = element.RenderSize.Height;
					NotifyPropertyChanged("TopStackMargin");
					NotifyPropertyChanged("BottomStackMargin");
					return;
				}
			}
			// Check the bottom children
			foreach (FrameworkElement element in ChildrenBottom)
			{
				Console.WriteLine("** Bottom ** Max Height: {0}, Replacement: {1}", bottomMaxChildHeight, element.DesiredSize.Height);
				if (element.RenderSize.Height > bottomMaxChildHeight)
				{
					
					bottomMaxChildHeight = element.RenderSize.Height;
					NotifyPropertyChanged("TopStackMargin");
					NotifyPropertyChanged("BottomStackMargin");
					return;
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Notifies the property changed.
		/// </summary>
		/// <param name="property">The property.</param>
		private void NotifyPropertyChanged(string property)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}
	}
}
