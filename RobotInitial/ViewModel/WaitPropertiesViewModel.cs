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
		// WaitModel property
		private WaitBlock _waitModel = DefaultModelFactory.Instance.CreateWaitBlock();
		public WaitBlock WaitModel { 
			get { return _waitModel; } 
			set { 
				_waitModel = value;
			} 
		}

		// Timer Condition
		private TimeConditional _timeCondition;

		// IR Condition
		private IRSensorConditional _irSensor;

		// Timer Condition Duration property
		public double TimeDuration { 
			get { return _timeCondition.Duration/1000; }
			set { _timeCondition.Duration = (int)value*1000; }
		}

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
				if (WaitModel.WaitUntil is IRSensorConditional) {
					_selectedCond = (int)Math.Log((double)((IRSensorConditional)WaitModel.WaitUntil).IRSensors, 2);
				}

				return _selectedCond;
			}
			set {
				_selectedCond = value;
				if(CondMode) {
					// Check which condition selected
					switch(value) {
						case 0: // IR Sensor - Front
                            _irSensor.IRSensors = LynxIRPort.FRONT;
							break;
						case 1: // IR Sensor - Front Left
                            _irSensor.IRSensors = LynxIRPort.REARLEFT;
							break;
						case 2: // IR Sensor - Front Right
                            _irSensor.IRSensors = LynxIRPort.FRONTRIGHT;
							break;
						case 3: // IR Sensor - Rear
                            _irSensor.IRSensors = LynxIRPort.REAR;
							break;
						case 4: // IR Sensor - Rear Left
                            _irSensor.IRSensors = LynxIRPort.REARLEFT;
							break;
						case 5: // IR Sensor - Rear Right
                            _irSensor.IRSensors = LynxIRPort.REARRIGHT;
							break;
					}
				}
			}
		}

		// The selected operator
		private int _selectedOperator = 0;

		// Handle the selected operator
		public int SelectedOperator {
			get {
				if (WaitModel.WaitUntil is IRSensorConditional) {
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
				if(CondMode) {
					// Selected value is an IRSensor value
					if (SelectedCond >= 0 && SelectedCond <= 5) {
						switch(value) {
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

		// Binding for radiobuttons, requires annoying logic
		private bool _condMode = false;
		private bool _timerMode = true;

		// Condition mode selected
		public bool CondMode { 
			get {
				return _condMode;
			} 
			set {
				if(_timerMode) {
					_timerMode = false;
					_condMode = true;
					WaitModel.WaitUntil = _irSensor;
					NotifyPropertyChanged("TimerMode");
					NotifyPropertyChanged("CondMode");
					NotifyPropertyChanged("SelectedOperator");
					NotifyPropertyChanged("SelectedCond");
					NotifyPropertyChanged("OperatorNumber");
				}
			}
		}

		// Timer mode selected
		public bool TimerMode {
			get {
				return _timerMode;
			}
			set {
				if (_condMode) {
					_condMode = false;
					_timerMode = true;
					WaitModel.WaitUntil = _timeCondition;
					NotifyPropertyChanged("CondMode");
					NotifyPropertyChanged("TimerMode");
					NotifyPropertyChanged("TimeDuration");
				}
			}
		}

		public WaitPropertiesViewModel() {
			// Set the condition from the default type
			if (WaitModel.WaitUntil is TimeConditional) {
				_timeCondition = (TimeConditional)WaitModel.WaitUntil;
				TimerMode = true;
			} else {
				_timeCondition = DefaultModelFactory.Instance.CreateTimeConditional();
				CondMode = true;
			}

			if (WaitModel.WaitUntil is IRSensorConditional) {
				_irSensor = (IRSensorConditional)WaitModel.WaitUntil;
				CondMode = true;
			}
			else {
				_irSensor = DefaultModelFactory.Instance.CreateIRSensorConditional();
				TimerMode = true;
			}
			
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
