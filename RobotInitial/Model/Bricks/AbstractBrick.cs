using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RobotInitial.Model {
    abstract class AbstractBrick : Brick {
        protected ParameterList parameters = new ParameterList();
         public ParameterList Parameters {
             get { return parameters; }
             set { parameters = value; }
         }

         public Point Location {
             get { return (Point)parameters["Location"]; }
             set { parameters["Location"] = value; }
         }

         public Brick Next {
             get { return parameters["Next"] as Brick; }
             set { parameters["Next"] = value; }
         }

        public abstract void perform(Protocol protocol);
    }
}
