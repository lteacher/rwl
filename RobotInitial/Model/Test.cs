using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace RobotInitial.Model {
    /* old tests, before factory
    public class TestIRSensor : IRData {
        private static int dist = 100;
        public int getDistance(int port) {
            dist -= 10;
            Debug.WriteLine(dist);
            return dist;
        }
    }

    class TestProtocol : Protocol {
        public void move(MoveParameters parameters) {
            Debug.WriteLine(parameters.Direction + " " + parameters.Duration + " " + parameters.DurationUnit + " "
                + parameters.Power + " " + parameters.Steering + " " + parameters.BrakeAfterMove);
        }
        public IRData requestIR() {
            IRData data = new TestIRSensor();
            return data;
        }

        public IMUData requestIMU() { return null;}
        public StatusData requestStatus() { return null; }
    }


    //just trying stuff out
    public class Test {
        public static void test() {
            StartBlock start = new StartBlock();

            LoopBlock loop = new LoopBlock();
            CountConditional count = new CountConditional();
            count.Limit = 10;
            loop.Condition = count;

            SwitchBlock<bool> switchb = new SwitchBlock<bool>();
            switchb.Condition = new RBGConditional();

            MoveBlock truepath = new MoveBlock();
            truepath.Power = 1337;
            truepath.BrakeAfterMove = true;

            MoveBlock falsepath = new MoveBlock();
            falsepath.BrakeAfterMove = false;
            //falsepath.Next = new MoveBlock();

            WaitBlock wait = new WaitBlock();
            wait.WaitUntil = new TimeConditional();

            loop.LoopPath = switchb;
            switchb.mapPath(true, truepath);
            switchb.mapPath(false, falsepath);
            loop.Next = wait;
            wait.Next = new MoveBlock();
            start.Next = loop;

            Executor executor = new Executor(start, new TestProtocol());
            executor.execute();
            Debug.WriteLine("Done");

            //start.Clone();
        }
    }*/
}
