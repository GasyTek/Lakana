using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Services;
using GasyTek.Lakana.Navigation.Transitions;
using Samples.GasyTek.Lakana.Navigation.Common;
using Samples.GasyTek.Lakana.Navigation.Features;

namespace Samples.GasyTek.Lakana.Navigation
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView
    {
        public MainWindowView()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // initializes navigation service
            Singletons.NavigationService.Initialize(ContentView);

            // Change the transition animation to Fade transition
            Singletons.NavigationService.ChangeTransitionAnimation(Transition.FadeTransition);

            // set HomeView as the first view
            var homeNavigationInfo = NavigationInfo.CreateSimple(ViewId.Home);
            Singletons.NavigationService.NavigateTo<HomeView>(homeNavigationInfo);
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // allow to close the application if any task is currently running
            if (Singletons.NavigationService.CloseApplication() == false)
            {
                e.Cancel = true;
            }
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

        private void MenuItemClick(object sender, RoutedEventArgs e)
        {
            switch (((MenuItem)e.Source).Name)
            {
                case "menuNoAnimation":
                    menuNoAnimation.IsChecked = true;
                    menuFade.IsChecked = false;
                    Singletons.NavigationService.ChangeTransitionAnimation(Transition.NoTransition);
                    break;
                case "menuFade":
                    menuNoAnimation.IsChecked = false;
                    menuFade.IsChecked = true;
                    Singletons.NavigationService.ChangeTransitionAnimation(Transition.FadeTransition);
                    break;
            }
        }
    }
}
