using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace RobotInitial.ViewModel
{
	class SwitchControlBlockViewModel : ControlBlockViewModel
	{
		public SwitchControlBlockViewModel() {
			Type = "Switch";
		}

		public Thickness StackMargin
		{
			get
			{
				if (Children.Count == 0) return new Thickness(0, 25, 0, 25);
				return new Thickness(25, 25, 25, 25);
			}
		}
	}
}
