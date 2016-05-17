using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace RobotInitial.ViewModel {
	class StartStopControlViewModel : ViewModelBase {

		public Visibility Visibility {
			get {
				return ((MainWindowViewModel)Application.Current.MainWindow.DataContext).Workspaces.Count == 0 ? Visibility.Hidden : Visibility.Visible;
			}

		}

		public StartStopControlViewModel() {

		}
	}
}
