﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using RobotInitial.View;
using System.Windows.Controls;
using System.ComponentModel;

namespace RobotInitial.ViewModel
{
	class LoopControlBlockViewModel : ControlBlockViewModel, INotifyPropertyChanged
	{
		private ObservableCollection<FrameworkElement> _children = new ObservableCollection<FrameworkElement>();
		private double _mainHeight = 100; // Default maximum 
		private double _mainWidth = 75;
		
		public double Height { get; set; } 
		public double Width { get; set; }
		public Thickness StackMargin { 
			get { 
				if(Children.Count == 0) return new Thickness(37.5,25,37.5,25);
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
		}
		
		public void ExpandControl() {
			MessageBox.Show("Count is currently: " + Children.Count);
		}

		public ObservableCollection<FrameworkElement> Children
		{
			get { return _children; }
		}

		public void AddChildBlock(FrameworkElement sourceView, FrameworkElement newElement, double xLocation)
		{
			if (Children.Count == 0)
			{
				_children.Add(new ArrowConnector());
				_children.Add(newElement);
				_children.Add(new ArrowConnector());
			}
			else
			{
				int dropIndex = 0;
				// Get the dropping location
				for(int i=0; i< _children.Count-1; i++) {
					if (_children[i].GetType() == typeof(ArrowConnector)) continue;

					Point childLeft = _children[i].TransformToAncestor(sourceView).Transform(new Point(0, 0));
					

					// For the first element
					if(i == 1) {
						if (xLocation < childLeft.X + _children[i].RenderSize.Width / 2)
						{
							dropIndex = 1;
							break;
						}
					}

					if (i == _children.Count-2)
					{
						if (xLocation >= childLeft.X + _children[i].RenderSize.Width/2) {
							dropIndex = _children.Count;
							break;
						}
					}

					if(_children.Count > 3) {

						Point childRight = _children[i + 2].TransformToAncestor(sourceView).Transform(new Point(0, 0));

						if (xLocation >= childLeft.X + _children[i].RenderSize.Width / 2 &&
							xLocation < childRight.X + _children[i + 2].RenderSize.Width / 2)
						{
							dropIndex = i + 2;
							break;
						}
					}
				}

				_children.Insert(dropIndex,newElement);
				_children.Insert(dropIndex + 1, new ArrowConnector());
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
