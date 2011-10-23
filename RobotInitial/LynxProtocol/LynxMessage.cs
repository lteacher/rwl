using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.LynxProtocol {
    //immutable
    class LynxMessage {
        private const char START = '$';
        private const char SEPERATOR = ',';

        public byte Length { get { return (byte)(args.Length + 2); } } //+2 since it includes Command and CommandSet, excludes checksum
        public LynxCommandSet CommandSet { get; private set; }
        public byte Command { get; private set; }
        public byte CheckSum { get; private set; }

        private readonly byte[] args;
        public byte GetArg(int index) {
            return (index >= 0 && index < args.Length) ? args[index] : (byte)0;
        }

        public int NumOfArguments() {
            return args.Length;
        }

        public LynxMessage(LynxCommandSet set, byte command, params byte[] args) {
            this.CommandSet = set;
            this.Command = command;
            this.args = args.Clone() as byte[];
            this.CheckSum = this.CalcCheckSum();
        }

        public LynxMessage(String messageStr) {
            if (messageStr[0] != START) {
                throw new InvalidMessageFormatException();
            }
            messageStr = messageStr.Substring(1);   //remove START

            String[] tokens = messageStr.Split(SEPERATOR);
            this.args = new byte[tokens.Length - 4];

            try {
                CommandSet = (LynxCommandSet)Convert.ToByte(tokens[1]);
                Command = Convert.ToByte(tokens[2]);
                for (int i = 0; i < args.Length; ++i) {
                    args[i] = Convert.ToByte(tokens[i + 3]);
                }
                CheckSum = Convert.ToByte(tokens[tokens.Length - 1]);
            } catch (FormatException e) {
                throw new InvalidMessageFormatException();
            } catch (OverflowException e) {
                throw new InvalidMessageFormatException();
            }
        }

        //"checksum is a logical XOR of each piece of information, including the length"
        private byte CalcCheckSum() {
            byte sum = (byte)(Length ^ (byte)CommandSet ^ Command);    
            foreach (byte arg in args) {
                sum ^= arg;
            }
            return sum;
        }

        public bool IsCheckSumCorrect() {
            return CheckSum == this.CalcCheckSum();
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append(START);
            sb.Append(Length).Append(SEPERATOR);
            sb.Append((byte)CommandSet).Append(SEPERATOR);
            sb.Append(Command).Append(SEPERATOR);
            foreach (byte arg in args) {
                sb.Append(arg).Append(SEPERATOR);
            }
            sb.Append(CheckSum);
            return sb.ToString();
        }

        /*
        static void Main() {
            LynxMessage test = new LynxMessage(LynxCommandSet.LEFTPASS, 2, 3, 4, 5, 6, 7, 8, 9);
            Console.WriteLine(test.ToString());
            LynxMessage copy = new LynxMessage(test.ToString());
            Console.WriteLine("2");
            Console.WriteLine(test + " == " + copy + " is " + test.ToString().Equals(copy.ToString()));
            Console.WriteLine("Copy Checksum correct? " + copy.IsCheckSumCorrect());
            while (true) ;
        }
        */
    }
}
