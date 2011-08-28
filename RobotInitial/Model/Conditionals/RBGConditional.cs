using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    //random boolean generator
    class RBGConditional : Conditional<bool> {

        private bool current;
        private Random rand = new Random();

        public void initilize() {
            //rand = new Random();  //was giving lame results
        }

        public void update() {
            //max is an exclusive bound
            current = rand.Next(0, 2) == 1;
        }

        public bool evaluate(Protocol protocol) {
            return current;
        }
    }
}
