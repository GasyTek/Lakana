using System.Threading.Tasks;

namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// An object that represents the result of a navigation process.
    /// </summary>
    public class NavigationResult
    {
        /// <summary>
        /// Gets a task that represents the async transition operation that may be in progress. 
        /// </summary>
        public Task AsyncTransition { get; private set; }

        /// <summary>
        /// Gets the view info that corresponds to the modal view.
        /// </summary>
        public View View { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationResult" /> class.
        /// </summary>
        /// <param name="asyncTransition"></param>
        /// <param name="view"></param>
        public NavigationResult(Task asyncTransition, View view)
        {
            AsyncTransition = asyncTransition;
            View = view;
        }
    }
}