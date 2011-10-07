using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    interface IMUData {
        Vector3 Accelerometer { get; }
        Vector3 Gyroscope { get; }
        Vector3 Magnetometer { get; }
    }
}
