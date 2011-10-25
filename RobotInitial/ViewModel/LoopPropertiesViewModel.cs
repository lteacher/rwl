using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;

namespace RobotInitial.ViewModel {
	class LoopPropertiesViewModel : ViewModelBase {

		private LoopBlock _loopModel = new LoopBlock();
		public LoopBlock LoopModel { get { return _loopModel; } }
		
		public LoopPropertiesViewModel() {

		}
	}
}
