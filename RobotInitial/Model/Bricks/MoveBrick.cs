using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {

    class MoveBrick : AbstractBrick, MoveParameters {
        
        public MoveDirection Direction {
            set { Parameters[MoveParameter.DIRECTION.ToString()] = value; }
            get { return (MoveDirection)Parameters[MoveParameter.DIRECTION.ToString()]; }
        }

        public int Duration {
            set { Parameters[MoveParameter.DURATION.ToString()] = value; }
            get { return (int)Parameters[MoveParameter.DURATION.ToString()]; }
        }

        public MoveDurationUnit DurationUnit {
            set { Parameters[MoveParameter.DURATIONUNIT.ToString()] = value; }
            get { return (MoveDurationUnit)Parameters[MoveParameter.DURATIONUNIT.ToString()]; }
        }

        public int Power {
            set { Parameters[MoveParameter.POWER.ToString()] = value; }
            get { return (int)Parameters[MoveParameter.POWER.ToString()]; }
        }

        public int Steering {
            set { Parameters[MoveParameter.STEERING.ToString()] = value; }
            get { return (int)Parameters[MoveParameter.STEERING.ToString()]; }
        }

        public bool BrakeAfterMove {
            set { Parameters[MoveParameter.BRAKEAFTERMOVE.ToString()] = value; }
            get { return (bool)Parameters[MoveParameter.BRAKEAFTERMOVE.ToString()]; }
        }

        public MoveBrick() {
            Direction = MoveDirection.FORWARD;
            Duration = 1;
            DurationUnit = MoveDurationUnit.ROTATIONS;
            Power = 50;
            Steering = 0;
            BrakeAfterMove = true;
        }

        public override void perform(Protocol protocol) {
            protocol.move(this);
        }
    }
}
