﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Input;

using RobotInitial.Command;
using RobotInitial.Properties;
using RobotInitial.Model;
using RobotInitial.View;

namespace RobotInitial.ViewModel
{
	class MainWindowViewModel : ClosableViewModel, INotifyPropertyChanged 
    {

        #region Fields

        #region Commands

        RelayCommand _newWorkspaceCommand;
        RelayCommand _openWorkspaceCommand;
        RelayCommand _saveWorkspaceCommand;
        RelayCommand _saveAsWorkspaceCommand;
        RelayCommand _closeWorkspaceCommand;

        RelayCommand _undoCommand;
        RelayCommand _redoCommand;

        #endregion // Commands
     
		//ObservableCollection<WorkspaceViewModel> _workspaces;
		//ObservableCollection<TaskBlockTabViewModel> _brickTabs;

		// The workspace views that can show in the tab control
		ObservableCollection<WorkspaceView> _workspaces = new ObservableCollection<WorkspaceView>();
        public ObservableCollection<WorkspaceView> Workspaces {
			get { return _workspaces; }
		}

		// The brick tabs on the left side, theres really just one tab for now
		ObservableCollection<TaskBlockTabView> _brickTabs = new ObservableCollection<TaskBlockTabView>();
		public ObservableCollection<TaskBlockTabView> BrickTabs {
			get { return _brickTabs; }
		}

		// The selected index of whatever workspace is active, used to get the currently selected index... since i know it will work
		public int SelectedIndex { get; set; }

		// Now the current/active workspace is easy to get, its the item in the collection from the selected index
		public WorkspaceView GetCurrentWorkspace() {
			return Workspaces[SelectedIndex];
		}

        bool _undoEnabled = false;
        bool _redoEnabled = false;

        #endregion // Fields

        public MainWindowViewModel()
        {
            base.DisplayName = Resources.applicationDisplayName;
			// Create a new workspace
			CreateNewWorkspace();

			// Just add a bricktabs probably there wont ever be any more tabs
			BrickTabs.Add(new TaskBlockTabView());
			((TaskBlockTabViewModel)BrickTabs[0].DataContext).DisplayName = "C";
			
        }


		//#region Command Definitions

		//#region NewWorkspaceCommand

		public ICommand NewWorkspaceCommand {
			get {
				if (_newWorkspaceCommand == null) {
					_newWorkspaceCommand = new RelayCommand(param => this.CreateNewWorkspace());
				}
				return _newWorkspaceCommand;
			}
		}

		private void CreateNewWorkspace() {
			WorkspaceView workspace = new WorkspaceView();
			((WorkspaceViewModel)workspace.DataContext).DisplayName = Resources.untitledFileName;
			Workspaces.Add(workspace);
		}

		//#endregion // NewWorkspaceCommand

		//#region OpenWorkspaceCommand

		//public ICommand OpenWorkspaceCommand
		//{
		//    get
		//    {
		//        if (_openWorkspaceCommand == null)
		//        {
		//            _openWorkspaceCommand = new RelayCommand(param => this.OpenWorkspace());
		//        }
		//        return _openWorkspaceCommand;
		//    }
		//}

		//#endregion // OpenWorkspaceCommand

		//#region SaveWorkspaceCommand

		//public ICommand SaveWorkspaceCommand
		//{
		//    get
		//    {
		//        if (_saveWorkspaceCommand == null)
		//        {
		//            _saveWorkspaceCommand = new RelayCommand(param => this.SaveWorkspace(false));
		//        }
		//        return _saveWorkspaceCommand;
		//    }
		//}

		//#endregion // SaveWorkspaceCommand

		//#region SaveAsWorkspaceCommand

		//public ICommand SaveAsWorkspaceCommand
		//{
		//    get
		//    {
		//        if (_saveAsWorkspaceCommand == null)
		//        {
		//            _saveAsWorkspaceCommand = new RelayCommand(param => this.SaveWorkspace(true));
		//        }
		//        return _saveAsWorkspaceCommand;
		//    }
		//}

		//#endregion // SaveWorkspaceAsCommand

		//#region CloseWorkspaceCommand

		//public ICommand CloseWorkspaceCommand
		//{
		//    get
		//    {
		//        if (_closeWorkspaceCommand == null)
		//        {
		//            _closeWorkspaceCommand = new RelayCommand(param => this.CloseWorkspace());
		//        }
		//        return _closeWorkspaceCommand;
		//    }
		//}

		//#endregion // CloseWorkspaceCommand

		//#region UndoCommand

		//public ICommand UndoCommand
		//{
		//    get
		//    {
		//        if (_undoCommand == null)
		//        {
		//            _undoCommand = new RelayCommand(param => this.GetCurrentWorkspace().Undo());
		//        }
		//        return _undoCommand;
		//    }
		//}

		//#endregion // UndoCommand

		//#region RedoCommand
        
		//public ICommand RedoCommand
		//{
		//    get
		//    {
		//        if (_redoCommand == null)
		//        {
		//            _redoCommand = new RelayCommand(param => this.GetCurrentWorkspace().Redo());
		//        }
		//        return _redoCommand;
		//    }
		//}
        
		//#endregion // RedoCommand

