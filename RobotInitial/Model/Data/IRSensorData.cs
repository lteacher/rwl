using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    interface IRSensorData {
        //im guessing you get all sensor data in one read?
        //temp, needs to be fixed later on...
        int getDistance(int port);
    }
}
