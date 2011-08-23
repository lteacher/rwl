using System;
using System.Windows.Input;
using System.Diagnostics;

using RobotInitial.Command;
using RobotInitial.Model;
using RobotInitial.Properties;

namespace RobotInitial.ViewModel
{
    class WorkspaceViewModel : ClosableViewModel
    {
        
        Workspace _workspace;

        public WorkspaceViewModel(Workspace workspace)
        {
            _workspace = workspace;
            base.DisplayName = _workspace.FileName;
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
            Debug.WriteLine("OnDrop called");
        }
    }
}
