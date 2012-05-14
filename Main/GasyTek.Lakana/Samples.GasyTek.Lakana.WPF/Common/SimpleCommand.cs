using System;
using System.Windows.Input;

namespace Samples.GasyTek.Lakana.WPF.Common
{
    public interface ISimpleCommand<TParam> : ICommand
    {
    }

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
            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }

        #endregion

        #region ISimpleCommand implementation

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null) handler(this, new EventArgs());
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
}
