using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;

namespace RobotInitial.ViewModel
{
	class MoveControlBlockViewModel : ControlBlockViewModel
	{
		public MoveBlock MoveBlock { get; set; }
		
		public MoveControlBlockViewModel() {
			Type = "Move";
		}
	}
}
