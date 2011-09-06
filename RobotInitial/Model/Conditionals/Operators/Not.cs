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

        public override bool evaluate(Protocol protocol) {
            return !Operand.evaluate(protocol);
        }

        public override void initilize() {
            Operand.initilize();
        }

        public override void update() {
            Operand.update();
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
