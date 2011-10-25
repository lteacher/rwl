using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;
using RobotInitial.View;

namespace RobotInitial.ViewModel
{
	class MoveControlBlockViewModel : ControlBlockViewModel
	{
		//public MoveBlock MoveBlock { get; set; }
		private MovePropertiesView _propertiesView = new MovePropertiesView();
		
		public MovePropertiesView PropertiesView {
			get { return _propertiesView; }
		}

		public MoveControlBlockViewModel() {
			Type = "Move";
		}
	}
}
