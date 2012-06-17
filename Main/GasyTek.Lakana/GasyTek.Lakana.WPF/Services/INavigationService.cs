using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GasyTek.Lakana.WPF.Services
{
    /// <summary>
    /// Represents a navigation system.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Occurs when the closing application view was shown.
        /// </summary>
        event EventHandler ShutdownApplicationShown;
        
        /// <summary>
        /// Occurs when the closing application view was hidden.
        /// </summary>
        event EventHandler ShutdownApplicationHidden;

        /// <summary>
        /// Workspace root panel which contains all views.
        /// </summary>
        Panel RootPanel { get; }
        
        /// <summary>
        /// Gets the the view that is currently displayed.
        /// </summary>
        ViewInfo CurrentView { get; }

        /// <summary>
        /// Gets all opened views (event those that are not actually visible) except message boxes.
        /// </summary>
        ReadOnlyObservableCollection<ViewInfo> OpenedViews { get; }
       
        /// <summary>
        /// Gets the number of currently opened views.
        /// </summary>
        int NbOpenedViews { get; }

        /// <summary>
        /// Initializes the navigation service and associates the root workspace to it.
        /// </summary>
        /// <param name="rootPanel">The root view that will constitute the shell of the application.</param>
        void Initialize(Panel rootPanel);

        /// <summary>
        /// Assigns the transition animation.
        /// </summary>
        /// <param name="animateTransitionAction"></param>
        void ChangeTransitionAnimation(AnimateTransitionAction animateTransitionAction);

        /// <summary>
        /// Navigates or creates the specified view.
        /// </summary>
        /// <typeparam name="TView">The type of the view.</typeparam>
        /// <param name="navigationInfo">The navigation info.</param>
        /// <returns></returns>
        ViewInfo NavigateTo<TView>(NavigationInfo navigationInfo) where TView : FrameworkElement, new();

        /// <summary>
        /// Navigates to the existing view specified by the key.
        /// </summary>
        /// <param name="viewKey">The view key.</param>
        /// <returns></returns>
        ViewInfo NavigateTo(string viewKey);

        /// <summary>
        /// Shows the view as modal.
        /// </summary>
        /// <typeparam name="TView">The type of the view.</typeparam>
        /// <typeparam name="TResult">The type of the expected result from the modal window </typeparam>
        /// <param name="navigationInfo">The navigation info. It must specify the parent view key on which to push the view.</param>
        /// <remarks></remarks>
        ModalResult<TResult> ShowModal<TView, TResult>(NavigationInfo navigationInfo) where TView : FrameworkElement, new();

        /// <summary>
        /// Shows a message box that is modal to the specified parent.
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
        /// <param name="modalResult">The result that the view must return if it was a modal view. You should set this value if you want your view to return a modal.</param>
        /// <returns>The view info for the closed view.</returns>
        ViewInfo Close(string viewKey, object modalResult = null);

        /// <summary>
        /// Closes the application.
        /// </summary>
        /// <param name="forceClose">If true then force the application to exit whatever its current state.</param>
        bool CloseApplication(bool forceClose = false);
    }
}
