using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics.Contracts;

namespace RobotInitial.Model {
    abstract class AbstractBlock : Block {
        public ParameterList Parameters { get; set; }

        public Point Location {
            get { return (Point)Parameters["Location"]; }
            set { Parameters["Location"] = value; }
        }

        public Block Next {
            get { return Parameters["Next"] as Block; }
            set { Parameters["Next"] = value; }
        }

        public AbstractBlock() {
            Parameters = new ParameterList();
        }

        public abstract void perform(Protocol protocol);
        public Block PerformNext { 
            get { return Next; }
        }
    }
}
