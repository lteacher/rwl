﻿using System;
using System.Diagnostics;
using System.Windows.Input;

using RobotInitial.Command;
using RobotInitial.Model;
using RobotInitial.Properties;
using RobotInitial.Services;
using RobotInitial.Undo;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using RobotInitial.View;
using System.Windows.Controls;

namespace RobotInitial.ViewModel
{
    class WorkspaceViewModel : ClosableViewModel, INotifyPropertyChanged
    {

        #region Fields
        Workspace _workspace;
        readonly UndoManager _undoManager;
		double _minWidth = Application.Current.MainWindow.RenderSize.Width;
		double _minHeight =  Application.Current.MainWindow.RenderSize.Height;
		private SequenceView _sequence = new SequenceView(); 

        #endregion // Fields

        #region Properties

		public double Width {
			get { return _minWidth; }
			set { _minWidth = value; }
		}

		public SequenceView Sequence {
			get { return _sequence; }
		}

		public double Height {
			get { return _minHeight; }
			set { _minHeight = value; }
		}

		public double SequenceY {
			get { return ((int)(Height/2)/25)*25; }
		}

        public bool IsUndoEnabled
        {
            get { return _undoManager.IsUndoEnabled; }
        }

        public bool IsRedoEnabled
        {
            get { return _undoManager.IsRedoEnabled; }
        }

        #endregion // Properties


        public void Undo()
        {
            _undoManager.Undo(out _workspace);
        }

        public void Redo()
        {
            _undoManager.Redo(out _workspace);
        }

		public WorkspaceViewModel() {
			//base.DisplayName = _workspace.FileName;
			_undoManager = ServiceLocator.GetService<IUndoService>().CreateUndoManager();
			_undoManager.UndoStackChanged += new EventHandler(OnUndoChanged);
			_undoManager.RedoStackChanged += new EventHandler(OnRedoChanged);

			OnUndoChanged(null, null);
			OnRedoChanged(null, null);

			Application.Current.MainWindow.SizeChanged += new SizeChangedEventHandler(MainWindow_SizeChanged);
			_sequence.SizeChanged += new SizeChangedEventHandler(Sequence_SizeChanged);
		}

		//public WorkspaceViewModel(Workspace workspace)
		//{
		//    _workspace = workspace;
            
		//}

		private void MainWindow_SizeChanged(object sender, EventArgs e) {
			//Application.Current.MainWindow.InvalidateVisual();
			Width = Application.Current.MainWindow.RenderSize.Width;
			Height = Application.Current.MainWindow.RenderSize.Height;
			NotifyPropertyChanged("Width");
			NotifyPropertyChanged("Height");		
		}

		private void Sequence_SizeChanged(object sender, EventArgs e)
		{
			//Application.Current.MainWindow.InvalidateVisual();
			Width = Application.Current.MainWindow.RenderSize.Width;
			Height = Application.Current.MainWindow.RenderSize.Height;
			NotifyPropertyChanged("Width");
			NotifyPropertyChanged("Height");
		}

        private void OnUndoChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Firing PropertyChanged on IsUndoEnabled");
            OnPropertyChanged("IsUndoEnabled");
        }

        private void OnRedoChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Firing PropertyChanged on IsRedoEnabled");
            OnPropertyChanged("IsRedoEnabled");
        }

        RelayCommand _dropCommand;

        public ICommand DropCommand
        {
            get
            {
                if (_dropCommand == null)
                {
                    _dropCommand = new RelayCommand(param => this.OnDrop());
                }

                return _dropCommand;
            }
        }

        public bool IsUntitled
        {
            get
            {
                if (_workspace != null)
                {
                    return _workspace.IsUntitled;
                }

                return false;
            }
        }

        void OnDrop()
        {
            
        }

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Notifies the property changed.
		/// </summary>
		/// <param name="property">The property.</param>
		private void NotifyPropertyChanged(string property)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}
	}
}
