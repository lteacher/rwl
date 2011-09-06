using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class IRSensorConditional : Conditional<bool> {
        [Flags]
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
        public int ListeningPort { get; set; }

        internal IRSensorConditional() {
        }

        public override void initilize() {
        }

        public override void update() {
        }

        public override bool evaluate(Protocol protocol) {
            IRData data = protocol.requestIR();
            int actualDistance = data.getDistance(ListeningPort);

            if (actualDistance < Distance) {
                return (EqualityOperator & Operator.LESS) == Operator.LESS;
            } else if (actualDistance > Distance) {
                return (EqualityOperator & Operator.GREATER) == Operator.GREATER;
            } else {
                return (EqualityOperator & Operator.EQUAL) == Operator.EQUAL;
            }
        }
    }
}
