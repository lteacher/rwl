using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace RobotInitial.Model {
    public class RandomIRSensor : IRSensorData {
        private int dist = new Random().Next(100);
        public int Distance {
            get { return dist; }
        }
    }

    class TestProtocol : Protocol {
        public void move(MoveParameters parameters) {
            Debug.WriteLine(parameters.Direction + " " + parameters.Duration + " " + parameters.DurationUnit + " "
                + parameters.Power + " " + parameters.Steering + " " + parameters.BrakeAfterMove);
        }
        public IRSensorData readIRSensor() {
            return new RandomIRSensor();
        }
    }


    public class Test {
        public static void test() {
            StartBrick start = new StartBrick();

            Brick brick = start;
            for (int i = 0; i < 10; i++) {
                brick.Next = new MoveBrick();
                brick = brick.Next;
            }

            Executor executor = new Executor(start, new TestProtocol());
            executor.execute();
        }
    }
}
