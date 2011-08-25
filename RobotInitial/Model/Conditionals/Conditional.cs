using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    interface Conditional<T> {
        T evaluate(Protocol protocol);
        void initilize();
        void update();
    }
}
