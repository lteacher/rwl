using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class Not : Conditional<bool> {

        public Conditional<bool> Operand { private set; get; }

        internal Not(Conditional<bool> operand) {
            Operand = operand;
        }

        public override bool Evaluate(Protocol protocol) {
            return !Operand.Evaluate(protocol);
        }

        public override void Initilize() {
            Operand.Initilize();
        }

        public override void Update() {
            Operand.Update();
        }

        public override object Clone() {
            Conditional<bool> clonedOperand = null;
            if (this.Operand != null) {
                clonedOperand = this.Operand.Clone() as Conditional<bool>;
            }
            Not clone = new Not(clonedOperand);
            return clone;
        }
    }
}
