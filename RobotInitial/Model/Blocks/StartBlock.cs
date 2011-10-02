using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LynxTest2.Communications;

namespace RobotInitial.Model {
    [Serializable()]
    class StartBlock : AbstractBlock {
        private static BinaryFormatter serialiser = new BinaryFormatter();

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
            MemoryStream memoryStream = new MemoryStream();
            serialiser.Serialize(memoryStream, this);
            Network.send(memoryStream, stream);
        }

        public static StartBlock deserialise(Stream stream) {
             MemoryStream memoryStream = Network.recieve(stream);
             return serialiser.Deserialize(memoryStream) as StartBlock;
        }
    }
}
