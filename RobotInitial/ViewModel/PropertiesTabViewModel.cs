using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using RobotInitial.View;
using System.ComponentModel;

namespace RobotInitial.ViewModel {
	class PropertiesTabViewModel : ViewModelBase, INotifyPropertyChanged {
		
		public FrameworkElement BlockProperties { get; set; }
		private BlankPropertiesView _blankProperties = new BlankPropertiesView();

		public PropertiesTabViewModel() {
			BlockProperties = _blankProperties;
		}

		public void setPropertiesView(FrameworkElement view) {
			BlockProperties = view;
			NotifyPropertyChanged("BlockProperties");
		}

		public void setBlankProperties() {
			BlockProperties = _blankProperties;
			NotifyPropertyChanged("BlockProperties");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Notifies the property changed.
		/// </summary>
		/// <param name="property">The property.</param>
		private void NotifyPropertyChanged(string property) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}
	}
}
