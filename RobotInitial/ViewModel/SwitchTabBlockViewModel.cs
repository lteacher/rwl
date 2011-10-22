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
				return new Thickness(0, 25 + (_maximumElementSize - _maxSizes[_caseIndex])/2, 0, 25+(_maximumElementSize - _maxSizes[_caseIndex])/2);
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

			//AddChildBlock(r);
			//AddChildBlock(v);
			//AddChildBlock(z);
		}
		
		public void ExpandControl() {
			MessageBox.Show("Count is currently: " + Children.Count);
		}

		public ObservableCollection<UIElement> Children
		{
			get { return _cases[_caseIndex]; }
		}

		public void AddChildBlock(FrameworkElement element)
		{

			element.Measure(new Size(element.MaxWidth, element.MaxHeight));

			// Set the ultime maximum element size if its ever increased
			if (element.DesiredSize.Height > _maximumElementSize) _maximumElementSize = element.DesiredSize.Height;

			// Set the case specific max element size
			if (element.DesiredSize.Height > _maxSizes[_caseIndex]) _maxSizes[_caseIndex] = element.DesiredSize.Height;
			if (Children.Count == 0)
			{
				element.Margin = new Thickness(50, 0, 50, 0);
			}
			else
			{
				element.Margin = new Thickness(-25, 0, 50, 0);
			}

			// Add a child to the collection
			Children.Add(element);
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
