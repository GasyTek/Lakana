using System.Windows;
using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Navigation.Attributes;
using GasyTek.Lakana.Navigation.Services;
using Samples.GasyTek.Lakana.Navigation.Common;

namespace Samples.GasyTek.Lakana.Navigation.Features
{
    /// <summary>
    /// Interaction logic for SendMailView.xaml
    /// </summary>
    [ViewKey(ViewId.SendMail)]
    public partial class SendMailView : IPresentable
    {
        public SendMailView()
        {
            InitializeComponent();

            UIMetadata= new UIMetadata { LabelProvider = () => "Send mail"};
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            NavigationManager.Close(ViewId.SendMail, TxtModalResult.Text);
        }

        public IUIMetadata UIMetadata { get; private set; }
    }
}
