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
        Panel RootPanel { get; }
        
        /// <summary>
        /// Gets the the view that is currently displayed.
        /// </summary>
        ViewInfo CurrentView { get; }

        /// <summary>
        /// Gets all the opened views except message boxes.
        /// </summary>
        ReadOnlyObservableCollection<ViewInfo> OpenedViews { get; }
       
        /// <summary>
        /// Gets the number of currently opened views.
        /// </summary>
        int NbOpenedViews { get; }

        void CreateWorkspace(Panel rootPanel, AnimateTransitionAction animateTransitionAction);

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
        /// <param name="navigationInfo">The navigation info. It must specify the parent view key on which to push the view.</param>
        /// <remarks></remarks>
        ViewInfo ShowModal<TView>(NavigationInfo navigationInfo) where TView : FrameworkElement, new();

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
        /// <returns>The view info for the closed view.</returns>
        ViewInfo Close(string viewKey);

        /// <summary>
        /// Closes the application.
        /// </summary>
        /// <param name="forceClose">If true then force the application to exit whatever its current state.</param>
        bool CloseApplication(bool forceClose = false);
    }
}
