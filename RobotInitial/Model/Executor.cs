using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class ModelExecutor {
        private Stack<Block> execStack = new Stack<Block>();

        public StartBlock Start { get; private set; }
        public Protocol Protocol { get; private set; }

        public ModelExecutor(StartBlock start, Protocol protocol) {
            this.Start = start;
            this.Protocol = protocol;
            execStack.Push(Start);
        }

        public void Reset() {
            execStack.Clear();
            execStack.Push(Start);
        }

        public bool IsDone() {
            return execStack.Count <= 0;
        }

        public void ExecuteOneBlock() {
            if (IsDone()) {
                return;
            }

            LinkedList<Block> performAfter = new LinkedList<Block>();
            Block block = execStack.Pop();

            try {
                block.Perform(Protocol, ref performAfter);
                while (performAfter.Count > 0) {
                    if (performAfter.Last() != null) {
                        execStack.Push(performAfter.Last());
                    }
                    performAfter.RemoveLast();
                }
            } catch (Exception e) {
                //Console.WriteLine("Program threw exception, stop execution");
                //program threw Exception, stop execution
                StopExecution();
                return;
            }

            if (IsDone()) {
                StopExecution();
            }
        }

        public void StopExecution() {
            execStack.Clear();
            Protocol.OnExecutionFinish();
        }

        public void ExecuteAll() {
            while (!IsDone()) {
                ExecuteOneBlock();
            }
        }

        public static void ExecuteAll(StartBlock start, Protocol protocol) {
            new ModelExecutor(start, protocol).ExecuteAll();
        }
    }
}
