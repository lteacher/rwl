using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class ModelExecutor {
        private Stack<Block> execStack = new Stack<Block>();
        private LinkedList<Block> performAfter = new LinkedList<Block>();

        public StartBlock Start { get; private set; }
        public Protocol Protocol { get; private set; }

        public ModelExecutor(StartBlock start, Protocol protocol) {
            this.Start = start;
            this.Protocol = protocol;
            execStack.Push(Start);
        }

        public void reset() {
            execStack.Clear();
            execStack.Push(Start);
        }

        public bool isDone() {
            return execStack.Count <= 0;
        }

        public void executeOneBlock() {
            if (isDone()) {
                return;
            }

            Block block = execStack.Pop();
            block.perform(Protocol, ref performAfter);

            while (performAfter.Count > 0) {
                if (performAfter.Last() != null) {
                    execStack.Push(performAfter.Last());
                }
                performAfter.RemoveLast();
            }
        }

        public void executeAll() {
            while (!isDone()) {
                executeOneBlock();
            }
        }

        public static void executeAll(StartBlock start, Protocol protocol) {
            new ModelExecutor(start, protocol).executeAll();
        }
    }
}
