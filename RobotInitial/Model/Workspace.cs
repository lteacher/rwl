using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model
{
    class Workspace
    {
        public static Workspace CreateNewWorkspace()
        {
            return new Workspace();
        }

        private Workspace()
        {
        }
    }
}
