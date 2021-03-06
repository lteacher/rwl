﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    [Serializable()]
    class RNGConditional : Conditional<int> {

        private int current;
        private Random rand;
        public int Max { get; set; }
        public int Min { get; set; }

        internal RNGConditional() {
        }

        public override void Initilize() {
            rand = new Random();
        }

        public override void Update() {
            //max is an exclusive bound
            current = rand.Next(Min, Max + 1);
        }

        public override int Evaluate(Protocol protocol) {
            return current;
        }

        public override string ToString() {
            return "Random Boolean";
        }
    }
}
