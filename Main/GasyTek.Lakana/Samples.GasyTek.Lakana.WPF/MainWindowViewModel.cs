using System.Collections.ObjectModel;
using System.Windows.Input;
using GasyTek.Lakana.WPF.Services;
using Samples.GasyTek.Lakana.WPF.Common;
using Samples.GasyTek.Lakana.WPF.Features;

namespace Samples.GasyTek.Lakana.WPF
{
    public class MainWindowViewModel
    {
        public ICommand OpenHomeCommand { get; private set; }
        public ICommand OpenProductListCommand { get; private set; }
        public ICommand OpenAboutCommand { get; private set; }
        public ICommand ExitApplicationCommand { get; private set; }

        public ReadOnlyObservableCollection<ViewInfo> OpenedViews
        {
            get { return Singletons.NavigationService.OpenedViews; }
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
            Singletons.NavigationService.NavigateTo(ViewId.Home);
        }

        private void OnOpenProductListCommandExecute(object param)
        {
            // navigates to product list view. 
            // The view will be created during the first call if it doesn't exist yet
            // Note that we pass the view model to the NavigateTo method 
            var navigationInfo = NavigationInfo.CreateSimple(ViewId.ProductList, new ProductListViewModel());
            Singletons.NavigationService.NavigateTo<ProductListView>(navigationInfo);
        }

        private void OnOpenAboutCommandExecute(object obj)
        {
            var navigationInfo = NavigationInfo.CreateSimple(ViewId.About);
            Singletons.NavigationService.NavigateTo<AboutView>(navigationInfo);
        }

        private void OnExitApplicationCommandExecute(object obj)
        {
            Singletons.NavigationService.CloseApplication();
        }
    }
}
