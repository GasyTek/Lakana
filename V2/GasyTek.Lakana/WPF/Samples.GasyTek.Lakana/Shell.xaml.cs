using System.Windows;
using System.Windows.Input;
using GasyTek.Lakana.Mvvm.Commands;
using GasyTek.Lakana.Navigation.Services;
using GasyTek.Lakana.Navigation.Transitions;
using Samples.GasyTek.Lakana.Screens;
using Samples.GasyTek.Lakana.Utils;

namespace Samples.GasyTek.Lakana
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell
    {
        public ICommand HomeCommand { get; private set;}
        public ICommand ContactListCommand { get; private set;}
        public ICommand AboutCommand { get; private set;}

        public Shell()
        {
            InitializeComponent();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            HomeCommand = new SimpleCommand(param =>
            {
                var navigationInfo = NavigationInfo.CreateSimple(ScreenId.Home);
                Singletons.NavigationServiceInstance.NavigateTo<HomeView>(navigationInfo);
            });

            ContactListCommand = new SimpleCommand(param =>
            {
                var navigationInfo = NavigationInfo.CreateSimple(ScreenId.ContactList, new ContactListViewModel());
                Singletons.NavigationServiceInstance.NavigateTo<ContactListView>(navigationInfo);
            });

            AboutCommand = new SimpleCommand(param =>
            {
                var navigationInfo = NavigationInfo.CreateSimple(ScreenId.About);
                Singletons.NavigationServiceInstance.NavigateTo<AboutView>(navigationInfo);
            });
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // Initializes the navigation service
            Singletons.NavigationServiceInstance.Initialize(Workspace);
            Singletons.NavigationServiceInstance.ChangeTransitionAnimation(Transition.FadeTransition);

            // Navigate to "Home" screen first
            var navigationInfo = NavigationInfo.CreateSimple(ScreenId.Home);
            Singletons.NavigationServiceInstance.NavigateTo<HomeView>(navigationInfo);
        }

        
    }
}
