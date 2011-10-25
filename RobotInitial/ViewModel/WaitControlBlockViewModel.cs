using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.View;
using RobotInitial.Model;

namespace RobotInitial.ViewModel
{
	class WaitControlBlockViewModel : ControlBlockViewModel
	{
		private WaitPropertiesView _propertiesView = new WaitPropertiesView();

		public WaitPropertiesView PropertiesView {
			get { return _propertiesView; }
		}

		// For convenience return the model here
		public WaitBlock ModelBlock { get{ return ((WaitPropertiesViewModel)_propertiesView.DataContext).WaitModel; } }

		public WaitControlBlockViewModel() {
			Type = "Wait";
		}
	}
}
