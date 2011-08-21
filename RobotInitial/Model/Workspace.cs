using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RobotInitial.Properties;

namespace RobotInitial.Model
{
    class Workspace
    {

        #region Static Accessors

        public static Workspace CreateNewWorkspace()
        {
            Workspace workspace = new Workspace();
            workspace.FileName = Resources.untitledFileName + Resources.fileExtension;
            workspace.IsUntitled = true;
            return workspace;
        }

        public static Workspace LoadWorkspace(string filename)
        {
            Workspace workspace = new Workspace();
            workspace.FileName = filename;
            workspace.IsUntitled = false;
            return workspace;
        }

        public static void SaveWorkspace(string filename, Workspace workspace)
        {
            workspace.IsUntitled = false;
            workspace.FileName = filename;
        }

        #endregion // Static Accessors

        #region Fields



        #endregion // Fields

        #region Properties

        public string FileName { get; protected set; }

        public bool IsUntitled { get; protected set; }

        #endregion Properties

        #region Constructors

        private Workspace()
        {
        }

        #endregion // Constructors

    }
}
