using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class CountConditional : Conditional<bool> {
        private int count = 0;
        public int Limit { get; set; }

        public CountConditional() {
            Limit = 5;
        }

        protected CountConditional(CountConditional other) {
            this.Limit = other.Limit;
            this.count = other.count;
        }

        public void initilize() {
            count = 0;
        }

        public void update() {
            ++count;
        }

        public bool evaluate(Protocol protocol) {
            //will evaulate true when loop is to terminate
            return count >= Limit;
        }

        public object Clone() {
            return new CountConditional(this);
        }

    }
}
