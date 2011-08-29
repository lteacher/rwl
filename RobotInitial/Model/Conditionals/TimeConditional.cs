using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace RobotInitial.Model {
    class TimeConditional : Conditional<bool> {
        private Stopwatch timer = new Stopwatch();
        public int Duration { get; set; }

        public TimeConditional() {
            Duration = 1000;
        }

        protected TimeConditional(TimeConditional other) {
            this.Duration = other.Duration;
        }

        public void initilize() {
            timer.Restart();
        }

        public void update() {
        }

        public bool evaluate(Protocol protocol) {
            //will evaulate true when loop is to terminate
            if (timer.ElapsedMilliseconds < Duration) {
                return false;
            } else {
                timer.Stop();
                return true;
            }
        }

        public object Clone() {
            return new TimeConditional(this);
        }
    }
}
