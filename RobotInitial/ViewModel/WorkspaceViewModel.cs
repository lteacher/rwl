using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RobotInitial.Model;
using RobotInitial.Properties;

namespace RobotInitial.ViewModel
{
    class WorkspaceViewModel : ClosableViewModel
    {
        
        Workspace _workspace;

        public WorkspaceViewModel(Workspace workspace)
        {
            base.DisplayName = Strings.WorkspaceViewModel_Untitled;
            _workspace = workspace;
        }
    }
}
