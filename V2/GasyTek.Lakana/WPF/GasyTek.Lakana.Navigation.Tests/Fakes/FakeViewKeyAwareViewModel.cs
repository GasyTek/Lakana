using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Tests.Fakes
{
    public class FakeViewKeyAwareViewModel : IViewKeyAware
    {
        public string ViewInstanceKey { get; set; }
    }
}
