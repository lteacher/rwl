using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    enum Axis {
        X = 0,
        Y = 1,
        Z = 2
    }

    interface IMUData {
        int getAccelerometer(Axis axis);
        int getGyroscope(Axis axis);
        int getMagnetometer(Axis axis);
    }
}
