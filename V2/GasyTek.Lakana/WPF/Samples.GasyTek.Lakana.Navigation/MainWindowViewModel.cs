using System.Collections.ObjectModel;
using System.Windows.Input;
using GasyTek.Lakana.Navigation.Services;
using Samples.GasyTek.Lakana.Navigation.Common;
using Samples.GasyTek.Lakana.Navigation.Features;

namespace Samples.GasyTek.Lakana.Navigation
{
    public class MainWindowViewModel
    {
        public ICommand OpenHomeCommand { get; private set; }
        public ICommand OpenProductListCommand { get; private set; }
        public ICommand OpenAboutCommand { get; private set; }
        public ICommand ExitApplicationCommand { get; private set; }

        public ReadOnlyObservableCollection<View> Views
        {
            get { return NavigationManager.Views; }
        }

        public MainWindowViewModel()
        {
            OpenHomeCommand = new SimpleCommand<object>(OnOpenHomeCommandExecute);
            OpenProductListCommand = new SimpleCommand<object>(OnOpenProductListCommandExecute);
            OpenAboutCommand = new SimpleCommand<object>(OnOpenAboutCommandExecute);
            ExitApplicationCommand = new SimpleCommand<object>(OnExitApplicationCommandExecute);
        }

        private void OnOpenHomeCommandExecute(object param)
        {
            // Navigate to an existing view. 
            // This overload of NavigateTo works only for existing view
            NavigationManager.NavigateTo(ViewId.Home);
        }

        private void OnOpenProductListCommandExecute(object param)
        {
            // navigates to product list view. 
            // The view will be created during the first call if it doesn't exist yet
            // Note that we pass the view model to the NavigateTo method 
            NavigationManager.NavigateTo(ViewId.ProductList, new ProductListViewModel());
        }

        private void OnOpenAboutCommandExecute(object obj)
        {
            NavigationManager.NavigateTo(ViewId.About);
        }

        private void OnExitApplicationCommandExecute(object obj)
        {
            NavigationManager.CloseApplication();
        }
    }
}
