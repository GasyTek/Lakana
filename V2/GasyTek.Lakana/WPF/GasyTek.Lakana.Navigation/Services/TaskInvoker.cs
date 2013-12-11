using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// 
    /// </summary>
    internal class TaskInvoker
    {
        private Task _executingTask;
        private readonly Dispatcher _uiDispatcher;

        /// <summary>
        /// Initializes a TaskInvoker instance.
        /// </summary>
        public TaskInvoker(Dispatcher uiDispatcher)
        {
            var tcs = new TaskCompletionSource<bool>();
            tcs.SetResult(true);

            _executingTask = tcs.Task;
            _uiDispatcher = uiDispatcher;
        }

        /// <summary>
        /// Enqueue a task
        /// </summary>
        /// <param name="task"></param>
        /// <param name="args"></param>
        public void Enqueue(Delegate task, params object[] args)
        {
            // Force TaskScheduler.Default so that the continuation will never be scheduled on ui thread
            _executingTask = _executingTask.ContinueWith(t => ((Task)_uiDispatcher.Invoke(new Func<Task>(() => (Task)task.DynamicInvoke(args)), DispatcherPriority.Background)).Wait(), TaskScheduler.Default);
        }
    }
}
