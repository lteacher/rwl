using System;
using System.Diagnostics;
using System.Windows.Input;

using RobotInitial.Command;
using RobotInitial.Model;
using RobotInitial.Properties;
using RobotInitial.Services;
using RobotInitial.Undo;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using RobotInitial.View;
using System.Windows.Controls;

namespace RobotInitial.ViewModel {
    class WorkspaceViewModel : ClosableViewModel, INotifyPropertyChanged {

        #region Fields
        Workspace _workspace;
        readonly UndoManager _undoManager;
        double _minWidth = Application.Current.MainWindow.RenderSize.Width;
        double _minHeight = Application.Current.MainWindow.RenderSize.Height;
        private SequenceView _sequence = new SequenceView();

        #endregion // Fields

        #region Properties

		public SequenceView Sequence {
			get { return _sequence; }
		}

		// Reference to the currently selected block model for animating
		public FrameworkElement SelectedBlock { get; set; }

        public double Width {
            get { 
				return _minWidth; 
			}
            set {
				if (value <= Sequence.RenderSize.Width) {
					_minWidth = Sequence.RenderSize.Width + 100;
				}
				else {
					_minWidth = value;
				}
			}
        }

        public double Height {
            get { 
				return _minHeight; 
			}
            set {
				Sequence.Measure(new Size(Sequence.MaxWidth, Sequence.MaxHeight));
				if(value/2 <= Sequence.RenderSize.Height) {
					_minHeight = Sequence.RenderSize.Height + 100;
				} else {
					_minHeight = value; 
				}
				NotifyPropertyChanged("SequenceY");
			}
        }

        public double SequenceY {
			get {
				int halfway = ((int)(Height / 2) / 25) *25;
				int halfSize = ((int) (Sequence.RenderSize.Height/2)/25)*25;
				return halfway-halfSize; 
			}
        }

        public bool IsUndoEnabled {
            get { return _undoManager.IsUndoEnabled; }
        }

        public bool IsRedoEnabled {
            get { return _undoManager.IsRedoEnabled; }
        }

        #endregion // Properties


        public void Undo() {
            _undoManager.Undo(out _workspace);
        }

        public void Redo() {
            _undoManager.Redo(out _workspace);
        }

        public WorkspaceViewModel() {
            _undoManager = ServiceLocator.GetService<IUndoService>().CreateUndoManager();
            _undoManager.UndoStackChanged += new EventHandler(OnUndoChanged);
            _undoManager.RedoStackChanged += new EventHandler(OnRedoChanged);

            OnUndoChanged(null, null);
            OnRedoChanged(null, null);

            Application.Current.MainWindow.SizeChanged += new SizeChangedEventHandler(MainWindow_SizeChanged);
            _sequence.SizeChanged += new SizeChangedEventHandler(Sequence_SizeChanged);
        }

        //public WorkspaceViewModel(Workspace workspace)
        //{
        //    _workspace = workspace;

        //}

        private void MainWindow_SizeChanged(object sender, EventArgs e) {
            //Application.Current.MainWindow.InvalidateVisual();
            Width = Application.Current.MainWindow.RenderSize.Width;
            Height = Application.Current.MainWindow.RenderSize.Height;
            NotifyPropertyChanged("Width");
            NotifyPropertyChanged("Height");
        }

        private void Sequence_SizeChanged(object sender, EventArgs e) {
            //Application.Current.MainWindow.InvalidateVisual();
            Width = Application.Current.MainWindow.RenderSize.Width;
            Height = Application.Current.MainWindow.RenderSize.Height;
            NotifyPropertyChanged("Width");
            NotifyPropertyChanged("Height");
        }

        private void OnUndoChanged(object sender, EventArgs e) {
            Debug.WriteLine("Firing PropertyChanged on IsUndoEnabled");
            OnPropertyChanged("IsUndoEnabled");
        }

        private void OnRedoChanged(object sender, EventArgs e) {
            Debug.WriteLine("Firing PropertyChanged on IsRedoEnabled");
            OnPropertyChanged("IsRedoEnabled");
        }

        RelayCommand _dropCommand;

        public ICommand DropCommand {
            get {
                if (_dropCommand == null) {
                    _dropCommand = new RelayCommand(param => this.OnDrop());
                }

                return _dropCommand;
            }
        }

        public bool IsUntitled {
            get {
                if (_workspace != null) {
                    return _workspace.IsUntitled;
                }

                return false;
            }
        }

        void OnDrop() {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="property">The property.</param>
        private void NotifyPropertyChanged(string property) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }


        private static Block GetConnectedBlocks(ObservableCollection<FrameworkElement> elements) {
            Block currentBlock = null;
            Block nextBlock = null;
            Block firstBlock = null;
            foreach (FrameworkElement blockView in elements) {
                if (blockView is MoveControlBlockView) {
                    MoveControlBlockViewModel moveViewModel = (MoveControlBlockViewModel)blockView.DataContext;
                    nextBlock = moveViewModel.ModelBlock;
                } else if (blockView is WaitControlBlockView) {
                    WaitControlBlockViewModel waitViewModel = (WaitControlBlockViewModel)blockView.DataContext;
                    nextBlock = waitViewModel.ModelBlock;
                } else if (blockView is LoopControlBlockView) {
                    LoopControlBlockViewModel loopViewModel = (LoopControlBlockViewModel)blockView.DataContext;
                    loopViewModel.ModelBlock.LoopPath = GetConnectedBlocks(loopViewModel.Children);
                    nextBlock = loopViewModel.ModelBlock;
                } else if (blockView is SwitchTabBlockView) {
                    SwitchTabBlockViewModel switchViewModel = (SwitchTabBlockViewModel)blockView.DataContext;
                    switchViewModel.ModelBlock.MapPath(true, GetConnectedBlocks(switchViewModel.Cases[0]));
                    switchViewModel.ModelBlock.MapPath(false, GetConnectedBlocks(switchViewModel.Cases[1]));
                    nextBlock = switchViewModel.ModelBlock;
                } else {
                    continue;
                }

                //the assignment takes place out here to prevent loads of duplicate checks..
                //only reason I have all the weird block vars
                if (currentBlock == null) {
                    firstBlock = nextBlock;
                    currentBlock = nextBlock;
                } else {
                    currentBlock.Next = nextBlock;
                    currentBlock = currentBlock.Next;
                }
                //set next to null for now, incase a block has been remove since it was last connected
                currentBlock.Next = null;

            }
            return firstBlock;
        }

        public StartBlock GetConnectedModel() {
            StartBlock start = DefaultModelFactory.Instance.CreateStartBlock();
            SequenceViewModel sequenceViewModel = (SequenceViewModel)_sequence.DataContext;
            start.Next = GetConnectedBlocks(sequenceViewModel.Blocks);
            return start;
        }
    }
}
