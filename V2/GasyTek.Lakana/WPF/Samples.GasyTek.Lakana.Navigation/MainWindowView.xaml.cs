using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Services;
using GasyTek.Lakana.Navigation.Transitions;
using Samples.GasyTek.Lakana.Navigation.Common;

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
            // set animation transition
            NavigationManager.ChangeTransitionAnimation(FadeTransition.Create());

            // set HomeView as the first view
            NavigationManager.NavigateTo(ViewId.Home);
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // allow to close the application if any task is currently running
            if (NavigationManager.CloseApplication() == false)
            {
                e.Cancel = true;
            }
        }

        private void OpenViewClick(object sender, RoutedEventArgs e)
        {
            var frameworkElement = e.OriginalSource as FrameworkElement;
            if (frameworkElement == null) return;

            var viewInfo = (View)frameworkElement.DataContext;
            if (viewInfo != View.Null)
            {
                // HOW TO : navigate to a previously opened view
                NavigationManager.NavigateTo(viewInfo.ViewInstanceKey);
            }
        }

        private void MenuItemClick(object sender, RoutedEventArgs e)
        {
            switch (((MenuItem)e.Source).Name)
            {
                case "MenuNoAnimation":
                    MenuNoAnimation.IsChecked = true;
                    MenuFadeAnimation.IsChecked = false;
                    NavigationManager.ChangeTransitionAnimation(NoAnimation.Create());
                    break;
                case "MenuFadeAnimation":
                    MenuNoAnimation.IsChecked = false;
                    MenuFadeAnimation.IsChecked = true;
                    NavigationManager.ChangeTransitionAnimation(FadeTransition.Create());
                    break;
            }
        }
    }
}
