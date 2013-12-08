using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GasyTek.Lakana.Navigation.Tests.Helper
{
    /// <summary>
    /// Helper class for unit testing code that manipulates WPF controls.
    /// </summary>
    internal class UITestHelper
    {
        private Dispatcher _dispatcher;

        // Do not declare this variable into ExecuteOnUIThread method
        // because the closure is lost and the variable is never correctly
        // assigned to be used after the dispatcher has shutdown.
        private Exception _unhandledException;

        /// <summary>
        /// Helper method that executes a WPF code inside a fake UI Thread.
        /// </summary>
        /// <param name="action"></param>
        public void ExecuteOnUIThread(Action action)
        {
            if (_dispatcher == null)
            {
                _dispatcher = CreateDispatcher();
                _dispatcher.UnhandledException += (sender, args) =>
                {
                    _unhandledException = args.Exception;
                    args.Dispatcher.InvokeShutdown();
                    args.Handled = true;
                };
            }

            // enqueue the action to the UI thread
            _dispatcher.Invoke(action);

            // if the task was finished because of an exception
            // rethrow this exception on the caller thread
            if (_unhandledException != null)
            {
                var callerDispatcher = Dispatcher.CurrentDispatcher;
                var ex = _unhandledException;
                _unhandledException = null;
                callerDispatcher.Invoke(new Action(delegate { throw ex; }));
            }
        }

        public void StopUIThread()
        {
            _dispatcher.InvokeShutdown();
            _dispatcher = null;
        }

        private Dispatcher CreateDispatcher()
        {
            var tcs = new TaskCompletionSource<Dispatcher>();

            var mockUIThread = new Thread(() =>
            {
                // Create the context, and install it:
                var dispatcher = Dispatcher.CurrentDispatcher;
                var syncContext = new DispatcherSynchronizationContext(dispatcher);

                SynchronizationContext.SetSynchronizationContext(syncContext);

                tcs.SetResult(dispatcher);

                // Start the Dispatcher and pump messages
                Dispatcher.Run();
            });

            mockUIThread.Name = "Fake UI Thread";
            mockUIThread.IsBackground = true;
            mockUIThread.SetApartmentState(ApartmentState.STA);
            mockUIThread.Start();

            return tcs.Task.Result;
        }
    }
}
