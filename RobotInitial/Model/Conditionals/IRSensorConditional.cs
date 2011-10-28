using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    [Flags]
    public enum Operator {
        EQUAL = 1,
        GREATER = 2,
        LESS = 4,
        EQUALORGREATER = EQUAL | GREATER,
        EQUALORLESS = EQUAL | LESS,
        NOTEQUAL = GREATER | LESS
    }

    static class OperatorExt {
        public static bool Evaluate(this Operator _operator, int operandLeft, int operandRight) {
            switch (_operator) {
                case Operator.EQUAL:
                    return operandLeft == operandRight;
                case Operator.GREATER:
                    return operandLeft > operandRight;
                case Operator.LESS:
                    return operandLeft < operandRight;
                case Operator.EQUALORGREATER:
                    return operandLeft >= operandRight;
                case Operator.EQUALORLESS:
                    return operandLeft <= operandRight;
                case Operator.NOTEQUAL:
                    return operandLeft != operandRight;
            }
            return false;
        }
    }

    [Serializable()]
    class IRSensorConditional : Conditional<bool> {

        public Operator EqualityOperator { get; set; }
        public int Distance { get; set; }
        public LynxIRPort IRSensors { get; set; }

        internal IRSensorConditional() {
        }

        public override void Initilize() {
        }

        public override void Update() {
        }

        public override bool Evaluate(Protocol protocol) {
            IRData data = protocol.RequestIR();
            List<int> actualDistances = data.GetDistances(IRSensors);
            return actualDistances.Any(x => EqualityOperator.Evaluate(x, Distance));
        }

        public override string ToString() {
            return "IRSensor " + IRSensors + " " + EqualityOperator + " " + Distance;
        }
    }
}
