using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class RNGConditional : Conditional<int> {

        private int current;
        private Random rand;
        public int Max { get; set; }
        public int Min { get; set; }

        internal RNGConditional() {
        }

        public override void initilize() {
            rand = new Random();
        }

        public override void update() {
            //max is an exclusive bound
            current = rand.Next(Min, Max + 1);
        }

        public override int evaluate(Protocol protocol) {
            return current;
        }
    }
}
