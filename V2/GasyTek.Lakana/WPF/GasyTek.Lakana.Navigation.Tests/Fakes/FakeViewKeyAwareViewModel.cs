using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Tests.Fakes
{
    public class FakeViewKeyAwareViewModel : IViewKeyAware
    {
        public string ViewKey { get; set; }
    }
}
