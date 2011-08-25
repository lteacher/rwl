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

        public Operator EqualityOperator { get; set; }
        public int Distance { get; set; }

        public void initilize() {
        }

        public void update() {
        }

        public bool evaluate(Protocol protocol) {
            IRSensorData data = protocol.readIRSensor();
            switch (EqualityOperator) {
                case Operator.GREATERTHAN:
                    return data.Distance >= Distance;
                case Operator.LESSTHAN:
                default:
                    return data.Distance <= Distance;
            }
        }
    }
}
