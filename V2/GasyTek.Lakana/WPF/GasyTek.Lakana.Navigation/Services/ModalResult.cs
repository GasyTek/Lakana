using System.Threading.Tasks;

namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// An object that represents the result returned by a modal view that was displayed.
    /// </summary>
    public class ModalResult<TResult>
    {
        private readonly TaskCompletionSource<TResult> _taskCompletionSource;

        /// <summary>
        /// Gets a task that will return the actual result of the modal operation. 
        /// </summary>
        /// <remarks>You can await this task for this purpose.</remarks>
        public Task<TResult> AsyncResult { get { return _taskCompletionSource.Task; } }

        /// <summary>
        /// Gets the view info that corresponds to the modal view.
        /// </summary>
        public View View { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModalResult{TResult}" /> class.
        /// </summary>
        /// <param name="firstTask">The first task.</param>
        /// <param name="view">The modal view.</param>
        internal ModalResult(Task<object> firstTask, View view)
        {
            _taskCompletionSource = new TaskCompletionSource<TResult>();
            firstTask.ContinueWith(tr => _taskCompletionSource.SetResult((TResult)tr.Result));
            View = view;
        }
    }
}