using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Input;

using RobotInitial.Command;
using RobotInitial.Properties;
using RobotInitial.Model;
using System.Collections.Specialized;

namespace RobotInitial.ViewModel
{
    class MainWindowViewModel : ClosableViewModel
    {

        #region Fields

        #region Commands

        RelayCommand _newWorkspaceCommand;
        RelayCommand _openWorkspaceCommand;
        RelayCommand _saveWorkspaceCommand;
        RelayCommand _saveWorkspaceAsCommand;
        RelayCommand _closeWorkspaceCommand;

        #endregion // Commands

        #region Collections
        
        ObservableCollection<WorkspaceViewModel> _workspaces;
        ObservableCollection<BrickTabViewModel> _brickTabs;
        
        #endregion // Collections

        #endregion // Fields

        #region Constructor

        public MainWindowViewModel()
        {
            base.DisplayName = Resources.applicationDisplayName;
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
                    _saveWorkspaceCommand = new RelayCommand(param => this.SaveWorkspace());
                }
                return _saveWorkspaceCommand;
            }
        }

        #endregion // SaveWorkspaceCommand

        #region SaveWorkspaceAsCommand

        public ICommand SaveWorkspaceAsCommand
        {
            get
            {
                if (_saveWorkspaceAsCommand == null)
                {
                    _saveWorkspaceAsCommand = new RelayCommand(param => this.SaveAsWorkspace());
                }
                return _saveWorkspaceAsCommand;
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

        #region Private Helper Methods

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
            // Show Dialog
            // Load Workspace Model
            // Create Workspace View Model
            // Add to Workspaces
        }

        void SaveWorkspace()
        {
            // If Workspace is unnamed,
            //    call SaveAsWorkspace()
            // else, 
            //    Write Workspace Model to disk.
        }

        void SaveAsWorkspace()
        {
            // Show Dialog
            // Write Workspace Model to disk
            // Mark Workspace as saved.
        }

        void CloseWorkspace()
        {
            // Save Workspace.

        }


        #endregion // Private Helper Methods

    }
}
