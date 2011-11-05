using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace RobotInitial.ViewModel {
	class SwitchPropertiesViewModel : ConditionalPropertiesViewModel, INotifyPropertyChanged {
		private SwitchBlock<bool> _switchModel = DefaultModelFactory.Instance.CreateSwitchBlock<bool>();
		public SwitchBlock<bool> SwitchModel { get { return _switchModel; } set { _switchModel = value; } }

		// Sensor Properties Visibility
		public Visibility SensorPaneVisibility {
			get {
				return SelectedCond == 0 ? Visibility.Visible : Visibility.Hidden;
			}
		}

		// Counter Properties Visibility
		public Visibility CounterPaneVisibility {
			get {
				return SelectedCond == 1 ? Visibility.Visible : Visibility.Hidden;
			}
		}

		private int _selectedCond = 0;

		// Track the selected index
		public int SelectedCond {
			get {
				if (SwitchModel.Condition is IRSensorConditional) {
					_selectedCond = 0;
				}
				if (SwitchModel.Condition is RBGConditional) {
					_selectedCond = 1;
				}
				return _selectedCond;
			}
			set {
				_selectedCond = value;
				// Check which condition selected
				switch (value) {
					case 0: // IR Sensor
						SwitchModel.Condition = _irSensor;
						break;
					case 1: // RandomBoolean
						SwitchModel.Condition = DefaultModelFactory.Instance.CreateRBGConditional();
						break;

				}
				NotifyVisibility();
				NotifyAllSensors();
			}
		}

		public SwitchPropertiesViewModel() {

			// If the condition is an IRSensor
			if (SwitchModel.Condition is IRSensorConditional) {
				_irSensor = (IRSensorConditional)SwitchModel.Condition;
			}
			else {
				_irSensor = DefaultModelFactory.Instance.CreateIRSensorConditional();
			}

			// Initiliase the condition types
			CondTypes.Add("IR Sensor");
			CondTypes.Add("Random Boolean");
		}

		// Override depending on properties type
		public override void NotifyVisibility() {
			NotifyPropertyChanged("SensorPaneVisibility");
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
