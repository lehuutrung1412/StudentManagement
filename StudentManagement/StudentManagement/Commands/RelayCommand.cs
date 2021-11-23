using System;
using System.Diagnostics;
using System.Windows.Input;

namespace StudentManagement.Commands
{
    public class RelayCommand<T> : BaseCommand
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        public RelayCommand(Predicate<T> canExecute, Action<T> execute)
        {
            _canExecute = canExecute;
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        [DebuggerStepThrough]
        public override bool CanExecute(object parameter)
        {
            try
            {
                return _canExecute == null || _canExecute((T)parameter);
            }
            catch
            {
                return true;
            }
        }

        public override void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public new event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}