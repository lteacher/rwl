using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class ModelExecutor {
        private Stack<Block> execStack = new Stack<Block>();
        private volatile bool finishCalled = false;
        private volatile bool paused = false;

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

        public bool WasTerminated()
        {
            return finishCalled;
        }

        public bool IsDone() {
            return execStack.Count <= 0;
        }

        public void ExecuteOneBlock() {
            if (IsDone() || WasTerminated()) {
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
                execStack.Clear();
                Console.WriteLine("An error occured during the execution of the program");
                Console.WriteLine(e);
                return;
            }

            if (IsDone()) {
                StopExecution();
            }
        }

        public void Pause() {
                paused = true;
                //repeated pauses is handled in the protocol with synchrnisation
                Protocol.Pause();
        }

        public void Resume() {
                paused = false;
                //repeated resumes is handled in the protocol with synchrnisation
                Protocol.Resume();
        }

        public void StopExecution() {
            //ensure finish is called only once
            lock (this) {
                if (!finishCalled) {
                    Protocol.OnExecutionFinish();
                    finishCalled = true;
                }
            }
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
