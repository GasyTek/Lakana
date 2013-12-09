using System.Threading.Tasks;
using System.Windows;

namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// Represents a navigation manager system.
    /// </summary>
    internal interface INavigationManager
    {
        /// <summary>
        /// Navigates to the specified navigation key.
        /// </summary>
        /// <param name="navigationKey">The navigation key. It must be of the form : [ parentViewKey [ # instanceID ] / ] viewKey [ # instanceID ] where fields between [..] are optionals.</param>
        /// <returns></returns>
        NavigationResult NavigateTo(string navigationKey);

        /// <summary>
        /// Navigates to the specified navigation key.
        /// </summary>
        /// <param name="navigationKey">The navigation key. It must be of the form : [ parentViewKey [ # instanceID ] / ] viewKey [ # instanceID ] where fields between [..] are optionals.</param>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        NavigationResult NavigateTo(string navigationKey, object viewModel);

        /// <summary>
        /// Display the specified view as modal.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="navigationKey">The navigation key. It must be of the form : [ parentViewKey [ # instanceID ] / ] viewKey [ # instanceID ] where fields between [..] are optionals.</param>
        /// <returns></returns>
        ModalResult<TResult> ShowModal<TResult>(string navigationKey);

        /// <summary>
        /// Display the specified view as modal.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="navigationKey">The navigation key. It must be of the form : [ parentViewKey [ # instanceID ] / ] viewKey [ # instanceID ] where fields between [..] are optionals.</param>
        /// <param name="viewModel">The view model to be associated to the opened view.</param>
        /// <returns></returns>
        ModalResult<TResult> ShowModal<TResult>(string navigationKey, object viewModel);

        /// <summary>
        /// Display a message box.
        /// </summary>
        /// <param name="ownerViewKey">The parent instance key : parentViewKey [ # instanceID ]</param>
        /// <param name="message">The message.</param>
        /// <param name="messageBoxImage">The message box image.</param>
        /// <param name="messageBoxButton">The message box button.</param>
        /// <returns></returns>
        Task<MessageBoxResult> ShowMessageBox(string ownerViewKey, string message = "",
                                              MessageBoxImage messageBoxImage = MessageBoxImage.Information,
                                              MessageBoxButton messageBoxButton = MessageBoxButton.OK);

        /// <summary>
        /// Closes the specified view.
        /// </summary>
        /// <param name="viewKey">The key associated to the view to be closed.</param>
        /// <param name="modalResult">The modal result.</param>
        /// <returns></returns>
        NavigationResult Close(string viewKey, object modalResult = null);

        /// <summary>
        /// Closes the application.
        /// </summary>
        /// <param name="forceClose">if set to <c>true</c> force the application to close.</param>
        /// <returns></returns>
        bool CloseApplication(bool forceClose = false);
    }
}