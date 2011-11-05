using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using RobotInitial.Model;
using System.Collections.ObjectModel;

namespace RobotInitial.ViewModel {
	abstract class ConditionalPropertiesViewModel : ViewModelBase, INotifyPropertyChanged {
		
		// The IRSensor conditional
		protected IRSensorConditional _irSensor;

		// Count Condition
		protected CountConditional _countCondition;

		// Time Condition
		protected TimeConditional _timeCondition;

		// Count Condition Duration property
		public int RepeatCount {
			get { return _countCondition.Limit; }
			set { _countCondition.Limit = value; }
		}

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

		// Sensor Logical Evaluation
		private ObservableCollection<string> _logcialEvaluators = new ObservableCollection<string>();
		public ObservableCollection<string> LogicalEvaluators {
			get { return _logcialEvaluators; }
		}

		public int LogicalEvaluator {
			get { 
				if(_irSensor.LogicalOperator == LogicalOperator.AND) return 0;
				if (_irSensor.LogicalOperator == LogicalOperator.OR) return 1;
				return -1;
			}
			set { 
				switch(value) {
					case 0:
						_irSensor.LogicalOperator = LogicalOperator.AND;
						break;
					case 1:
						_irSensor.LogicalOperator = LogicalOperator.OR;
						break;
				}
			}
		}

		
		// The selected operator
		private int _selectedOperator = 0;

		// Handle the selected operator
		public int SelectedOperator {
			get {
				if (_irSensor.EqualityOperator == Operator.EQUAL) _selectedOperator = 0;
				else if (_irSensor.EqualityOperator == Operator.NOTEQUAL) _selectedOperator = 1;
				else if (_irSensor.EqualityOperator == Operator.LESS) _selectedOperator = 2;
				else if (_irSensor.EqualityOperator == Operator.EQUALORLESS) _selectedOperator = 3;
				else if (_irSensor.EqualityOperator == Operator.GREATER) _selectedOperator = 4;
				else if (_irSensor.EqualityOperator == Operator.EQUALORGREATER) _selectedOperator = 5;
				return _selectedOperator;
			}
			set {
				_selectedOperator = value;
				// Selected value is an IRSensor value
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

		//------------ SENSOR ENABLING BINDINGS ----------------------
		public bool FrontEnabled {
			get { return _irSensor.GetPortState(LynxIRPort.FRONT); }
			set {
				_irSensor.SetPortState(LynxIRPort.FRONT, value);
				OnPropertyChanged("FrontVisibility");
				OnPropertyChanged("FrontDistance");
			}
		}

		public bool FrontLeftEnabled {
			get { return _irSensor.GetPortState(LynxIRPort.FRONTLEFT); }
			set {
				_irSensor.SetPortState(LynxIRPort.FRONTLEFT, value);
				OnPropertyChanged("FrontLeftVisibility");
				OnPropertyChanged("FrontLeftDistance");

			}
		}

		public bool FrontRightEnabled {
			get { return _irSensor.GetPortState(LynxIRPort.FRONTRIGHT); }
			set {
				_irSensor.SetPortState(LynxIRPort.FRONTRIGHT, value);
				OnPropertyChanged("FrontRightVisibility");
				OnPropertyChanged("FrontRightDistance");
			}
		}

		public bool RearEnabled {
			get { return _irSensor.GetPortState(LynxIRPort.REAR); }
			set {
				_irSensor.SetPortState(LynxIRPort.REAR, value);
				OnPropertyChanged("RearVisibility");
				OnPropertyChanged("RearDistance");
			}
		}

		public bool RearLeftEnabled {
			get { return _irSensor.GetPortState(LynxIRPort.REARLEFT); }
			set {
				_irSensor.SetPortState(LynxIRPort.REARLEFT, value);
				OnPropertyChanged("RearLeftVisibility");
				OnPropertyChanged("RearLeftDistance");
			}
		}

		public bool RearRightEnabled {
			get { return _irSensor.GetPortState(LynxIRPort.REARRIGHT); }
			set {
				_irSensor.SetPortState(LynxIRPort.REARRIGHT, value);
				OnPropertyChanged("RearRightVisibility");
				OnPropertyChanged("RearRightDistance");
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

		public ConditionalPropertiesViewModel() {
			// Initialise the condition operators, the in order of above
			ObservableCollection<string> sensorOperators = new ObservableCollection<string>();
			sensorOperators.Add("Equal To (==)");
			sensorOperators.Add("Not Equal To (!=)");
			sensorOperators.Add("Less Than (<)");
			sensorOperators.Add("Less Than or Equal To (<=)");
			sensorOperators.Add("Greater Than (>)");
			sensorOperators.Add("Greater Than or Equal To (>=)");
			_condOperators.Add(sensorOperators);

			LogicalEvaluators.Add("AND Evaluation");
			LogicalEvaluators.Add("OR Evaluation");
		}

		// Update the visibility of the properties grids
		public abstract void NotifyVisibility();

		public void NotifyAllSensors() {
			OnPropertyChanged("FrontEnabled");
			OnPropertyChanged("FrontLeftEnabled");
			OnPropertyChanged("FrontRightEnabled");
			OnPropertyChanged("RearEnabled");
			OnPropertyChanged("RearLeftEnabled");
			OnPropertyChanged("RearRightEnabled");
		}
	}
}
