using System.Threading.Tasks;

namespace GasyTek.Lakana.WPF.Services
{
    /// <summary>
    /// An object that represents the result returned by a modal view that was shown.
    /// </summary>
    public class ModalResult<TResult>
    {
        private readonly TaskCompletionSource<TResult> _taskCompletionSource;

        /// <summary>
        /// Gets a task that will return the actual result of the modal operation. You can await this task for this purpose.
        /// </summary>
        public Task<TResult> Result { get { return _taskCompletionSource.Task; } }

        /// <summary>
        /// Gets the view info that corresponds to the modal view.
        /// </summary>
        public ViewInfo ViewInfo { get; internal set; }

        internal ModalResult(Task<object> firstTask)
        {
            _taskCompletionSource = new TaskCompletionSource<TResult>();
            firstTask.ContinueWith(tr => _taskCompletionSource.SetResult((TResult)tr.Result));
        }
    }
}