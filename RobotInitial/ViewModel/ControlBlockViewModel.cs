using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;

namespace RobotInitial.ViewModel
{
	// This class provides the ability to access certain
	// Standard properties expected of some Views,
	// Most importantly, the on drop functionality required
	// by the LoopControlBlockView and SwitchControlBlockView
	class ControlBlockViewModel : ViewModelBase
	{

		private ObservableCollection<UIElement> _children = new ObservableCollection<UIElement>();
		public String Type { get; set; }

		public ObservableCollection<UIElement> Children
		{
			get { return _children; }
		}

		public void AddChildBlock(FrameworkElement element)
		{
			if (Children.Count == 0) {
				element.Margin = new Thickness(50,0,50,0);
			}
			else {
				element.Margin = new Thickness(-25,0,50,0);
			}
			
			// Add a child to the collection
			_children.Add(element);
		}


	}
}
