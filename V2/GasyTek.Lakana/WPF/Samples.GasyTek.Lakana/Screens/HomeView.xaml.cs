using System.Windows.Media;
using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Navigation.Attributes;
using GasyTek.Lakana.Navigation.Services;
using Samples.GasyTek.Lakana.Utils;
using Samples.GasyTek.Lakana.Resources;

namespace Samples.GasyTek.Lakana.Screens
{
    /// <summary>
    /// Logique d'interaction pour HomeView.xaml
    /// </summary>
    [ViewKey(ScreenId.Home)]
    public partial class HomeView : IPresentable
    {
        private readonly IUIMetadata _uiMetadata;

        public HomeView()
        {
            InitializeComponent();

            _uiMetadata = new UIMetadata { LabelProvider = () => Texts.Home };
        }

        public IUIMetadata UIMetadata
        {
            get { return _uiMetadata; }
        }
    }
}