using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

using RobotInitial.Properties;

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
                    _newWorkspaceCommand = new RelayCommand(param => this.OnNewWorkspace());
                }

                return _newWorkspaceCommand;
            }
        }

        #endregion // NewWorkspaceCommand

        #region NewWorkspace [event]

        public event EventHandler NewWorkspace;

        void OnNewWorkspace()
        {
            EventHandler handler = NewWorkspace;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion // NewWorkspace [event]
    }
}
