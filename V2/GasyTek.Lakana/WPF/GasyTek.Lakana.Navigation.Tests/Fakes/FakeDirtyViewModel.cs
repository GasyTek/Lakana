using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Tests.Fakes
{
    public class FakeDirtyViewModel : ICloseable
    {
        public bool CanClose()
        {
            return false;
        }
    }
}
