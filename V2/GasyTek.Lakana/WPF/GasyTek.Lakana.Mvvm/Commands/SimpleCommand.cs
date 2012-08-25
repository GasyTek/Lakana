using System;

namespace GasyTek.Lakana.Mvvm.Commands
{
    /// <summary>
    /// A very simple implementation of ICommand.
    /// </summary>
    /// <typeparam name="TParam">The type of the param.</typeparam>
    public class SimpleCommand<TParam> : ISimpleCommand<TParam>
    {
        private readonly Action<TParam> _executeAction;
        private readonly Predicate<TParam> _canExecuteAction;

        #region Constructor

        public SimpleCommand(Action<TParam> executeAction)
        {
            _executeAction = executeAction;
        }

        public SimpleCommand(Action<TParam> executeAction, Predicate<TParam> canExecuteAction)
        {
            if(executeAction == null)
                throw new ArgumentNullException("executeAction");

            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }

        #endregion

        // TODO : link to CommandManager and RequerySuggested

        #region ISimpleCommand implementation

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null) handler (this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteAction == null || _canExecuteAction((TParam)parameter);
        }

        public void Execute(object parameter)
        {
            if (_executeAction == null) return;
            _executeAction((TParam)parameter);
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class SimpleCommand : SimpleCommand<object>
    {
        public SimpleCommand(Action<object> executeAction)
            : base(executeAction)
        {
        }

        public SimpleCommand(Action<object> executeAction, Predicate<object> canExecuteAction)
            : base(executeAction, canExecuteAction)
        {
        }
    }
}
