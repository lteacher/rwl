using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Input;

using RobotInitial.Properties;
using RobotInitial.Model;
using System.Collections.Specialized;

namespace RobotInitial.ViewModel
{
    class MainWindowViewModel : ClosableViewModel
    {

        #region Fields

        RelayCommand _newWorkspaceCommand;
        RelayCommand _openWorkspaceCommand;
        RelayCommand _saveWorkspaceCommand;
        RelayCommand _closeWorkspaceCommand;

        ObservableCollection<WorkspaceViewModel> _workspaces;

        #endregion // Fields

        #region Constructor
        
        public MainWindowViewModel()
        {
            base.DisplayName = Strings.MainWindowViewModel_DisplayName;
        }

        #endregion // Constructor

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


        #endregion // Private Helper Methods
    }
}
