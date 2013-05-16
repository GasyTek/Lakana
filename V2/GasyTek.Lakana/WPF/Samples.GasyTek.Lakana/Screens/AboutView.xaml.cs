using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Navigation.Attributes;
using GasyTek.Lakana.Navigation.Services;
using Samples.GasyTek.Lakana.Utils;
using Samples.GasyTek.Lakana.Resources;
using System.Globalization;

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

            _uiMetadata = new UIMetadata { LabelProvider = () => Texts.About };
        }

        public IUIMetadata UIMetadata
        {
            get { return _uiMetadata; }
        }

        private void CmbLanguages_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // English
            if (CmbLanguages.SelectedIndex == 0)
            {
                LocalizationManager.ChangeCulture(new CultureInfo("en-US"));
            }

            // French
            if (CmbLanguages.SelectedIndex == 1)
            {
                LocalizationManager.ChangeCulture(new CultureInfo("fr-FR"));
            }
        }
    }
}
