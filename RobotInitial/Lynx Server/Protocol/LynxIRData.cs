﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;

namespace RobotInitial.Lynx_Server{
    struct LynxIRData : IRData {
        enum Port {
            FRONT,
            FRONTRIGHT,
            REARRIGHT,
            REAR,
            REARLEFT,
            FRONTLEFT
        }

        //private const int NUMOFPORTS = 6;
        private int[] distances;

        public LynxIRData(params int[] args) {
            distances = args.Clone() as int[];
        }

        public int getDistance(int port) {
            return (port >= 0 && port < distances.Length) ? distances[port] : -1;
        }
    }
}