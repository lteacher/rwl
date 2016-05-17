using System.Windows.Input;
using System;

using RobotInitial.Command;

namespace RobotInitial.ViewModel
{
    // Based on the demo application from the MVVM tutorial
    // at http://msdn.microsoft.com/en-us/magazine/dd419663.aspx
    //
    public abstract class ClosableViewModel : ViewModelBase
    {
        #region Fields

        RelayCommand _closeCommand;

        #endregion // Fields

        #region Constructor

        protected ClosableViewModel()
        {
        }

        #endregion // Constructor

        #region CloseCommand

        public ICommand CloseCommand
        {
			
            get
            {
				
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(param => this.OnRequestClose());
                }

                return _closeCommand;
            }
        }

        #endregion // CloseCommand

        #region RequestClose [event]

        public event EventHandler RequestClose;

        void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion // RequestClose [event]
    }
}
