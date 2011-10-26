using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace RobotInitial.ViewModel {
	class SwitchPropertiesViewModel : ViewModelBase, INotifyPropertyChanged {
		private SwitchBlock<bool> _switchModel = DefaultBlockFactory.Instance.CreateSwitchBlock<bool>();
		public SwitchBlock<bool> SwitchModel { get { return _switchModel; } }

		// IR Condition
		private IRSensorConditional _irSensor;

		// Condition types and its property
		private ObservableCollection<string> _condTypes = new ObservableCollection<string>();
		public ObservableCollection<string> CondTypes {
			get { return _condTypes; }
		}

		// Condition options and its property
		private ObservableCollection<ObservableCollection<string>> _condOptions = new ObservableCollection<ObservableCollection<string>>();
		public ObservableCollection<string> CondOptions {
			get { return _condOptions[0]; }
		}

		// Condition operators and its property
		private ObservableCollection<ObservableCollection<string>> _condOperators = new ObservableCollection<ObservableCollection<string>>();
		public ObservableCollection<string> CondOperators {
			get { return _condOperators[0]; }
		}

		private int _selectedCond = 0;

		// Track the selected index
		public int SelectedCond {
			get {
				if(SwitchModel.Condition is IRSensorConditional) {
					_selectedCond = ((IRSensorConditional)SwitchModel.Condition).IRSensorNumber;
				}
					 
				return _selectedCond;
			}
			set {
				_selectedCond = value;

				Visibility = Visibility.Visible;
				NotifyPropertyChanged("Visibility");
				// Check which condition selected
				switch (value) {
					case 0: // IR Sensor - Front
						_irSensor.IRSensorNumber = value;
						SwitchModel.Condition = _irSensor;
						break;
					case 1: // IR Sensor - Front Left
						_irSensor.IRSensorNumber = value;
						SwitchModel.Condition = _irSensor;
						break;
					case 2: // IR Sensor - Front Right
						_irSensor.IRSensorNumber = value;
						SwitchModel.Condition = _irSensor;
						break;
					case 3: // IR Sensor - Rear
						_irSensor.IRSensorNumber = value;
						SwitchModel.Condition = _irSensor;
						break;
					case 4: // IR Sensor - Rear Left
						_irSensor.IRSensorNumber = value;
						SwitchModel.Condition = _irSensor;
						break;
					case 5: // IR Sensor - Rear Right
						_irSensor.IRSensorNumber = value;
						SwitchModel.Condition = _irSensor;
						break;
				}
			}
		}

		// The selected operator
		private int _selectedOperator = 0;

		// Handle the selected operator
		public int SelectedOperator {
			get {
				if (SwitchModel.Condition is IRSensorConditional) {
					if(_irSensor.EqualityOperator == IRSensorConditional.Operator.EQUAL) _selectedOperator = 0;
					else if(_irSensor.EqualityOperator == IRSensorConditional.Operator.NOTEQUAL) _selectedOperator = 1;
					else if (_irSensor.EqualityOperator == IRSensorConditional.Operator.LESS) _selectedOperator = 2;
					else if (_irSensor.EqualityOperator == IRSensorConditional.Operator.EQUALORLESS) _selectedOperator = 3;
					else if (_irSensor.EqualityOperator == IRSensorConditional.Operator.GREATER) _selectedOperator = 4;
					else if (_irSensor.EqualityOperator == IRSensorConditional.Operator.EQUALORGREATER) _selectedOperator = 5;
				} 
				return _selectedOperator; 
			}
			set {
				_selectedOperator = value;

				// Selected value is an IRSensor value
				if (SelectedCond >= 0 && SelectedCond <= 5) {
					switch (value) {
						case 0: // Equal To (==)
							_irSensor.EqualityOperator = IRSensorConditional.Operator.EQUAL;
							break;
						case 1: // Not Equal To (!=)
							_irSensor.EqualityOperator = IRSensorConditional.Operator.NOTEQUAL;
							break;
						case 2: // Less Than (<)
							_irSensor.EqualityOperator = IRSensorConditional.Operator.LESS;
							break;
						case 3: // Less Than or Equal To (<=)
							_irSensor.EqualityOperator = IRSensorConditional.Operator.EQUALORLESS;
							break;
						case 4: // Greater Than (>)
							_irSensor.EqualityOperator = IRSensorConditional.Operator.GREATER;
							break;
						case 5: // Greater Than or Equal To (>=)
							_irSensor.EqualityOperator = IRSensorConditional.Operator.EQUALORGREATER;
							break;
					}
				}
			}
		}

		// Operator number e.g. distance
		public int OperatorNumber {
			get {
				return _irSensor.Distance;
			}
			set {
				_irSensor.Distance = value;
			}
		}

		// Used for setting forever mode
		private Visibility _isVisible = Visibility.Visible;
		public Visibility Visibility {
			get { return _isVisible; }
			set {
				_isVisible = value;
			}
		}

		public SwitchPropertiesViewModel() {
			// the default condition for a wait block is a IR Sensor
			_irSensor = (IRSensorConditional)SwitchModel.Condition;

			// Initiliase the condition types
			_condTypes.Add("IR Sensor - Front");
			_condTypes.Add("IR Sensor - Front Left");
			_condTypes.Add("IR Sensor - Front Right");
			_condTypes.Add("IR Sensor - Rear");
			_condTypes.Add("IR Sensor - Rear Left");
			_condTypes.Add("IR Sensor - Rear Right");

			// Initialise the condition options, the in order of above
			ObservableCollection<string> sensorOptions = new ObservableCollection<string>();
			sensorOptions.Add("Range");
			_condOptions.Add(sensorOptions);

			// Initialise the condition operators, the in order of above
			ObservableCollection<string> sensorOperators = new ObservableCollection<string>();
			sensorOperators.Add("Equal To (==)");
			sensorOperators.Add("Not Equal To (!=)");
			sensorOperators.Add("Less Than (<)");
			sensorOperators.Add("Less Than or Equal To (<=)");
			sensorOperators.Add("Greater Than (>)");
			sensorOperators.Add("Greater Than or Equal To (>=)");
			_condOperators.Add(sensorOperators);
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
