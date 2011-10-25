using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace RobotInitial.ViewModel
{
	class WaitPropertiesViewModel : ViewModelBase, INotifyPropertyChanged
	{
		private WaitBlock _waitModel = new WaitBlock();
		public WaitBlock WaitModel { get { return _waitModel; } }	
		private ObservableCollection<string> _condTypes = new ObservableCollection<string>();
		public ObservableCollection<string> CondTypes {
			get { return _condTypes; }
		}

		public int SelectedCond { get; set; }

		private bool _condMode = false;
		private bool _timerMode = true;

		public bool CondMode { 
			get { 
				return _condMode;
			} 
			set {
				if(_timerMode) {
					_timerMode = false;
					_condMode = true;
					NotifyPropertyChanged("TimerMode");
				}
			}
		}

		public bool TimerMode {
			get {
				return _timerMode;
			}
			set {
				if (_condMode) {
					_condMode = false;
					_timerMode = true;
					NotifyPropertyChanged("CondMode");
				}
			}
		}

		public WaitPropertiesViewModel() {
			_condTypes.Add("IR Sensor");
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
