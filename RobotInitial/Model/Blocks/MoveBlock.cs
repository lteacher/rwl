using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    [Serializable()]
    class MoveBlock : AbstractBlock, MoveParameters {
        private const int MINPOWER = 0;
        private const int MAXPOWER = 100;
        
        public MoveDirection RightDirection { get; set; }
        public MoveDirection LeftDirection { get; set; }
        public MoveDurationUnit DurationUnit { get; set; }
        public bool BrakeAfterMove { get; set; }

        int rightPower;
        public int RightPower {
            get { return rightPower; }
            set { rightPower = Clamp(value, MINPOWER, MAXPOWER); }
        }

        int leftPower;
        public int LeftPower {
            get { return leftPower; }
            set { leftPower = Clamp(value, MINPOWER, MAXPOWER); }
        }

        float leftDuration;
        public float LeftDuration {
            get { return leftDuration; }
            set { leftDuration = Math.Max(value, 0f); }
        }

        float rightDuration;
        public float RightDuration {
            get { return rightDuration; }
            set { rightDuration = Math.Max(value, 0f); }
        }

        private int Clamp(int value, int min, int max)  {
            return Math.Max(min, Math.Min(max, value));
        }

        internal MoveBlock() {
        }

        protected MoveBlock(MoveBlock other)
            : base(other) {
            this.RightDirection = other.RightDirection;
            this.LeftDirection = other.LeftDirection;
            this.RightPower = other.RightPower;
            this.LeftPower = other.LeftPower;
            this.RightDuration = other.RightDuration;
            this.LeftDuration = other.LeftDuration;
            this.DurationUnit = other.DurationUnit;
            this.BrakeAfterMove = other.BrakeAfterMove;
        }

        public override void Perform(Protocol protocol, ref LinkedList<Block> performAfter) {
            protocol.Move(this);
            performAfter.AddFirst(Next);
        }

        public override object Clone() {
            return new MoveBlock(this);
        }

        public override string ToString() {
            string left = "Left(" + LeftDirection + " " + LeftPower + "% " + LeftDuration + " " + DurationUnit + ")";
            string right = "Right(" + RightDirection + " " + RightPower + "% " + RightDuration + " " + DurationUnit + ")";
            return "Move " + left + " " + right + "\n" + this.Next;
        }
    }
}
