using System.Windows;

namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// A view locator that registers and instantiate views.
    /// </summary>
    public interface IViewLocator
    {
        /// <summary>
        /// Registers all instantiable views for the application.
        /// </summary>
        void RegisterApplicationViews();
        /// <summary>
        /// Gets the view instance according to the specified view key.
        /// </summary>
        /// <param name="viewKey">The view key.</param>
        /// <returns></returns>
        FrameworkElement GetViewInstance(string viewKey);
    }
}