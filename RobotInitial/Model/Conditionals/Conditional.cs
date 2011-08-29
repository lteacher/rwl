using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    interface Conditional<T> : ICloneable {
        T evaluate(Protocol protocol);
        void initilize();
        void update();
        //void uninitilize();
        //bool isInitilized();
    }
}
