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
		public String Type { get; set; }
		private double _topOffset = 0;
		private Thickness _offsetMargin = new Thickness(0, 0, 0, 0);
		public Thickness OffsetMargin
		{
			get
			{
				return _offsetMargin;
			}
			set
			{
				_offsetMargin = value;
			}
		}

		public void SetOffsetMargin(FrameworkElement element) {
			_topOffset -= 200;
			Console.WriteLine("=>>>>>>>>> SET THE OFFSET");
			OffsetMargin = new Thickness(0,_topOffset,0,0);
		}
	}
}
