﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    //random boolean generator
    [Serializable()]
    class RBGConditional : Conditional<bool> {

        private bool current;
        private Random rand;

        internal RBGConditional() {
        }

        public override void initilize() {
            rand = new Random();
        }

        public override void update() {
            //max is an exclusive bound
            current = rand.Next(0, 2) == 1;
        }

        public override bool evaluate(Protocol protocol) {
            return current;
        }
    }
}
