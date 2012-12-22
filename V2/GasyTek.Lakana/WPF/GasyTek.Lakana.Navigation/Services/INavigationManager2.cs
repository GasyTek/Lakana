using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// Represents a navigation system.
    /// </summary>
    public interface INavigationManager2
    {
        /// <summary>
        /// Workspace root panel which contains all views.
        /// </summary>
        Panel MainWorkspace { get; }
        
        /// <summary>
        /// Gets the the view that is currently displayed.
        /// </summary>
        ViewInfo CurrentView { get; }

        /// <summary>
        /// Gets all opened views with those that are not currently visible except message boxes.
        /// </summary>
        ReadOnlyObservableCollection<ViewInfo> OpenedViews { get; }
       
        /// <summary>
        /// Gets the number of currently opened views.
        /// </summary>
        int NbOpenedViews { get; }

        /// <summary>
        /// Assigns the transition animation provider.
        /// </summary>
        /// <param name="transitionAnimationProvider"></param>
        void ChangeTransitionAnimation(TransitionAnimationProvider transitionAnimationProvider);

        /// <summary>
        /// Navigates or creates the specified view.
        /// </summary>
        /// <param name="navigationInfo">The navigation info.</param>
        /// <returns></returns>
        ViewInfo NavigateTo(NavigationInfo navigationInfo);

        /// <summary>
        /// Navigates to and already opened view instance specified by the given key.
        /// </summary>
        /// <param name="viewKey">The view key.</param>
        /// <returns></returns>
        ViewInfo NavigateTo(string viewKey);

        /// <summary>
        /// Shows a view as modal.
        /// </summary>
        /// <typeparam name="TResult">The type of the expected result from the modal window </typeparam>
        /// <param name="modalNavigationInfo">The navigation info.</param>
        /// <remarks></remarks>
        ModalResult<TResult> ShowModal<TResult>(ModalNavigationInfo modalNavigationInfo);

        /// <summary>
        /// Shows a message box that is modal only to the specified parent.
        /// </summary>
        /// <param name="parentViewKey">The parent view key.</param>
        /// <param name="message">The message.</param>
        /// <param name="messageBoxImage">The message box image.</param>
        /// <param name="messageBoxButton">The message box button.</param>
        /// <returns>A task that will return a <see cref="MessageBoxResult"/></returns>
        Task<MessageBoxResult> ShowMessageBox(string parentViewKey, string message = "", MessageBoxImage messageBoxImage = MessageBoxImage.Information, MessageBoxButton messageBoxButton = MessageBoxButton.OK);

        /// <summary>
        /// Closes the specified view.
        /// </summary>
        /// <param name="viewKey">The view key.</param>
        /// <param name="modalResult">The result that the view returns if it was a modal view that needs to retrieve a value.</param>
        /// <returns>The view info for the closed view.</returns>
        ViewInfo Close(string viewKey, object modalResult = null);

        /// <summary>
        /// Closes the application.
        /// </summary>
        /// <param name="forceClose">If true then force the application to exit whatever its current state.</param>
        bool CloseApplication(bool forceClose = false);
    }
}
