using System.Windows;

namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// A simple view locator that provides instance of views.
    /// </summary>
    public interface IViewLocator
    {
        void RegisterApplicationViews();
        FrameworkElement GetViewInstance(string viewKey);
    }
}