using System.Windows;
using Samples.GasyTek.Lakana.Navigation.Common;

namespace Samples.GasyTek.Lakana.Navigation.Features
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
            Singletons.NavigationService.Close(ViewId.SendMail, txtModalResult.Text);
        }
    }
}
