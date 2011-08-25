using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RobotInitial.Model {
    interface Block {
        Point Location { get; set; }
        Block Next { get; set; }
        ParameterList Parameters { get; set; }
        void perform(Protocol protocol);
        Block PerformNext { get; }
    }
}
