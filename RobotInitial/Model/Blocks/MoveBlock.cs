using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {

    class MoveBlock : AbstractBlock, MoveParameters {

        public MoveDirection Direction { get; set; }
        public int Duration { get; set; }
        public int Power { get; set; }
        public int Steering { get; set; }
        public bool BrakeAfterMove { get; set; }
        public MoveDurationUnit DurationUnit { get; set; }

        public MoveBlock() {
            Direction = MoveDirection.FORWARD;
            Duration = 1;
            DurationUnit = MoveDurationUnit.ROTATIONS;
            Power = 50;
            Steering = 0;
            BrakeAfterMove = true;
        }

        protected MoveBlock(MoveBlock other) : base(other) {
            this.Direction = other.Direction;
            this.Duration = other.Duration;
            this.Power = other.Power;
            this.Steering = other.Steering;
            this.BrakeAfterMove = other.BrakeAfterMove;
            this.DurationUnit = other.DurationUnit;
        }

        public override void perform(Protocol protocol) {
            protocol.move(this);
        }

        public override object Clone() {
            return new MoveBlock(this);
        }
    }
}
