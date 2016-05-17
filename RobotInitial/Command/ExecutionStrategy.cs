using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Command
{
    interface IExecutionStrategy
    {
        CommandBehaviorBinding Behavior { get; set; }

        void Execute(object parameter);
    }

    class CommandExecutionStrategy : IExecutionStrategy
    {

        public CommandBehaviorBinding Behavior { get; set; }

        public void Execute(object parameter)
        {
            if (Behavior == null)
            {
                throw new InvalidOperationException("Behavior is null, which is invalid");
            }

            if (Behavior.Command.CanExecute(parameter))
            {
                Behavior.Command.Execute(parameter);
            }

        }
    }

    class ActionExecutionStrategy : IExecutionStrategy
    {
        public CommandBehaviorBinding Behavior { get; set; }

        public void Execute(object parameter)
        {
            if (Behavior == null)
            {
                throw new InvalidOperationException("Behavior is null, which is invalid");
            }

            Behavior.Action(parameter);
        }
    }
}
