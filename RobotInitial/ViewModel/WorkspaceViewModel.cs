using System;
using System.Diagnostics;
using System.Windows.Input;

using RobotInitial.Command;
using RobotInitial.Model;
using RobotInitial.Properties;
using RobotInitial.Services;
using RobotInitial.Undo;

namespace RobotInitial.ViewModel
{
    class WorkspaceViewModel : ClosableViewModel
    {

        #region Fields
        Workspace _workspace;
        readonly UndoManager _undoManager;
		int _minWidth = 2000;
		int _minHeight = 2000;

        #endregion // Fields

        #region Properties

		public int Width {
			get { return _minWidth; }
		}

		public int Height {
			get { return _minHeight; }
		}

        public bool IsUndoEnabled
        {
            get { return _undoManager.IsUndoEnabled; }
        }

        public bool IsRedoEnabled
        {
            get { return _undoManager.IsRedoEnabled; }
        }

        #endregion // Properties


        public void Undo()
        {
            _undoManager.Undo(out _workspace);
        }

        public void Redo()
        {
            _undoManager.Redo(out _workspace);
        }

		public WorkspaceViewModel() {

		}

        public WorkspaceViewModel(Workspace workspace)
        {
            _workspace = workspace;
            base.DisplayName = _workspace.FileName;
            _undoManager = ServiceLocator.GetService<IUndoService>().CreateUndoManager();
            _undoManager.UndoStackChanged += new EventHandler(OnUndoChanged);
            _undoManager.RedoStackChanged += new EventHandler(OnRedoChanged);

            OnUndoChanged(null, null);
            OnRedoChanged(null, null);
        }

        private void OnUndoChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Firing PropertyChanged on IsUndoEnabled");
            OnPropertyChanged("IsUndoEnabled");
        }

        private void OnRedoChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Firing PropertyChanged on IsRedoEnabled");
            OnPropertyChanged("IsRedoEnabled");
        }

        RelayCommand _dropCommand;

        public ICommand DropCommand
        {
            get
            {
                if (_dropCommand == null)
                {
                    _dropCommand = new RelayCommand(param => this.OnDrop());
                }

                return _dropCommand;
            }
        }

        public bool IsUntitled
        {
            get
            {
                if (_workspace != null)
                {
                    return _workspace.IsUntitled;
                }

                return false;
            }
        }

        void OnDrop()
        {
            Debug.WriteLine("OnDrop called");
        }
    }
}
