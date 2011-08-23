using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class IRSensorConditional : Conditional<bool> {
        public enum Operator {
            GREATERTHAN,
            LESSTHAN
        }

        private Operator op = Operator.GREATERTHAN;
        public Operator EqualityOperator {
            set { op = value; }
            get { return op; }
        }

        private int distance = 50;
        public int Distance {
            get { return distance; }
            set { distance = Math.Max(0, value); }
        }


        public bool evaluate(Protocol protocol) {
            IRSensorData data = protocol.readIRSensor();
            switch (op) {
                case Operator.GREATERTHAN:
                    return data.Distance >= distance;
                case Operator.LESSTHAN:
                default:
                    return data.Distance <= distance;
            }
        }
    }
}
