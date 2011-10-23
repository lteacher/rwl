using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using RobotInitial.View;

namespace RobotInitial.ViewModel
{
	class SequenceViewModel : ControlBlockViewModel, INotifyPropertyChanged
	{
		private ObservableCollection<FrameworkElement> _blocks = new ObservableCollection<FrameworkElement>();
		private bool isInitial = true;

		public double Height { get; set; }
		public double Width { get; set; }

		public Thickness StackMargin
		{
			get
			{
				if (Blocks.Count == 0) return new Thickness(37.5, 25, 37.5, 25);
				return new Thickness(25, 25, 25, 25);
			}
		}

		public SequenceViewModel() {
			// Set the default Height and Width
			Height = 75;
			Width = 75;

			// Create the starting block
			Rectangle start = new Rectangle();
			start.Width = 75;
			start.Height = 75;
			start.RadiusX = 4.0;
			start.RadiusY = 4.0;
			start.Stroke = Brushes.Black;
			start.StrokeDashArray = new DoubleCollection { 5.0, 2.0 };
			start.Margin = new Thickness(25,0,25,0);
			Blocks.Add(start);
		}

		public ObservableCollection<FrameworkElement> Blocks
		{
			get { return _blocks; }
		}

		public void AddBlock(FrameworkElement sourceView, FrameworkElement newElement, double xLocation)
		{
			if(isInitial) {
				Blocks.RemoveAt(0);
				newElement.Margin = new Thickness(25,0,25,0);
				Blocks.Add(newElement);
				//Blocks.Add(new ArrowConnector());
				isInitial = false;
			} else {
				int dropIndex = 0;
				// Get the dropping location
				for (int i = 0; i < Blocks.Count; i++)
				{
					if (Blocks[i].GetType() == typeof(ArrowConnector)) continue;

					Point childLeft = Blocks[i].TransformToAncestor(sourceView).Transform(new Point(0, 0));

					// For the first element
					if (i == 0)
					{
						if (xLocation < childLeft.X + Blocks[i].RenderSize.Width / 2)
						{
							dropIndex = 0;
							newElement.Margin = new Thickness(25,0,0,0);
							Blocks[i].Margin = Blocks.Count == 1 ? new Thickness(0, 0, 25, 0) : new Thickness(0, 0, 0, 0);
							Blocks.Insert(dropIndex, newElement);
							Blocks.Insert(dropIndex + 1, new ArrowConnector());
							break;
						}
					}

					// The last element
					if (i == Blocks.Count - 1)
					{
						if (xLocation >= childLeft.X + Blocks[i].RenderSize.Width / 2)
						{
							dropIndex = Blocks.Count;
							newElement.Margin = new Thickness(0, 0, 25, 0);
							Blocks[i].Margin = Blocks.Count == 1 ? new Thickness(25, 0, 0, 0) : new Thickness(0, 0, 0, 0);
							Blocks.Add(new ArrowConnector());
							Blocks.Add(newElement);
							
							break;
						}
					}

					if (Blocks.Count >= 3)
					{

						Point childRight = Blocks[i + 2].TransformToAncestor(sourceView).Transform(new Point(0, 0));

						if (xLocation >= childLeft.X + Blocks[i].RenderSize.Width / 2 &&
							xLocation < childRight.X + Blocks[i + 2].RenderSize.Width / 2)
						{
							dropIndex = i + 2;
							Blocks.Insert(dropIndex, newElement);
							Blocks.Insert(dropIndex + 1, new ArrowConnector());
							break;
						}
					}
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
