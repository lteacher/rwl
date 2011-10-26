using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;

namespace RobotInitial.ViewModel {
	class SwitchPropertiesViewModel : ViewModelBase {
		private SwitchBlock<bool> _switchModel = new SwitchBlock<bool>();
		public SwitchBlock<bool> SwitchModel { get { return _switchModel; } }

		public SwitchPropertiesViewModel() {

		}

	}
}
