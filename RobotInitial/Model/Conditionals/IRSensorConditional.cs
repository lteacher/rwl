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

        //public static string ToString(this Operator _operator) {
        //    switch (_operator) {
        //        case Operator.EQUAL:
        //            return "==";
        //        case Operator.GREATER:
        //            return ">";
        //        case Operator.LESS:
        //            return "<";
        //        case Operator.EQUALORGREATER:
        //            return ">=";
        //        case Operator.EQUALORLESS:
        //            return "<=";
        //        case Operator.NOTEQUAL:
        //            return "!=";
        //    }
        //    return "undefined";
        //}
    }

    public enum LogicalOperator {
        OR,
        AND
    }

    //static class LogicalOperatorExt {
    //    public static string ToString(this LogicalOperator _operator) {
    //        switch (_operator) {
    //            case LogicalOperator.OR:
    //                return "||";
    //            case LogicalOperator.AND:
    //                return "&&";
    //        }
    //        return "undefined";
    //    }
    //}

    [Serializable()]
    class IRSensorConditional : Conditional<bool> {

        public Operator EqualityOperator { get; set; }
        public LogicalOperator LogicalOperator { get; set; }
        //public LynxIRPort IRSensors { get; set; }
        private readonly Dictionary<LynxIRPort, int> distances = new Dictionary<LynxIRPort, int>();
        private readonly Dictionary<LynxIRPort, bool> portStates = new Dictionary<LynxIRPort, bool>();  //could probably be a single int.,.. but oh well

        //activate/deactivate a port
        public void SetPortState(LynxIRPort port, bool state) {
            portStates[port] = state;
            if (!distances.ContainsKey(port)) {
                distances[port] = DefaultModelFactory.IRDISTANCEDEFAULT;
            }
        }

        public bool GetPortState(LynxIRPort port) {
            return portStates.ContainsKey(port) ? portStates[port] : false;
        }

        public void SetDistance(LynxIRPort port, int distance) {
            distances[port] = distance;
        }

        public int GetDistance(LynxIRPort port) {
            return distances[port];
            //return distances.ContainsKey(port) ? distances[port] : DefaultModelFactory.IRDISTANCEDEFAULT;
        }

        internal IRSensorConditional() {
        }

        public override void Initilize() {
        }

        public override void Update() {
        }

        public override bool Evaluate(Protocol protocol) {
            IRData data = protocol.RequestIR();
            IEnumerable<KeyValuePair<LynxIRPort, int>> enabledDistances = distances.Where(kvp => portStates[kvp.Key]);
            if (LogicalOperator == LogicalOperator.AND) {
                return enabledDistances.All(kvp => EqualityOperator.Evaluate(kvp.Value, data.GetDistance(kvp.Key)));
            } else {  //if (LogicalOperator == LogicalOperator.OR) 
                return enabledDistances.Any(kvp => EqualityOperator.Evaluate(kvp.Value, data.GetDistance(kvp.Key)));
            }
        }

        public override string ToString() {
            IEnumerable<KeyValuePair<LynxIRPort, int>> enabledDistances = distances.Where(kvp => portStates[kvp.Key]);
            string expression = "";

            foreach (KeyValuePair<LynxIRPort, int> kvp in enabledDistances) {
                expression += kvp.Key + " " + EqualityOperator + " " + kvp.Value;
                if (kvp.Key != enabledDistances.Last().Key) {
                    expression += " " + LogicalOperator + " ";
                }
            }

            return expression;
        }

    //    static void Main() {
    //        DefaultModelFactory fact = DefaultModelFactory.Instance;
    //        IRSensorConditional ir = fact.CreateIRSensorConditional();
    //        ir.SetPortState(LynxIRPort.FRONTLEFT, true);
    //        ir.SetDistance(LynxIRPort.FRONTLEFT, 1338);
    //        ir.SetPortState(LynxIRPort.REAR, true);
    //        ir.SetPortState(LynxIRPort.FRONTLEFT, false);
    //        ir.SetPortState(LynxIRPort.REARLEFT, true);
    //        ir.SetDistance(LynxIRPort.REARLEFT, 2121);
    //        Console.WriteLine(ir.ToString());
    //        while (true) { }
    //}
    }
}
