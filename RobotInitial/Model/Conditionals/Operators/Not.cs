using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class Not : Conditional<bool> {

        public Conditional<bool> Operand { private set; get; }

        public bool evaluate(Protocol protocol) {
            return !Operand.evaluate(protocol);
        }

        public void initilize() {
            Operand.initilize();
        }

        public void update() {
            Operand.update();
        }

        public Not(Conditional<bool> operand) {
            Operand = operand;
        }
    }
}
