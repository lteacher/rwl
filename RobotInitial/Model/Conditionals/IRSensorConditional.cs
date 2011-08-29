using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class IRSensorConditional : Conditional<bool> {
        public enum Operator {
            EQUAL = 1,
            GREATER = 2,
            LESS = 4,
            EQUALORGREATER = EQUAL | GREATER,
            EQUALORLESS = EQUAL | LESS,
            NOTEQUAL = GREATER | LESS
        }

        public Operator EqualityOperator { get; set; }
        public int Distance { get; set; }
        public int Port { get; set; }

        public IRSensorConditional() {
            Distance = 50;
            Port = 0;
            EqualityOperator = Operator.LESS;
        }

        protected IRSensorConditional(IRSensorConditional other) {
            this.EqualityOperator = other.EqualityOperator;
            this.Distance = other.Distance;
            this.Port = other.Port;
        }

        public void initilize() {
        }

        public void update() {
        }

        public bool evaluate(Protocol protocol) {
            IRData data = protocol.requestIR();
            int actualDistance = data.getDistance(Port);

            if (actualDistance < Distance) {
                return (EqualityOperator & Operator.LESS) == Operator.LESS;
            } else if (actualDistance > Distance) {
                return (EqualityOperator & Operator.GREATER) == Operator.GREATER;
            } else {
                return (EqualityOperator & Operator.EQUAL) == Operator.EQUAL;
            }
        }

        public object Clone() {
            return new IRSensorConditional(this);
        }
    }
}
