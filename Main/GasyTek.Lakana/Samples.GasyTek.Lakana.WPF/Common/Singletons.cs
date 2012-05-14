using GasyTek.Lakana.WPF.Services;

namespace Samples.GasyTek.Lakana.WPF.Common
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
