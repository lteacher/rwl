using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;

namespace RobotInitial.LynxProtocol {
    struct LynxIRData : IRData {
        enum Port {
            FRONT,
            FRONTLEFT,
            REARLEFT,
            FRONTRIGHT,
            REARRIGHT,
            REAR
        }

        //private const int NUMOFPORTS = 6;
        private int[] distances;

        public LynxIRData(params int[] args) {
            distances = args.Clone() as int[];
        }

        public int GetDistance(int port) {
            return (port >= 0 && port < distances.Length) ? distances[port] : -1;
        }
    }
}
