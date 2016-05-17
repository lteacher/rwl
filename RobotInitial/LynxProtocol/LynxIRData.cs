using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;

namespace RobotInitial.LynxProtocol {
    struct LynxIRData : IRData {
        private int[] distances;

        public LynxIRData(params int[] args) {
            distances = args.Clone() as int[];
        }

        //public List<int> GetDistances(LynxIRPort port) {
        //    List<int> requestedDistances = new List<int>();
        //    int portInt = (int)port;

        //    for (int i = 0; portInt != 0; ++i, portInt >>= 1) {
        //        if ((portInt & 1) == 1) {
        //            requestedDistances.Add(i >= 0 && i < distances.Length ? distances[i] : -1);
        //        }
        //    }

        //    return requestedDistances;
        //}

        //private static bool IsPowerOfTwo(int x) {
        //    return (x != 0) && ((x & (x - 1)) == 0);
        //}

        public int GetDistance(LynxIRPort port) {
            return distances[(int)port];
        }

        //public static void Main() {
        //    LynxIRData data = new LynxIRData(500, 30, 700, 5123, 12, 56);
        //    data.GetDistances(LynxIRPort.FRONT).ForEach(s => Console.Write(s + " "));
        //    Console.WriteLine();
        //    data.GetDistances(LynxIRPort.FRONTRIGHT | LynxIRPort.REARRIGHT).ForEach(s => Console.Write(s + " "));
        //    Console.WriteLine();
        //    data.GetDistances(LynxIRPort.FRONT | LynxIRPort.FRONTLEFT | LynxIRPort.REARLEFT | LynxIRPort.FRONTRIGHT | LynxIRPort.REARRIGHT | LynxIRPort.REAR).ForEach(s => Console.Write(s + " "));
        //    Console.WriteLine();
        //    data.GetDistances(LynxIRPort.FRONT | LynxIRPort.FRONTLEFT | LynxIRPort.FRONTRIGHT).ForEach(s => Console.Write(s + " "));
        //    while (true) ;
        //}
    }
}
