using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Navigation.Attributes;
using GasyTek.Lakana.Navigation.Services;
using Samples.GasyTek.Lakana.Utils;

namespace Samples.GasyTek.Lakana.Screens
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    [ViewKey(ScreenId.About)]
    public partial class AboutView : IPresentable
    {
        private readonly IUIMetadata _uiMetadata;

        public AboutView()
        {
            InitializeComponent();

            _uiMetadata = new UIMetadata { LabelProvider = () => "About" };
        }

        public IUIMetadata UIMetadata
        {
            get { return _uiMetadata; }
        }
    }
}
