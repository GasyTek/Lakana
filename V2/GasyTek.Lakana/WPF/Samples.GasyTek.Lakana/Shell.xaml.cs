using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GasyTek.Lakana.Mvvm.Commands;
using GasyTek.Lakana.Navigation.Services;
using Samples.GasyTek.Lakana.Screens;
using Samples.GasyTek.Lakana.Utils;

namespace Samples.GasyTek.Lakana
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell
    {
        public ICommand HomeCommand { get; private set; }
        public ICommand ContactListCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }

        public Shell()
        {
            InitializeComponent();
            InitializeCommands();

            MouseDown += (sender, args) =>
                             {
                                 var innerBorder = args.OriginalSource as Border;
                                 if (innerBorder != null && innerBorder.Name == "PART_DragBorder")
                                 {
                                     DragMove();
                                 }
                             };
        }

        private void InitializeCommands()
        {
            HomeCommand = new SimpleCommand(param => NavigationManager.NavigateTo(ScreenId.Home));
            ContactListCommand = new SimpleCommand(param => NavigationManager.NavigateTo(ScreenId.ContactList, new ContactListViewModel()));
            AboutCommand = new SimpleCommand(param => NavigationManager.NavigateTo(ScreenId.About));
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // Navigate to "Home" screen first
            NavigationManager.NavigateTo(ScreenId.Home);
        }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            NavigationManager.CloseApplication();
        }
    }
}
