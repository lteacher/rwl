using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {

    class MoveBrick : AbstractBrick, MoveParameters {
        
        public MoveDirection Direction {
            set { parameters[MoveParameter.DIRECTION.ToString()] = value; }
            get { return (MoveDirection)parameters[MoveParameter.DIRECTION.ToString()]; }
        }

        public int Duration {
            set { parameters[MoveParameter.DURATION.ToString()] = value; }
            get { return (int)parameters[MoveParameter.DIRECTION.ToString()]; }
        }

        public MoveDurationUnit DurationUnit {
            set { parameters[MoveParameter.DURATIONUNIT.ToString()] = value; }
            get { return (MoveDurationUnit)parameters[MoveParameter.DURATIONUNIT.ToString()]; }
        }

        public int Power {
            set { parameters[MoveParameter.POWER.ToString()] = value; }
            get { return (int)parameters[MoveParameter.POWER.ToString()]; }
        }

        public int Steering {
            set { parameters[MoveParameter.STEERING.ToString()] = value; }
            get { return (int)parameters[MoveParameter.STEERING.ToString()]; }
        }

        public bool BrakeAfterMove {
            set { parameters[MoveParameter.BRAKEAFTERMOVE.ToString()] = value; }
            get { return (bool)parameters[MoveParameter.BRAKEAFTERMOVE.ToString()]; }
        }

        public MoveBrick() {
            Direction = MoveDirection.FORWARD;
            Duration = 1;
            DurationUnit = MoveDurationUnit.ROTATIONS;
            Power = 50;
            Steering = 0;
        }

        public override void perform(Protocol protocol) {
            protocol.move(this);
            //if (Next != null) {
            //    Next.perform(protocol);
            //}
        }
    }
}
