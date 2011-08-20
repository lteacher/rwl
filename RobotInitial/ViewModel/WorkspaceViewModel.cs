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
            base.DisplayName = Resources.untitledFileName + Resources.fileExtension;
            _workspace = workspace;
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

        void OnDrop()
        {
            Debug.WriteLine("OnDrop called");
        }
    }
}
