using GasyTek.Lakana.Navigation.Services;

namespace Samples.GasyTek.Lakana.Navigation.Common
{
    /// <summary>
    /// Offers a singleton objects.
    /// </summary>
    public class Singletons
    {
        private static INavigationService _navigationService;

        public static INavigationService NavigationService
        {
            get { return _navigationService ?? (_navigationService = new NavigationService()); }
        }
    }
}
