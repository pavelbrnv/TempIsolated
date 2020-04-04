using System;
using System.Windows.Input;

namespace TempIsolated.Common.Extensions
{
    public sealed class ActionCommand : ICommand
    {
        private readonly Func<bool> canExecute;
        private readonly Action execute;

        public string PublicName { get; }

        public ActionCommand(Func<bool> canExecute, Action execute, string publicName)
        {
            Contracts.Requires(canExecute != null);
            Contracts.Requires(execute != null);

            this.canExecute = canExecute;
            this.execute = execute;
            
            PublicName = publicName;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute();
        }

        public void Execute(object parameter)
        {
            execute();
        }
    }
}
