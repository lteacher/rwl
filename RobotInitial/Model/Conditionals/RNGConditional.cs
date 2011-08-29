using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class RNGConditional : Conditional<int> {

        private int current;
        private Random rand = new Random();
        public int Max { get; set; }
        public int Min { get; set; }

        public RNGConditional() {
        }

        protected RNGConditional(RNGConditional other) {
            this.Min = other.Min;
            this.Max = other.Max;
            this.current = other.current;
        }

        public void initilize() {
            //rand = new Random();
        }

        public void update() {
            //max is an exclusive bound
            current = rand.Next(Min, Max + 1);
        }

        public int evaluate(Protocol protocol) {
            return current;
        }

        public object Clone() {
            return new RNGConditional(this);
        }
    }
}
