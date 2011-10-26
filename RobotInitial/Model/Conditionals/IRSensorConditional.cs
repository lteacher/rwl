using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    [Serializable()]
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

        public override void Initilize() {
        }

        public override void Update() {
        }

        public override bool Evaluate(Protocol protocol) {
            IRData data = protocol.RequestIR();
            int actualDistance = data.GetDistance(ListeningPort);

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
