using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    [Serializable()]
    abstract class Conditional<T> : ICloneable {
        public abstract T Evaluate(Protocol protocol);
        public abstract void Initilize();
        public abstract void Update();

        //shallow clone, will overide if deep is needed
        public virtual object Clone() {
            return this.MemberwiseClone();
        }

        //void uninitilize();
        //bool isInitilized();
    }
}
