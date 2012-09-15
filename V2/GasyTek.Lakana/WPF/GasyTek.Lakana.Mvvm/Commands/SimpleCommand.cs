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
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCommand&lt;TParam&gt;"/> class.
        /// </summary>
        /// <param name="executeAction">The execute action.</param>
        public SimpleCommand(Action<TParam> executeAction)
        {
            _executeAction = executeAction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCommand&lt;TParam&gt;"/> class.
        /// </summary>
        /// <param name="executeAction">The execute action.</param>
        /// <param name="canExecuteAction">The can execute action.</param>
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

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Raises the can execute changed.
        /// </summary>
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
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCommand"/> class.
        /// </summary>
        /// <param name="executeAction">The execute action.</param>
        public SimpleCommand(Action<object> executeAction)
            : base(executeAction)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCommand"/> class.
        /// </summary>
        /// <param name="executeAction">The execute action.</param>
        /// <param name="canExecuteAction">The can execute action.</param>
        public SimpleCommand(Action<object> executeAction, Predicate<object> canExecuteAction)
            : base(executeAction, canExecuteAction)
        {
        }
    }
}
