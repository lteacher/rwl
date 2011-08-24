using System;
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

namespace RobotInitial.ViewModel
{
    class MainWindowViewModel : ClosableViewModel
    {

        #region Fields

        #region Commands

        RelayCommand _newWorkspaceCommand;
        RelayCommand _openWorkspaceCommand;
        RelayCommand _saveWorkspaceCommand;
        RelayCommand _saveAsWorkspaceCommand;
        RelayCommand _closeWorkspaceCommand;

        #endregion // Commands

        #region Collections
        
        ObservableCollection<WorkspaceViewModel> _workspaces;
        ObservableCollection<TaskBlockTabViewModel> _brickTabs;
        
        #endregion // Collections

        bool _undoEnabled = false;
        bool _redoEnabled = false;

        #endregion // Fields

        #region Constructor

        public MainWindowViewModel()
        {
            base.DisplayName = Resources.applicationDisplayName;
            Initialise();
        }

        #endregion // Constructor

        #region Command Definitions

        #region NewWorkspaceCommand

        public ICommand NewWorkspaceCommand
        {
            get
            {
                if (_newWorkspaceCommand == null)
                {
                    _newWorkspaceCommand = new RelayCommand(param => this.CreateNewWorkspace());
                }
                return _newWorkspaceCommand;
            }
        }

        #endregion // NewWorkspaceCommand

        #region OpenWorkspaceCommand

        public ICommand OpenWorkspaceCommand
        {
            get
            {
                if (_openWorkspaceCommand == null)
                {
                    _openWorkspaceCommand = new RelayCommand(param => this.OpenWorkspace());
                }
                return _openWorkspaceCommand;
            }
        }

        #endregion // OpenWorkspaceCommand

        #region SaveWorkspaceCommand

        public ICommand SaveWorkspaceCommand
        {
            get
            {
                if (_saveWorkspaceCommand == null)
                {
                    _saveWorkspaceCommand = new RelayCommand(param => this.SaveWorkspace(false));
                }
                return _saveWorkspaceCommand;
            }
        }

        #endregion // SaveWorkspaceCommand

        #region SaveAsWorkspaceCommand

        public ICommand SaveAsWorkspaceCommand
        {
            get
            {
                if (_saveAsWorkspaceCommand == null)
                {
                    _saveAsWorkspaceCommand = new RelayCommand(param => this.SaveWorkspace(true));
                }
                return _saveAsWorkspaceCommand;
            }
        }

        #endregion // SaveWorkspaceAsCommand

        #region CloseWorkspaceCommand

        public ICommand CloseWorkspaceCommand
        {
            get
            {
                if (_closeWorkspaceCommand == null)
                {
                    _closeWorkspaceCommand = new RelayCommand(param => this.CloseWorkspace());
                }
                return _closeWorkspaceCommand;
            }
        }

        #endregion // CloseWorkspaceCommand

        #endregion // Command Definitions

        #region Property Bindings
        
        public bool IsRedoEnabled
        {
            get { return _redoEnabled; }
        }

        public bool IsUndoEnabled
        {
            get { return _undoEnabled; }
        }

        #endregion // Property Bindings

        #region Collection Definitions

        #region Workspaces

        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_workspaces == null)
                {
                    _workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _workspaces.CollectionChanged += OnWorkspacesChanged;
                }

                return _workspaces;
            }
        }

        void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
            {
                foreach (WorkspaceViewModel workspace in e.NewItems)
                {
                    workspace.RequestClose += this.OnWorkspaceRequestClose;
                }

            }

            if (e.OldItems != null && e.OldItems.Count != 0)
            {
                foreach (WorkspaceViewModel workspace in e.OldItems)
                {
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
                }
            }
        }

        void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            workspace.Dispose();
            this.Workspaces.Remove(workspace);
        }

#endregion // Workspaces

        #region BrickTabs

        public ObservableCollection<TaskBlockTabViewModel> BrickTabs
        {
            get
            {
                if (_brickTabs == null)
                {
                    _brickTabs = new ObservableCollection<TaskBlockTabViewModel>();
                    _brickTabs.CollectionChanged += OnBricksTabChanged;
                }

                return _brickTabs;

            }
        }

        void OnBricksTabChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
            {
                foreach (TaskBlockTabViewModel brickTab in e.NewItems)
                {
                    brickTab.RequestClose += this.OnBrickTabRequestClose;
                }

            }

            if (e.OldItems != null && e.OldItems.Count != 0)
            {
                foreach (TaskBlockTabViewModel brickTab in e.OldItems)
                {
                    brickTab.RequestClose -= this.OnBrickTabRequestClose;
                }
            }
        }

        void OnBrickTabRequestClose(object sender, EventArgs e)
        {
            TaskBlockTabViewModel brickTab = sender as TaskBlockTabViewModel;
            brickTab.Dispose();
            this.BrickTabs.Remove(brickTab);
        }

        #endregion // BrickTabs

        #endregion // Collection Definitions

        #region Private Helper Methods

        void Initialise()
        {
            TaskBlockTabViewModel btmodel = new TaskBlockTabViewModel();
            this.BrickTabs.Add(btmodel);
        }

        void CreateNewWorkspace()
        {
            Workspace workspaceModel = Workspace.CreateNewWorkspace();
            WorkspaceViewModel workspace = new WorkspaceViewModel(workspaceModel);
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }

        void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
            {
                collectionView.MoveCurrentTo(workspace);
            }
        }

        void OpenWorkspace()
        {
            // Show Dialog (this is a non-trivial task in MVVM)
            // Get filename from dialog
            Workspace workspaceModel = Workspace.LoadWorkspace("rawr.rwl");
            WorkspaceViewModel workspace = new WorkspaceViewModel(workspaceModel);
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }

        void SaveWorkspace(bool newFile)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
            {
                WorkspaceViewModel viewmodel = collectionView.CurrentItem as WorkspaceViewModel;
                if (viewmodel != null)
                {
                    if (viewmodel.IsUntitled || newFile)
                    {
                        // Show Dialog (this is a non-trivial task in MVVM)
                        // Get filename from dialog
                    }

                    // Write Workspace Model to disk
                    // Mark Workspace as saved.
                }
                    
            }
        }

        void CloseWorkspace()
        {
            // Save Workspace.

        }


        #endregion // Private Helper Methods

    }
}
