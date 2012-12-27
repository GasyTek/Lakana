using System.Windows;

namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// A view locator that registers and instantiate views.
    /// </summary>
    public interface IViewLocator
    {
        void RegisterApplicationViews();
        FrameworkElement GetViewInstance(string viewKey);
    }
}