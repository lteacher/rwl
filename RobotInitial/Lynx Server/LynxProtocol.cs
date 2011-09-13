using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;

namespace RobotInitial.Lynx_Server {
    class LynxProtocol : Protocol {
        public void move(MoveParameters parameters) {
            throw new NotImplementedException();
        }

        public IRData requestIR() {
            throw new NotImplementedException();
        }

        public IMUData requestIMU() {
            throw new NotImplementedException();
        }

        public StatusData requestStatus() {
            throw new NotImplementedException();
        }
    }
}
