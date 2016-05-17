using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;
using RobotInitial.Model;

namespace RobotInitial.Undo
{
    class UndoManager
    {
        readonly Stack<UndoOperation> _undoStack = new Stack<UndoOperation>();
        readonly Stack<UndoOperation> _redoStack = new Stack<UndoOperation>();

        UndoOperation _currentOperation;

        bool _isTransacting = false;

        public bool IsUndoEnabled { get { return _undoStack.Count != 0; } }
        public bool IsRedoEnabled { get { return _redoStack.Count != 0; } }

        public bool IsInTransaction { get { return _isTransacting; } }

        public void StartTransaction(Workspace workspace)
        {
            Debug.Assert(!_isTransacting, "Call to StartTransaction while already transacting :-(");
            _isTransacting = true;
            _currentOperation = new UndoOperation();
            _currentOperation.SetInitialState(workspace);
        }

        public void CommitTransation(Workspace workspace)
        {
            Debug.Assert(_isTransacting, "Call to CommitTransaction while not transacting :-(");
            _currentOperation.SetFinalState(workspace);
            PushToUndo(_currentOperation);
            _currentOperation = null;
            _isTransacting = false;
        }

        public void RollbackTransaction(out Workspace workspace)
        {
            Debug.Assert(_isTransacting, "Call to RollbackTransaction while not transacting :-(");
            _currentOperation.Rollback(out workspace);
            _currentOperation = null;
            _isTransacting = false;
        }

        private void PushToUndo(UndoOperation op)
        {
            _undoStack.Push(op);
            UndoStackChanged(this, new EventArgs());
            _redoStack.Clear();
            RedoStackChanged(this, new EventArgs());
        }

        public void Undo(out Workspace workspace)
        {
            UndoOperation op = _undoStack.Pop();
            UndoStackChanged(this, new EventArgs());
            op.Rollback(out workspace);
            _redoStack.Push(op);
            RedoStackChanged(this, new EventArgs());
        }

        public void Redo(out Workspace workspace)
        {
            UndoOperation op = _redoStack.Pop();
            RedoStackChanged(this, new EventArgs());
            op.Commit(out workspace);
            _undoStack.Push(op);
            UndoStackChanged(this, new EventArgs());
        }

        public event EventHandler UndoStackChanged;
        public event EventHandler RedoStackChanged;

    }
}
