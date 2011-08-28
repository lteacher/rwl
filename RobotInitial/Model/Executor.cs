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
            this.execute(Start);
        }

        private void execute(Block block) {
            while (block != null) {
                block.perform(Protocol);
                if (block is CompositeBlock) {
                    this.execute((block as CompositeBlock).PathToPerform);
                }
                block = block.NextToPerform;
            }
        }
    }
}
