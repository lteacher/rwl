using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics.Contracts;

namespace RobotInitial.Model {
    abstract class AbstractBrick : Brick {
        public ParameterList Parameters { get; set; }

        public Point Location {
            get { return (Point)Parameters["Location"]; }
            set { Parameters["Location"] = value; }
        }

        public Brick Next {
            get { return Parameters["Next"] as Brick; }
            set { Parameters["Next"] = value; }
        }

        public AbstractBrick() {
            Parameters = new ParameterList();
        }

        public abstract void perform(Protocol protocol);
        public Brick PerformNext { 
            get { return Next; }
        }
    }
}
