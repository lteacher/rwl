using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    [Serializable()]
    class MoveBlock : AbstractBlock, MoveParameters {
        public MoveDirection RightDirection { get; set; }
        public MoveDirection LeftDirection { get; set; }
        public int RightPower { get; set; }
        public int LeftPower { get; set; }
        public float Duration { get; set; }
        public MoveDurationUnit DurationUnit { get; set; }
        public bool BrakeAfterMove { get; set; }

        internal MoveBlock() {
        }

        protected MoveBlock(MoveBlock other) : base(other) {
            this.RightDirection = other.RightDirection;
            this.LeftDirection = other.LeftDirection;
            this.RightPower = other.RightPower;
            this.LeftPower = other.LeftPower;
            this.Duration = other.Duration;
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
    }
}