		//#endregion // Command Definitions

//        #region Property Bindings

//        public bool IsRedoEnabled
//        {
//            get { return GetCurrentWorkspace().IsRedoEnabled; }
//        }

//        public bool IsUndoEnabled
//        {
//            get { return GetCurrentWorkspace().IsUndoEnabled; }
//        }

//        #endregion // Property Bindings

//        #region Collection Definitions

//        #region Workspaces

//        public ObservableCollection<WorkspaceViewModel> Workspaces
//        {
//            get
//            {
//                if (_workspaces == null)
//                {
//                    _workspaces = new ObservableCollection<WorkspaceViewModel>();
//                    _workspaces.CollectionChanged += OnWorkspacesChanged;
//                }

//                return _workspaces;
//            }
//        }

//        void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
//        {
//            if (e.NewItems != null && e.NewItems.Count != 0)
//            {
//                foreach (WorkspaceViewModel workspace in e.NewItems)
//                {
//                    workspace.RequestClose += this.OnWorkspaceRequestClose;
//                }

//            }

//            if (e.OldItems != null && e.OldItems.Count != 0)
//            {
//                foreach (WorkspaceViewModel workspace in e.OldItems)
//                {
//                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
//                }
//            }
//        }

//        void OnWorkspacePropertyChanged(object sender, EventArgs e)
//        {
//            Debug.WriteLine("Property Changed: " + e);
//            if (e.ToString().Equals("IsUndoEnabled"))
//            {
//                OnPropertyChanged("IsUndoEnabled");
//            }
//            else if (e.ToString().Equals("IsRedoEnabled"))
//            {
//                OnPropertyChanged("IsRedoEnabled");
//            }

//        }

//        void OnWorkspaceRequestClose(object sender, EventArgs e)
//        {
//            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
//            workspace.Dispose();
//            this.Workspaces.Remove(workspace);
//        }

//#endregion // Workspaces

//        #region BrickTabs

//        public ObservableCollection<TaskBlockTabViewModel> BrickTabs
//        {
//            get
//            {
//                if (_brickTabs == null)
//                {
//                    _brickTabs = new ObservableCollection<TaskBlockTabViewModel>();
//                    _brickTabs.CollectionChanged += OnBricksTabChanged;
//                }

//                return _brickTabs;

//            }
//        }

//        void OnBricksTabChanged(object sender, NotifyCollectionChangedEventArgs e)
//        {
//            if (e.NewItems != null && e.NewItems.Count != 0)
//            {
//                foreach (TaskBlockTabViewModel brickTab in e.NewItems)
//                {
//                    brickTab.RequestClose += this.OnBrickTabRequestClose;
//                }

//            }

//            if (e.OldItems != null && e.OldItems.Count != 0)
//            {
//                foreach (TaskBlockTabViewModel brickTab in e.OldItems)
//                {
//                    brickTab.RequestClose -= this.OnBrickTabRequestClose;
//                }
//            }
//        }

//        void OnBrickTabRequestClose(object sender, EventArgs e)
//        {
//            TaskBlockTabViewModel brickTab = sender as TaskBlockTabViewModel;
//            brickTab.Dispose();
//            this.BrickTabs.Remove(brickTab);
//        }

//        #endregion // BrickTabs

//        #endregion // Collection Definitions

//        #region Private Helper Methods

//        void Initialise()
//        {
//            TaskBlockTabViewModel btmodel = new TaskBlockTabViewModel();
//            this.BrickTabs.Add(btmodel);

//            // Create a new workspace on startup
//            CreateNewWorkspace();
//        }

//        void CreateNewWorkspace()
//        {
//            Workspace workspaceModel = Workspace.CreateNewWorkspace();
//            WorkspaceViewModel workspace = new WorkspaceViewModel();
//            workspace.DisplayName = Resources.untitledFileName;
//            this.Workspaces.Add(workspace);
//            //workspace.PropertyChanged += new PropertyChangedEventHandler(OnWorkspacePropertyChanged);
//            this.SetActiveWorkspace(workspace);
//        }

//        void SetActiveWorkspace(WorkspaceViewModel workspace)
//        {
//            Debug.Assert(Workspaces.Contains(workspace));

//            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
//            if (collectionView != null)
//            {
//                collectionView.MoveCurrentTo(workspace);
//                NotifyPropertyChanged("Workspaces");
//            }
//        }

//        void OpenWorkspace()
//        {
//            // Show Dialog (this is a non-trivial task in MVVM)
//            // Get filename from dialog
//            Workspace workspaceModel = Workspace.LoadWorkspace("rawr.rwl");
//            //WorkspaceViewModel workspace = new WorkspaceViewModel(workspaceModel);
//            WorkspaceViewModel workspace = new WorkspaceViewModel();
//            this.Workspaces.Add(workspace);
//            this.SetActiveWorkspace(workspace);
//        }

//        void SaveWorkspace(bool newFile)
//        {
//            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
//            if (collectionView != null)
//            {
//                WorkspaceViewModel viewmodel = collectionView.CurrentItem as WorkspaceViewModel;
//                if (viewmodel != null)
//                {
//                    if (viewmodel.IsUntitled || newFile)
//                    {
//                        // Show Dialog (this is a non-trivial task in MVVM)
//                        // Get filename from dialog
//                    }

//                    // Write Workspace Model to disk
//                    // Mark Workspace as saved.
//                }
                    
//            }
//        }

//        void CloseWorkspace()
//        {
//            // Save Workspace.

//        }

//        WorkspaceViewModel GetCurrentWorkspace()
//        {
//            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
//            return collectionView.CurrentItem as WorkspaceViewModel;
//        }


//        #endregion // Private Helper Methods

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
