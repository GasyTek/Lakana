using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Controls
{
    /// <summary>
    /// Custom control that serve to display a list of all tasks that necessitates a user action to be terminated.
    /// </summary>
    public class ShutdownApplicationItemsControl : ItemsControl
    {
        #region Properties

        internal INavigationService NavigationService { get; set; }

        #endregion

        #region Constructor

        static ShutdownApplicationItemsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ShutdownApplicationItemsControl), new FrameworkPropertyMetadata(typeof(ShutdownApplicationItemsControl)));
        }

        #endregion

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ShutdownApplicationItem { NavigationService = NavigationService };
        }
    }
}
