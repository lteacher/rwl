using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace RobotInitial.ViewModel {
	class LoopPropertiesViewModel : ViewModelBase, INotifyPropertyChanged {

		private LoopBlock _loopModel = DefaultModelFactory.Instance.CreateLoopBlock();
		public LoopBlock LoopModel { get { return _loopModel; } set { _loopModel = value; } }

		// Count Condition
		private CountConditional _countCondition;

		// Count Condition Duration property
		public int RepeatCount {
			get { return _countCondition.Limit; }
			set { _countCondition.Limit = value; }
		}

		// IR Condition
		private IRSensorConditional _irSensor;

		// Condition types and its property
		private ObservableCollection<string> _condTypes = new ObservableCollection<string>();
		public ObservableCollection<string> CondTypes {
			get { return _condTypes; }
		}

		// Condition operators and its property
		private ObservableCollection<ObservableCollection<string>> _condOperators = new ObservableCollection<ObservableCollection<string>>();
		public ObservableCollection<string> CondOperators {
			get { return _condOperators[0]; }
		}

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
				else if(LoopModel.Condition is CountConditional) {
					_selectedCond = 1;
				}
				else if(LoopModel.Condition is FalseConditional) {
					_selectedCond = 2;
				}
				//NotifyVisibility();
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
				NotifyPropertyChanged("FrontDistance");
			}
		}

		// Update the visibility of the properties grids
		private void NotifyVisibility() {
			NotifyPropertyChanged("SensorPaneVisibility");
			NotifyPropertyChanged("CounterPaneVisibility");
		}

		// The selected operator
		private int _selectedOperator = 0;

		// Handle the selected operator
		public int SelectedOperator {
			get {
				if (LoopModel.Condition is IRSensorConditional) {
					if (_irSensor.EqualityOperator == Operator.EQUAL) _selectedOperator = 0;
					else if (_irSensor.EqualityOperator == Operator.NOTEQUAL) _selectedOperator = 1;
					else if (_irSensor.EqualityOperator == Operator.LESS) _selectedOperator = 2;
					else if (_irSensor.EqualityOperator == Operator.EQUALORLESS) _selectedOperator = 3;
					else if (_irSensor.EqualityOperator == Operator.GREATER) _selectedOperator = 4;
					else if (_irSensor.EqualityOperator == Operator.EQUALORGREATER) _selectedOperator = 5;
				}
				return _selectedOperator;
			}
			set {
				_selectedOperator = value;
				// Selected value is an IRSensor value
				if (SelectedCond >= 0 && SelectedCond <= 5) {
					switch (value) {
						case 0: // Equal To (==)
							_irSensor.EqualityOperator = Operator.EQUAL;
							break;
						case 1: // Not Equal To (!=)
							_irSensor.EqualityOperator = Operator.NOTEQUAL;
							break;
						case 2: // Less Than (<)
							_irSensor.EqualityOperator = Operator.LESS;
							break;
						case 3: // Less Than or Equal To (<=)
							_irSensor.EqualityOperator = Operator.EQUALORLESS;
							break;
						case 4: // Greater Than (>)
							_irSensor.EqualityOperator = Operator.GREATER;
							break;
						case 5: // Greater Than or Equal To (>=)
							_irSensor.EqualityOperator = Operator.EQUALORGREATER;
							break;
					}
				}
			}
		}

		// Operator number e.g. distance
		public int OperatorNumber {
			get {
				//return _irSensor.Distance;
				return 0;
			}
			set {
				//_irSensor.Distance = value;
			}
		}

		//------------ SENSOR ENABLING BINDINGS ----------------------
		public bool FrontEnabled {
			get { return _irSensor.GetPortState(LynxIRPort.FRONT); }
			set {
				_irSensor.SetPortState(LynxIRPort.FRONT, value);
				NotifyPropertyChanged("FrontVisibility");
				NotifyPropertyChanged("FrontDistance");
			}
		}

		public bool FrontLeftEnabled {
			get { return _irSensor.GetPortState(LynxIRPort.FRONTLEFT); }
			set {
				_irSensor.SetPortState(LynxIRPort.FRONTLEFT, value);
				NotifyPropertyChanged("FrontLeftVisibility");
				NotifyPropertyChanged("FrontLeftDistance");
			}
		}

		public bool FrontRightEnabled {
			get { return _irSensor.GetPortState(LynxIRPort.FRONTRIGHT); }
			set {
				_irSensor.SetPortState(LynxIRPort.FRONTRIGHT, value);
				NotifyPropertyChanged("FrontRightVisibility");
				NotifyPropertyChanged("FrontRightDistance");
			}
		}

		public bool RearEnabled {
			get { return _irSensor.GetPortState(LynxIRPort.REAR); }
			set {
				_irSensor.SetPortState(LynxIRPort.REAR, value);
				NotifyPropertyChanged("RearVisibility");
				NotifyPropertyChanged("RearDistance");
			}
		}

		public bool RearLeftEnabled {
			get { return _irSensor.GetPortState(LynxIRPort.REARLEFT); }
			set {
				_irSensor.SetPortState(LynxIRPort.REARLEFT, value);
				NotifyPropertyChanged("RearLeftVisibility");
				NotifyPropertyChanged("RearLeftDistance");
			}
		}

		public bool RearRightEnabled {
			get { return _irSensor.GetPortState(LynxIRPort.REARRIGHT); }
			set {
				_irSensor.SetPortState(LynxIRPort.REARRIGHT, value);
				NotifyPropertyChanged("RearRightVisibility");
				NotifyPropertyChanged("RearDistance");
			}
		}
		//------------ END SENSOR ENABLING BINDINGS ----------------------

		//------------ SENSOR DISTANCE BINDINGS ----------------------
		public int FrontDistance {
			get { return _irSensor.GetDistance(LynxIRPort.FRONT); }
			set { _irSensor.SetDistance(LynxIRPort.FRONT, value); }
		}

		public int FrontLeftDistance {
			get { return _irSensor.GetDistance(LynxIRPort.FRONTLEFT); }
			set { _irSensor.SetDistance(LynxIRPort.FRONTLEFT, value); }
		}

		public int FrontRightDistance {
			get { return _irSensor.GetDistance(LynxIRPort.FRONTRIGHT); }
			set { _irSensor.SetDistance(LynxIRPort.FRONTRIGHT, value); }
		}

		public int RearDistance {
			get { return _irSensor.GetDistance(LynxIRPort.REAR); }
			set { _irSensor.SetDistance(LynxIRPort.REAR, value); }
		}

		public int RearLeftDistance {
			get { return _irSensor.GetDistance(LynxIRPort.REARLEFT); }
			set { _irSensor.SetDistance(LynxIRPort.REARLEFT, value); }
		}

		public int RearRightDistance {
			get { return _irSensor.GetDistance(LynxIRPort.REARRIGHT); }
			set { _irSensor.SetDistance(LynxIRPort.REARRIGHT, value); }
		}
		//------------ END SENSOR DISTANCE BINDINGS ----------------------

		//------------ SENSOR VISIBILITY BINDINGS ----------------------
		public Visibility FrontVisibility {
			get { return FrontEnabled ? Visibility.Visible : Visibility.Hidden; }
		}

		public Visibility FrontLeftVisibility {
			get { return FrontLeftEnabled ? Visibility.Visible : Visibility.Hidden; }
		}

		public Visibility FrontRightVisibility {
			get { return FrontRightEnabled ? Visibility.Visible : Visibility.Hidden; }
		}

		public Visibility RearVisibility {
			get { return RearEnabled ? Visibility.Visible : Visibility.Hidden; }
		}

		public Visibility RearLeftVisibility {
			get { return RearLeftEnabled ? Visibility.Visible : Visibility.Hidden; }
		}

		public Visibility RearRightVisibility {
			get { return RearRightEnabled ? Visibility.Visible : Visibility.Hidden; }
		}
		//------------ END SENSOR VISIBILITY BINDINGS ----------------------

		public LoopPropertiesViewModel() {
			// the default condition for a wait block is a TimeCondition
			_countCondition = (CountConditional)LoopModel.Condition;

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
			_condTypes.Add("IR Sensor");
			_condTypes.Add("Counter");
			_condTypes.Add("Forever");

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
