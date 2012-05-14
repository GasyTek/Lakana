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
        private readonly IPresentationMetadata _presentationMetadata;


        public AboutView()
        {
            InitializeComponent();

            _presentationMetadata = new PresentationMetadata { LabelProvider = () => "About" };
        }

        public IPresentationMetadata PresentationMetadata
        {
            get { return _presentationMetadata; }
        }

        private void SendMailClick(object sender, RoutedEventArgs e)
        {
            // HOW TO : open a modal view on top of another one
            var navigationInfo = NavigationInfo.CreateComplex(ViewId.SendMail, ViewId.About, false);
            Singletons.NavigationService.ShowModal<SendMailView>(navigationInfo);
        }
    }
}
