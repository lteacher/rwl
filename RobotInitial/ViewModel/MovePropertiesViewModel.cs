using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace RobotInitial.ViewModel
{
	class MovePropertiesViewModel : ViewModelBase, INotifyPropertyChanged
	{
		private MoveBlock _moveModel = DefaultModelFactory.Instance.CreateMoveBlock();
		public MoveBlock MoveModel { get { return _moveModel; } set { _moveModel = value; } }

		// Directions valid for the motors
		private Collection<string> _directions = new Collection<string>();
		public Collection<string> Directions {
			get { return _directions; }
		}

		// Duration Units for the motors
		private Collection<string> _durationUnits = new Collection<string>();
		public Collection<string> DurationUnits { 
			get { return _durationUnits; }
		}

		// Left Direction selection
		private int _leftDirection = 0;
		public int LeftDirection {
			get {
				LeftStopVisibility = Visibility.Visible;
				NotifyPropertyChanged("LeftStopVisibility");
				if(MoveModel.LeftDirection == MoveDirection.FORWARD) _leftDirection = 0;
				else if(MoveModel.LeftDirection == MoveDirection.BACK) _leftDirection = 1;
				else if (MoveModel.LeftDirection == MoveDirection.STOP) {
					_leftDirection = 2;
					LeftStopVisibility = Visibility.Hidden;
					NotifyPropertyChanged("LeftStopVisibility");
				}
				return _leftDirection;
			}
			set {
				_leftDirection = value;
				LeftStopVisibility = Visibility.Visible;
				NotifyPropertyChanged("LeftStopVisibility");
				switch(value) {
					case 0: // Forwards
						MoveModel.LeftDirection = MoveDirection.FORWARD; 
						break;
					case 1: // Backwards
						MoveModel.LeftDirection = MoveDirection.BACK;
						break;
					case 2: // Stop
						MoveModel.LeftDirection = MoveDirection.STOP;
						LeftStopVisibility = Visibility.Hidden;
						NotifyPropertyChanged("LeftStopVisibility");
						break;
				}
			}
		}

		// Right Direction selection
		private int _rightDirection = 0;
		public int RightDirection {
			get {
				RightStopVisibility = Visibility.Visible;
				NotifyPropertyChanged("RightStopVisibility");
				if (MoveModel.RightDirection == MoveDirection.FORWARD) _rightDirection = 0;
				else if (MoveModel.RightDirection == MoveDirection.BACK) _rightDirection = 1;
				else if (MoveModel.RightDirection == MoveDirection.STOP) {
					_rightDirection = 2;
					RightStopVisibility = Visibility.Hidden;
					NotifyPropertyChanged("RightStopVisibility");
				}
				return _rightDirection;
			}
			set {
				_rightDirection = value;
				RightStopVisibility = Visibility.Visible;
				NotifyPropertyChanged("RightStopVisibility");
				switch (value) {
					case 0: // Forwards
						MoveModel.RightDirection = MoveDirection.FORWARD;
						break;
					case 1: // Backwards
						MoveModel.RightDirection = MoveDirection.BACK;
						break;
					case 2: // Stop
						LeftStopVisibility = Visibility.Hidden;
						NotifyPropertyChanged("RightStopVisibility");
						MoveModel.RightDirection = MoveDirection.STOP;
						break;
				}
			}
		}

		// DurationUnit selection
		private int _durationUnit = 0;
		public int DurationUnit {
			get {
				if (MoveModel.DurationUnit == MoveDurationUnit.ENCODERCOUNT) _durationUnit = 0;
				else if (MoveModel.DurationUnit == MoveDurationUnit.DEGREES) _durationUnit = 1;
				else if (MoveModel.DurationUnit == MoveDurationUnit.MILLISECONDS) _durationUnit = 2;
				else if (MoveModel.DurationUnit == MoveDurationUnit.UNLIMITED) {
					_durationUnit = 3;
				}
				return _durationUnit;
			}
			set {
				_durationUnit = value;
				
				switch (value) {
					case 0: // Encoder count
						MoveModel.DurationUnit = MoveDurationUnit.ENCODERCOUNT;
						break;
					case 1: // Degrees
						MoveModel.DurationUnit = MoveDurationUnit.DEGREES;
						break;
					case 2: // milliseconds
						MoveModel.DurationUnit = MoveDurationUnit.MILLISECONDS;
						break;
					case 3: // Unlimited
						MoveModel.DurationUnit = MoveDurationUnit.UNLIMITED;
						break;
				}
				NotifyPropertyChanged("DurationUnit");
			}
		}

		// Left Power property
		public int LeftPower {
			get {
				return MoveModel.LeftPower;
			}
			set {
				MoveModel.LeftPower = value;
				NotifyPropertyChanged("LeftPower");
			}
		}

		// Right Power property
		public int RightPower {
			get {
				return MoveModel.RightPower;
			}
			set {
				MoveModel.RightPower = value;
				NotifyPropertyChanged("RightPower");
			}
		}

		// Left Duration property
		public float LeftDuration {
			get {
				return MoveModel.LeftDuration;
			}
			set {
				MoveModel.LeftDuration = value;
			}
		}

		// Right Duration property
		public float RightDuration {
			get {
				return MoveModel.RightDuration;
			}
			set {
				MoveModel.RightDuration = value;
			}
		}

		// Special visibility if stop is selected
		public Visibility LeftStopVisibility { get; set; }
		public Visibility RightStopVisibility { get; set; }

		public MovePropertiesViewModel() {
			// Add the direction labels
			Directions.Add("Forwards");
			Directions.Add("Backwards");
			Directions.Add("Stop");

			// Add the duration labels
			DurationUnits.Add("Encoder Counts");
			DurationUnits.Add("Degrees");
			DurationUnits.Add("Milliseconds");
			DurationUnits.Add("Forever");
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
