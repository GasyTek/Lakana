using GasyTek.Lakana.WPF.Services;

namespace GasyTek.Lakana.WPF.Tests.Fakes
{
    public class FakeViewKeyAwareViewModel : IViewKeyAware
    {
        public string ViewKey { get; set; }
    }
}
