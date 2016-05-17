using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.LynxProtocol {
    //immutable
    class LynxMessage {
        private const char START = '$';
        private const char SEPERATOR = ',';

        public byte Length { get { return (byte)(args.Length); } }
        public byte CheckSum { get; private set; }

        private readonly byte[] args;
        public byte GetArg(int index) {
            return (index >= 0 && index < args.Length) ? args[index] : (byte)0;
        }

        public LynxMessage(params byte[] args) {
            this.args = args.Clone() as byte[];
            this.CheckSum = this.CalcCheckSum();
        }

        public LynxMessage(String messageStr) {
            if (messageStr[0] != START) {
                throw new InvalidMessageFormatException();
            }
            messageStr = messageStr.Substring(1);   //remove START

            String[] tokens = messageStr.Split(SEPERATOR);
            this.args = new byte[tokens.Length - 2];    //args doesn't include checksum or length

            try {
                for (int i = 0; i < args.Length; ++i) {
                    args[i] = Convert.ToByte(tokens[i + 1]);
                }
                CheckSum = Convert.ToByte(tokens[tokens.Length - 1]);   //use provided checksum, may not be correct
            } catch (FormatException e) {
                throw new InvalidMessageFormatException();
            } catch (OverflowException e) {
                throw new InvalidMessageFormatException();
            }
        }

        //"checksum is a logical XOR of each piece of information, including the length"
        private byte CalcCheckSum() {
            byte sum = Length;    
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
            foreach (byte arg in args) {
                sb.Append(arg).Append(SEPERATOR);
            }
            sb.Append(CheckSum);
            return sb.ToString();
        }

        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            } else if (ReferenceEquals(obj, this)) {
                return true;
            }

            LynxMessage other = obj as LynxMessage;
            if (other == null) {
                return false;
            }

            //check length now so we don't go out of bounds in the array comparison
            if (this.Length != other.Length) {
                return false;
            }

            for (int i = 0; i < this.Length; ++i) {
                if (this.args[i] != other.args[i]) {
                    return false;
                }
            }

            return this.CheckSum == other.CheckSum;
        }

        /*
        static void Main() {
            LynxMessage test = new LynxMessage(1, 30, 1, 4, 5, 6, 7, 8, 9);
            Console.WriteLine(test.ToString());
            LynxMessage copy = new LynxMessage(test.ToString());
            Console.WriteLine(test + " == " + copy + " is " + test.ToString().Equals(copy.ToString()));
            Console.WriteLine("Copy Checksum correct? " + copy.IsCheckSumCorrect());
            while (true) ;
        }
        */
    }
}
