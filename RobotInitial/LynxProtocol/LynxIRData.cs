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

        public List<int> GetDistances(LynxIRPort port) {
            List<int> indexes = new List<int>();
            int portInt = (int)port;
            for (int i = 0; port != 0; ++i, portInt >>= 1) {
                if ((portInt & 1) == 1) {
                    indexes.Add(i);
                }
            }

            List<int> requestedDistances = new List<int>();
            foreach (int i in indexes) {
                requestedDistances.Add(i >= 0 && i < distances.Length ? distances[i] : -1);
            }

            return requestedDistances;
        }
    }
}
