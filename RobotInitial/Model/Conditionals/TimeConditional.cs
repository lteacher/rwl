using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace RobotInitial.Model {
    class TimeConditional : Conditional<bool> {
        Stopwatch timer = new Stopwatch();

        private int duration = 1000;
        public int Duration {
            set { duration = Math.Max(0, value); }
            get { return duration; }
        }

        public void startTimer() {
            timer.Start();
        }

        public bool evaluate(Protocol protocol) {
            if (!timer.IsRunning) {
                timer.Start();
            }

            if (timer.ElapsedMilliseconds > duration) {
                timer.Reset();
                return true;
            } else {
                return false;
            }
        }
    }
}
