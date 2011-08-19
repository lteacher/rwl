using System;
using System.Windows.Input;

namespace RobotInitial.ViewModel
{
    // Based on the demo application from the MVVM tutorial
    // at http://msdn.microsoft.com/en-us/magazine/dd419663.aspx
    //
    public class CommandViewModel : ViewModelBase
    {
        public CommandViewModel(string displayName, ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            base.DisplayName = displayName;
            this.Command = command;
        }

        public ICommand Command { get; private set; }

    }
}
