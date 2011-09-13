using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Undo;

namespace RobotInitial.Services
{
    class UndoService : IUndoService
    {
        public UndoManager CreateUndoManager()
        {
            return new UndoManager();
        }
    }
}
