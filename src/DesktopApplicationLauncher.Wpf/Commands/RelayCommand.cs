namespace DesktopApplicationLauncher.Wpf.Commands
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;

    public sealed class RelayCommand : ICommand
    {
        private readonly Action<object> _executeAction;

        private readonly Func<object, bool> _canExecuteFunc;

        public RelayCommand(Action<object> executeAction, Func<object, bool> canExecuteFunc = null)
        {
            _executeAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            _canExecuteFunc = canExecuteFunc;
        }

        [SuppressMessage("Roslynator", "RCS1146:Use conditional access.", Justification = "<Pending>")]
        public bool CanExecute(object parameter)
        {
            return _canExecuteFunc == null || _canExecuteFunc.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            _executeAction(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}