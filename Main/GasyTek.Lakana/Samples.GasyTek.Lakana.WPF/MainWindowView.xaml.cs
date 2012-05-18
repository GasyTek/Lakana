using System.Windows;
using GasyTek.Lakana.WPF.Services;
using GasyTek.Lakana.WPF.Transitions;
using Samples.GasyTek.Lakana.WPF.Common;
using Samples.GasyTek.Lakana.WPF.Features;

namespace Samples.GasyTek.Lakana.WPF
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // initializes navigation service
            Singletons.NavigationService.CreateWorkspace(ContentView, Transitions.FadeTransition);

            // set HomeView as the first view
            var homeNavigationInfo = NavigationInfo.CreateSimple(ViewId.Home);
            Singletons.NavigationService.NavigateTo<HomeView>(homeNavigationInfo);

            // activate/deactivate menu whether the closing application view is visible or not
            Singletons.NavigationService.ClosingApplicationShown += (sender1, e1) => mainMenu.IsEnabled = false;
            Singletons.NavigationService.ClosingApplicationHidden += (sender1, e1) => mainMenu.IsEnabled = true;

        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // allow to close the application if any task is currently running
            e.Cancel = !Singletons.NavigationService.CloseApplication();
        }

        private void OpenViewClick(object sender, RoutedEventArgs e)
        {
            var frameworkElement = e.OriginalSource as FrameworkElement;
            if (frameworkElement == null) return;

            var viewInfo = (ViewInfo) frameworkElement.DataContext;
            if(viewInfo != ViewInfo.Null)
            {
                // HOW TO : navigate to a previously opened view
                Singletons.NavigationService.NavigateTo(viewInfo.ViewKey);
            }
        }
    }
}
