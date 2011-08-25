using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RobotInitial.Model {
    interface Brick {
        Point Location { get; set; }
        Brick Next { get; set; }
        ParameterList Parameters { get; set; }
        void perform(Protocol protocol);
        Brick PerformNext { get; }
    }
}
