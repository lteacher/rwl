using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace RobotInitial.Model {
    [Serializable()]
    class StartBlock : AbstractBlock {

        internal StartBlock() {
        }

        protected StartBlock(StartBlock other) : base(other) {
        }

        public override void perform(Protocol protocol, ref LinkedList<Block> performAfter) {
            performAfter.AddFirst(Next);
        }

        public override object Clone() {
            return new StartBlock(this);
        }

        //to send the program over the network
        public void serialise(Stream stream) {
            ModelSerialiser.serialise(stream, this);
        }

        public static StartBlock deserialise(Stream stream) {
            return ModelSerialiser.deserialise(stream) as StartBlock;
        }
    }
}
