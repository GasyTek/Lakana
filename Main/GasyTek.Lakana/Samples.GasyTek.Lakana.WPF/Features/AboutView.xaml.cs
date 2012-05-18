using System.Windows;
using GasyTek.Lakana.WPF.Common;
using GasyTek.Lakana.WPF.Services;
using Samples.GasyTek.Lakana.WPF.Common;

namespace Samples.GasyTek.Lakana.WPF.Features
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class AboutView : IPresentable
    {
        private readonly IUiMetadata _uiMetadata;


        public AboutView()
        {
            InitializeComponent();

            _uiMetadata = new UiMetadata { LabelProvider = () => "About" };
        }

        public IUiMetadata UiMetadata
        {
            get { return _uiMetadata; }
        }

        private void SendMailClick(object sender, RoutedEventArgs e)
        {
            // HOW TO : open a modal view on top of another one
            var navigationInfo = NavigationInfo.CreateComplex(ViewId.SendMail, ViewId.About, false);
            Singletons.NavigationService.ShowModal<SendMailView>(navigationInfo);
        }
    }
}
