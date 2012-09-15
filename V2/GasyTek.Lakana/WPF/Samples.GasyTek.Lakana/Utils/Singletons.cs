using GasyTek.Lakana.Navigation.Services;

namespace Samples.GasyTek.Lakana.Utils
{
    public class Singletons
    {
        // You can replace singleton implementation with aan IoC container or a Service Locator implementation

        private static INavigationService _navigationServiceInstance = null;
        public static INavigationService NavigationServiceInstance
        {
            get { return _navigationServiceInstance ?? (_navigationServiceInstance = new NavigationService()); }
        }
    }
}