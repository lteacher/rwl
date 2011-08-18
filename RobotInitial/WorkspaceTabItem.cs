using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace RobotInitial
{
    class WorkspaceTabItem : TabItem
    {
        Canvas canvas;
        Workspace workspace;

        public WorkspaceTabItem()
        {
            Header = "untitled_RAWR_.rwl";
            
            AddChild(canvas = new Canvas());
            GridMask gridMask = new GridMask();
            gridMask.ColWidth = 25;
            gridMask.RowWidth = 25;
            canvas.Children.Add(gridMask);
            canvas.Children.Add(workspace = new Workspace());

        }

        public Canvas Canvas
        {
            get { return canvas; }
        }

    }
}
