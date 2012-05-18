using GasyTek.Lakana.WPF.Common;
using GasyTek.Lakana.WPF.Services;

namespace Samples.GasyTek.Lakana.WPF.Features
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : IPresentable
    {
        private readonly IUIMetadata _uiMetadata;

        public HomeView()
        {
            InitializeComponent();

            _uiMetadata = new UIMetadata {LabelProvider = () => "Home"};
        }

        public IUIMetadata UIMetadata
        {
            get { return _uiMetadata; }
        }
    }
}
