using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class Executor {
        public StartBlock Start { get; private set; }
        public Protocol Protocol { get; private set; }

        public Executor(StartBlock start, Protocol protocol) {
            this.Start = start;
            this.Protocol = protocol;
        }

        public void execute() {
            LinkedList<Block> performAfter = new LinkedList<Block>();
            Stack<Block> stack = new Stack<Block>();
            stack.Push(Start);

            while (stack.Count > 0) {
                Block current = stack.Pop();
                if (current == null) continue;
                current.perform(this.Protocol, ref performAfter);
                while (performAfter.Count > 0) {
                    stack.Push(performAfter.Last());
                    performAfter.RemoveLast();
                }
            }
        }

        public static void execute(StartBlock start, Protocol protocol) {
            new Executor(start, protocol).execute();
        }
    }
}
