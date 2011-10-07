using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    interface Protocol {
        void move(MoveParameters parameters);
        IRData requestIR();
        IMUData requestIMU();
        int requestStatus();
    }
}
