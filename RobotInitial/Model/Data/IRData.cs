using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    //This is lynx specific stuff in the model due fuctionalitly/time reasons. 
    //A better solution should really be devised to fix this.
    [Flags]
    enum LynxIRPort {
        FRONT = 1,
        FRONTLEFT = 2,
        REARLEFT = 4,
        FRONTRIGHT = 8,
        REARRIGHT = 16,
        REAR = 32
    }

    interface IRData {
        List<int> GetDistances(LynxIRPort sensorNumber);
    }
}
