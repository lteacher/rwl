using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;

namespace RobotInitial.Undo
{
    class UndoOperation
    {
        Workspace _initialState;
        Workspace _finalState;

        public void SetInitialState(Workspace workspace)
        {
            _initialState = (Workspace) workspace.Clone();
        }

        public void SetFinalState(Workspace workspace)
        {
            _finalState = (Workspace) workspace.Clone();
        }

        public void Commit(out Workspace workspace)
        {
            workspace = _finalState;
        }

        public void Rollback(out Workspace workspace)
        {
            workspace = _initialState;
        }
    }
}
