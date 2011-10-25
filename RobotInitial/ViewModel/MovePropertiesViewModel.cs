using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;

namespace RobotInitial.ViewModel
{
	class MovePropertiesViewModel : ViewModelBase
	{
		private MoveBlock _moveModel = new MoveBlock();
		public MoveBlock MoveModel { get { return _moveModel; } }

		public MovePropertiesViewModel() {

		}
	}
}
