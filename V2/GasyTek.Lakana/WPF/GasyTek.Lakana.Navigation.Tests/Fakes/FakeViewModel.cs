using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Tests.Fakes
{
    public class FakeViewModel : ICloseable, IPresentable, IViewKeyAware
    {
        public IUIMetadata UIMetadata { get; private set; }

        public string ViewInstanceKey { get; set; }

        public FakeViewModel()
        {
            UIMetadata = new UIMetadata { LabelProvider = () => "Label" };
        }

        public bool CanClose()
        {
            return false;
        }
    }
}