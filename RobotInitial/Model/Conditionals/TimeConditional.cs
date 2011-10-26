using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace RobotInitial.Model {
    [Serializable()]
    class TimeConditional : Conditional<bool> {
        private Stopwatch timer;
        public int Duration { get; set; }

        internal TimeConditional() {
        }

        public override void Initilize() {
            timer = new Stopwatch();
            timer.Start();
        }

        public override void Update() {
        }

        public override bool Evaluate(Protocol protocol) {
            bool done = timer.ElapsedMilliseconds >= Duration;

            if (done) {
                timer.Stop();   //probably not necessary :)
            }

            return done; //evaulate true when loop is to terminate
        }
    }
}
