using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Lynx_Server {
    class LynxMessage {
        private const char START = '$';
        private const char SEPERATOR = ',';
        private const byte LEGOCOMMANDSET = 4;

        private readonly byte[] args;
        public byte CheckSum { get; private set; }
        public byte CommandType { get; private set; }
        public byte Length { get { return (byte)(args.Length + 1); } } //length includes command set number, but not checksum

        public byte getArg(int index) {
            return (index >= 0 && index < args.Length) ? args[index] : (byte)0;
        }

        public int numOfArguments() {
            return args.Length;
        }

        public LynxMessage(params byte[] args) {
            this.args = args;
            this.CommandType = LEGOCOMMANDSET;
            this.CheckSum = (byte)(Length ^ CommandType);    //"checksum is a logical XOR of each piece of information, including the length"
            foreach (byte arg in args) {
                CheckSum ^= arg;
            }
        }

        public LynxMessage(String messageStr) {
            String[] tokens = messageStr.Substring(1).Split(SEPERATOR);    //ignore start and split on seperator
            this.args = new byte[tokens.Length - 3];    //doens't contian length, checksum or commandtype

            CommandType = Convert.ToByte(tokens[1]);
            for (int i = 0; i < args.Length; ++i) {
                args[i] = Convert.ToByte(tokens[i + 2]);
            }
            CheckSum = Convert.ToByte(tokens[tokens.Length - 1]);
        }

        public bool isCheckSumCorrect() {
            byte sum = (byte)(Length ^ CommandType);
            foreach (byte num in args) {
                sum ^= num;
            }
            return CheckSum == sum;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append(START);
            sb.Append(Length).Append(SEPERATOR);
            sb.Append(CommandType).Append(SEPERATOR);
            foreach (byte arg in args) {
                sb.Append(arg).Append(SEPERATOR);
            }
            sb.Append(CheckSum);
            return sb.ToString();
        }

        /*
        static void Main() {
            LynxMessage test = new LynxMessage(61, 2, 3, 72, 1, 2, 12, 1, 64);
            LynxMessage copy = new LynxMessage(test.ToString());
            Console.WriteLine(test + " == " + copy + " is " + test.ToString().Equals(copy.ToString()));
            Console.WriteLine("Copy Checksum correct " + copy.isCheckSumCorrect());
            while (true) ;
        }*/
    }
}
