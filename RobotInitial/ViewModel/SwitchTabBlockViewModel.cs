using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Input;
using RobotInitial.View;

namespace RobotInitial.ViewModel
{
	class SwitchTabBlockViewModel : ControlBlockViewModel, INotifyPropertyChanged
	{
		private ObservableCollection<ObservableCollection<UIElement>> _cases = new ObservableCollection<ObservableCollection<UIElement>>();
		private ObservableCollection<double> _maxSizes = new ObservableCollection<double>();
		private int _caseIndex = 0;
		private double _mainHeight = 100; // Default maximum 
		private double _mainWidth = 75;
		private double _maximumElementSize = 0;
		
		public double Height { get; set; } 
		public double Width { get; set; }
		public Thickness StackMargin { 
			get {
				if(Children.Count == 0) { 
					return new Thickness(37.5, 25 + (_maximumElementSize - _maxSizes[_caseIndex])/2, 37.5, 25+(_maximumElementSize - _maxSizes[_caseIndex])/2);
				}else {
					return new Thickness(25, 25 + (_maximumElementSize - _maxSizes[_caseIndex]) / 2, 25, 25 + (_maximumElementSize - _maxSizes[_caseIndex]) / 2);
				}
			} 
		}

		public Brush TopButtonColour { get; set; }
		public double TopButtonOpacity { get; set; }
		public Brush BottomButtonColour { get; set; }
		public double BottomButtonOpacity { get; set; }

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

		public SwitchTabBlockViewModel()
		{
			Type = "Switch";

			// Set the default Height and Width
			Height = 75;
			Width = 75;

			// Initialise the cases, initially there is just 2 as default
			_cases.Add(new ObservableCollection<UIElement>());
			_cases.Add(new ObservableCollection<UIElement>());

			// Set the max sizes
			_maxSizes.Add(0);
			_maxSizes.Add(0);

			// Initialise the case control buttons
			TopButtonColour = Brushes.LawnGreen;
			TopButtonOpacity = 0.9;
			BottomButtonColour = Brushes.DarkGray;
			BottomButtonOpacity = 0.5;
		}
		
		public void ExpandControl() {
			MessageBox.Show("Count is currently: " + Children.Count);
		}

		public ObservableCollection<UIElement> Children
		{
			get { return _cases[_caseIndex]; }
		}

		public void AddChildBlock(FrameworkElement sourceView, FrameworkElement newElement, double xLocation)
		{

			newElement.Measure(new Size(newElement.MaxWidth, newElement.MaxHeight));

			// Set the ultime maximum element size if its ever increased
			if (newElement.DesiredSize.Height > _maximumElementSize) _maximumElementSize = newElement.DesiredSize.Height;

			// Set the case specific max element size
			if (newElement.DesiredSize.Height > _maxSizes[_caseIndex]) _maxSizes[_caseIndex] = newElement.DesiredSize.Height;
			if (Children.Count == 0)
			{
				Children.Add(new ArrowConnector());
				Children.Add(newElement);
				Children.Add(new ArrowConnector());
			}
			else
			{
				int dropIndex = 0;
				// Get the dropping location
				for (int i = 0; i < Children.Count - 1; i++)
				{
					if (Children[i].GetType() == typeof(ArrowConnector)) continue;

					Point childLeft = Children[i].TransformToAncestor(sourceView).Transform(new Point(0, 0));


					// For the first element
					if (i == 1)
					{
						if (xLocation < childLeft.X + Children[i].RenderSize.Width / 2)
						{
							dropIndex = 1;
							break;
						}
					}

					if (i == Children.Count - 2)
					{
						if (xLocation >= childLeft.X + Children[i].RenderSize.Width / 2)
						{
							dropIndex = Children.Count;
							break;
						}
					}

					if (Children.Count > 3)
					{

						Point childRight = Children[i + 2].TransformToAncestor(sourceView).Transform(new Point(0, 0));

						if (xLocation >= childLeft.X + Children[i].RenderSize.Width / 2 &&
							xLocation < childRight.X + Children[i + 2].RenderSize.Width / 2)
						{
							dropIndex = i + 2;
							break;
						}
					}
				}

				Children.Insert(dropIndex, newElement);
				Children.Insert(dropIndex + 1, new ArrowConnector());
			}

			// Add a child to the collection
			//Children.Add(element);
			NotifyPropertyChanged("Children");
			NotifyPropertyChanged("StackMargin");
		}

		public void AddCase() {
			
		}

		public void TopButtonAction() {
			if (TopButtonColour == Brushes.DarkGray)
			{
				TopButtonColour = Brushes.LawnGreen;
				TopButtonOpacity = 0.9;
				BottomButtonColour = Brushes.DarkGray;
				BottomButtonOpacity = 0.5;
				_caseIndex = 0;
				NotifyPropertyChanged("Children");
				NotifyPropertyChanged("TopButtonColour");
				NotifyPropertyChanged("TopButtonOpacity");
				NotifyPropertyChanged("BottomButtonColour");
				NotifyPropertyChanged("BottomButtonOpacity");
				NotifyPropertyChanged("StackMargin");
			} 
		}

		public void BottomButtonAction() {
			if (BottomButtonColour == Brushes.DarkGray)
			{
				BottomButtonColour = Brushes.Red;
				BottomButtonOpacity = 0.9;
				TopButtonColour = Brushes.DarkGray;
				TopButtonOpacity = 0.5;
				_caseIndex = 1;
				NotifyPropertyChanged("Children");
				NotifyPropertyChanged("TopButtonColour");
				NotifyPropertyChanged("TopButtonOpacity");
				NotifyPropertyChanged("BottomButtonColour");
				NotifyPropertyChanged("BottomButtonOpacity");
				NotifyPropertyChanged("StackMargin");
			} 
		}

		public void UpdateChildrenSizes() {
			for(int i=0; i < _cases[_caseIndex].Count; i++) {
				if(_cases[_caseIndex][i].RenderSize.Height > _maximumElementSize) _maximumElementSize = _cases[_caseIndex][i].RenderSize.Height;
				if (_cases[_caseIndex][i].RenderSize.Height > _maxSizes[_caseIndex]) _maxSizes[_caseIndex] = _cases[_caseIndex][i].RenderSize.Height;
			}
			NotifyPropertyChanged("StackMargin");
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
