using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class Not : Conditional<bool> {

        public Conditional<bool> Operand { private set; get; }

        public Not() {
        }

        protected Not(Not other) {
            this.Operand = (other.Operand == null) ? null : other.Operand.Clone() as Conditional<bool>;
        }

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

        public object Clone() {
            return new Not(this);
        }
    }
}
