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

        public override void Perform(Protocol protocol, ref LinkedList<Block> performAfter) {
            protocol.OnExecutionStart();
            performAfter.AddFirst(Next);
        }

        public override object Clone() {
            return new StartBlock(this);
        }

        //to send the program over the network
        public void Serialise(Stream stream) {
            MemoryStream memoryStream = new MemoryStream();
            serialiser.Serialize(memoryStream, this);
            Network.send(memoryStream, stream);
        }

        public static StartBlock Deserialise(Stream stream) {
             MemoryStream memoryStream = Network.recieve(stream);
             return serialiser.Deserialize(memoryStream) as StartBlock;
        }

        private static readonly String[] indentOn = { "Switch", "Case", "Do"};
        private static readonly String[] unindentOn = { "EndSwitch", "EndCase", "Until" };

        public override string ToString() {
            string code = "Start\n" + this.Next;
            string formattedCode = "";
            int level = 0;

            foreach (string line in code.Split('\n')) {
                if (unindentOn.Any(s => line.StartsWith(s))) {
                    --level;
                }
                formattedCode += new String('\t', level) + line + '\n';
                if (indentOn.Any(s => line.StartsWith(s))) {
                    ++level;
                } 
            }

            return formattedCode;
        }
    }
}
