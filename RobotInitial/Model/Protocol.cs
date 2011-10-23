using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    interface Protocol {
        void Move(MoveParameters parameters);
        IRData RequestIR();
        IMUData RequestIMU();
        int RequestStatus();
        //void KeepAlive();
    }
}
