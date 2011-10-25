using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.View;

namespace RobotInitial.ViewModel
{
	class WaitControlBlockViewModel : ControlBlockViewModel
	{
		private WaitPropertiesView _propertiesView = new WaitPropertiesView();

		public WaitPropertiesView PropertiesView {
			get { return _propertiesView; }
		}

		public WaitControlBlockViewModel() {
			Type = "Wait";
		}
	}
}
