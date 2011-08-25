using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace RobotInitial.Model {
    class TimeConditional : Conditional<bool> {
        private Stopwatch timer = new Stopwatch();
        public int Duration { get; set; }

        public void initilize() {
            timer.Restart();
        }

        public void update() {
        }

        public bool evaluate(Protocol protocol) {
            if (timer.ElapsedMilliseconds > Duration) {
                timer.Stop();
                return true;
            } else {
                return false;
            }
        }
    }
}
