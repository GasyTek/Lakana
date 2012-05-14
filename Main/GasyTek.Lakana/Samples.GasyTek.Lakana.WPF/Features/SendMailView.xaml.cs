using System.Windows;
using Samples.GasyTek.Lakana.WPF.Common;

namespace Samples.GasyTek.Lakana.WPF.Features
{
    /// <summary>
    /// Interaction logic for SendMailView.xaml
    /// </summary>
    public partial class SendMailView
    {
        public SendMailView()
        {
            InitializeComponent();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Singletons.NavigationService.Close(ViewId.SendMail);
        }
    }
}
