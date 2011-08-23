using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class ParameterList {
        private Dictionary<String, Object> parameters = new Dictionary<String, Object>();

        public Object this[String key] {
            get {
                if (parameters.ContainsKey(key)) {
                    return parameters.ContainsKey(key);
                } else {
                    return null;
                }
            }

            set {
                if (parameters.ContainsKey(key)) {
                    parameters[key] = value;
                } else {
                    parameters.Add(key, value);
                }
            }
        }
    }
}
