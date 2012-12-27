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
        /// Navigates to.
        /// </summary>
        /// <param name="navigationKey">The navigation key.</param>
        /// <returns></returns>
        ViewInfo NavigateTo(string navigationKey);

        /// <summary>
        /// Navigates to.
        /// </summary>
        /// <param name="navigationKey">The navigation key.</param>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        ViewInfo NavigateTo(string navigationKey, object viewModel);

        /// <summary>
        /// Shows the modal.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="navigationKey">The navigation key.</param>
        /// <returns></returns>
        ModalResult<TResult> ShowModal<TResult>(string navigationKey);

        /// <summary>
        /// Shows the modal.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="navigationKey">The navigation key.</param>
        /// <param name="viewModel">The view model to be associated to the opened view.</param>
        /// <returns></returns>
        ModalResult<TResult> ShowModal<TResult>(string navigationKey, object viewModel);

        /// <summary>
        /// Shows the message box.
        /// </summary>
        /// <param name="ownerViewKey">The parent instance key.</param>
        /// <param name="message">The message.</param>
        /// <param name="messageBoxImage">The message box image.</param>
        /// <param name="messageBoxButton">The message box button.</param>
        /// <returns></returns>
        Task<MessageBoxResult> ShowMessageBox(string ownerViewKey, string message = "",
                                              MessageBoxImage messageBoxImage = MessageBoxImage.Information,
                                              MessageBoxButton messageBoxButton = MessageBoxButton.OK);

        /// <summary>
        /// Closes the specified view key.
        /// </summary>
        /// <param name="viewKey">The view key.</param>
        /// <param name="modalResult">The modal result.</param>
        /// <returns></returns>
        ViewInfo Close(string viewKey, object modalResult = null);

        /// <summary>
        /// Closes the application.
        /// </summary>
        /// <param name="forceClose">if set to <c>true</c> [force close].</param>
        /// <returns></returns>
        bool CloseApplication(bool forceClose = false);
    }
}