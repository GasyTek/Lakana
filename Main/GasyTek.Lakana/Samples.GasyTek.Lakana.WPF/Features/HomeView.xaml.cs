using GasyTek.Lakana.WPF.Common;
using GasyTek.Lakana.WPF.Services;

namespace Samples.GasyTek.Lakana.WPF.Features
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : IPresentable
    {
        private readonly IUiMetadata _uiMetadata;

        public HomeView()
        {
            InitializeComponent();

            _uiMetadata = new UiMetadata {LabelProvider = () => "Home"};
        }

        public IUiMetadata UiMetadata
        {
            get { return _uiMetadata; }
        }
    }
}
