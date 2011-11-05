using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace RobotInitial.ViewModel {
	class LoopPropertiesViewModel : ConditionalPropertiesViewModel, INotifyPropertyChanged {

		private LoopBlock _loopModel = DefaultModelFactory.Instance.CreateLoopBlock();
		public LoopBlock LoopModel { get { return _loopModel; } set { _loopModel = value; } }

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
				if (LoopModel.Condition is IRSensorConditional) {
					_selectedCond = 0;
				}
				else if (LoopModel.Condition is CountConditional) {
					_selectedCond = 1;
				}
				else if (LoopModel.Condition is FalseConditional) {
					_selectedCond = 2;
				}
				return _selectedCond;
			}
			set {
				_selectedCond = value;
				// Check which condition selected
				switch (value) {
					case 0: // IR Sensor
						LoopModel.Condition = _irSensor;
						break;
					case 1: // Count
						LoopModel.Condition = _countCondition;
						break;
					case 2: // Forever
						LoopModel.Condition = DefaultModelFactory.Instance.CreateFalseConditional();
						break;
				}
				NotifyVisibility();
				NotifyAllSensors();
			}
		}

		public LoopPropertiesViewModel() {

			// Set the condition from the default type
			if (LoopModel.Condition is CountConditional) {
				_countCondition = (CountConditional)LoopModel.Condition;
			}
			else {
				_countCondition = DefaultModelFactory.Instance.CreateCountConditional();
			}

			// If the condition is an IRSensor
			if (LoopModel.Condition is IRSensorConditional) {
				_irSensor = (IRSensorConditional)LoopModel.Condition;
			}
			else {
				_irSensor = DefaultModelFactory.Instance.CreateIRSensorConditional();
			}

			// Initiliase the condition types
			CondTypes.Add("IR Sensor");
			CondTypes.Add("Counter");
			CondTypes.Add("Forever");
		}

		// Override depending on properties type
		public override void NotifyVisibility() {
			NotifyPropertyChanged("SensorPaneVisibility");
			NotifyPropertyChanged("CounterPaneVisibility");
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
