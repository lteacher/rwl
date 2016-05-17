using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace RobotInitial.ViewModel
{
	class WaitPropertiesViewModel : ConditionalPropertiesViewModel, INotifyPropertyChanged
	{
		// WaitModel property
		private WaitBlock _waitModel = DefaultModelFactory.Instance.CreateWaitBlock();
		public WaitBlock WaitModel { get { return _waitModel; } set { _waitModel = value; } }

		// Sensor Properties Visibility
		public Visibility SensorPaneVisibility {
			get {
				return SelectedCond == 0 ? Visibility.Visible : Visibility.Hidden;
			}
		}

		// Counter Properties Visibility
		public Visibility TimerPaneVisibility {
			get {
				return SelectedCond == 1 ? Visibility.Visible : Visibility.Hidden;
			}
		}

		private int _selectedCond = 0;

		// Track the selected index
		public int SelectedCond {
			get {
				if (WaitModel.WaitUntil is IRSensorConditional) {
					_selectedCond = 0;
				}
				else if (WaitModel.WaitUntil is TimeConditional) {
					_selectedCond = 1;
				}
				return _selectedCond;
			}
			set {
				_selectedCond = value;
				// Check which condition selected
				switch (value) {
					case 0: // IR Sensor
						WaitModel.WaitUntil = _irSensor;
						break;
					case 1: // Count
						WaitModel.WaitUntil = _timeCondition;
						break;
				}
				NotifyVisibility();
				NotifyAllSensors();
			}
		}

		// Timer Condition Duration property
		public string TimeDuration {
			get {
				return (_timeCondition.Duration / 1000).ToString();
			}
			set {
				double result;
				if (Double.TryParse(value, out result)) {
					result *= 1000;
					_timeCondition.Duration = (int)result;
					NotifyPropertyChanged("TimeDuration");
				}
				else return;
			}
		}

		public WaitPropertiesViewModel() {
			// Set the condition from the default type
			if (WaitModel.WaitUntil is TimeConditional) {
				_timeCondition = (TimeConditional)WaitModel.WaitUntil;
			} else {
				_timeCondition = DefaultModelFactory.Instance.CreateTimeConditional();
			}

			if (WaitModel.WaitUntil is IRSensorConditional) {
				_irSensor = (IRSensorConditional)WaitModel.WaitUntil;
			}
			else {
				_irSensor = DefaultModelFactory.Instance.CreateIRSensorConditional();
			}

			// Initiliase the condition types
			CondTypes.Add("IR Sensor");
			CondTypes.Add("Timer");
		}

		// Override depending on properties type
		public override void NotifyVisibility() {
			NotifyPropertyChanged("SensorPaneVisibility");
			NotifyPropertyChanged("TimerPaneVisibility");
		}

		protected override void OnPropertyChanged(string propertyName) {
			NotifyPropertyChanged(propertyName);
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
