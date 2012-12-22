using System.Windows;
using GasyTek.Lakana.Navigation.Attributes;
using GasyTek.Lakana.Navigation.Services;
using Samples.GasyTek.Lakana.Navigation.Common;

namespace Samples.GasyTek.Lakana.Navigation.Features
{
    /// <summary>
    /// Interaction logic for SendMailView.xaml
    /// </summary>
    [ViewKey(ViewId.SendMail)]
    public partial class SendMailView
    {
        public SendMailView()
        {
            InitializeComponent();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            NavigationManager.Close(ViewId.SendMail, txtModalResult.Text);
        }
    }
}
