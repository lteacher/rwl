using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial {
	class VirtualMachineOwnershipException : ApplicationException  {
	}

    class LynxBusyException : ApplicationException {
    }

    class InvalidMessageFormatException : ApplicationException {
    }

    class LynxIsNotAliveException : ApplicationException {
    }

    class AlreadyConnectedException : ApplicationException {
    }

    class NotConnectedException : ApplicationException {
    }

    class LynxNotPausedException : ApplicationException {
    }
}
