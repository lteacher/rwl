using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RobotInitial.Undo;

namespace RobotInitial.Services
{
    interface IUndoService
    {
        UndoManager CreateUndoManager();
    }
}
