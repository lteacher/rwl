using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    //This is lynx specific stuff in the model due fuctionalitly/time reasons. 
    //A better solution should really be devised to fix this.
    enum LynxIRPort {
        FRONT = 0,
        FRONTLEFT = 1,
        REARLEFT = 2,
        FRONTRIGHT = 3,
        REARRIGHT = 4,
        REAR = 5
    }

    interface IRData {
        int GetDistance(LynxIRPort sensorNumber);
    }
}
