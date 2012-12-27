using System.Windows.Controls;
using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Tests.Fakes
{
    public class FakeView : UserControl, IPresentable, IViewKeyAware
    {
        public IUIMetadata UIMetadata { get; private set; }

        public FakeView()
        {
            UIMetadata = new UIMetadata { LabelProvider = () => "ViewLabel" };
        }

        public string ViewInstanceKey { get; set; }
    }
}
