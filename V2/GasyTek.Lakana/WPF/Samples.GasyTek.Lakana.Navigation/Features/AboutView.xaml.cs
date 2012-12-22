using System.Threading.Tasks;
using System.Windows;
using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Navigation.Attributes;
using GasyTek.Lakana.Navigation.Services;
using Samples.GasyTek.Lakana.Navigation.Common;

namespace Samples.GasyTek.Lakana.Navigation.Features
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    [ViewKey(ViewId.About)]
    public partial class AboutView : IPresentable
    {
        private readonly IUIMetadata _uiMetadata;


        public AboutView()
        {
            InitializeComponent();

            _uiMetadata = new UIMetadata { LabelProvider = () => "About" };
        }

        public IUIMetadata UIMetadata
        {
            get { return _uiMetadata; }
        }

        private void SendMailClick(object sender, RoutedEventArgs e)
        {
            // HOW TO : open a modal view on top of another one
            // Here is an example of a modal custom view.
            // The modal view returns a Task<TResult>, that means that you can use the C#5 async/await pattern to improve this code.
            // Notice that you have to use TaskScheduler.FromCurrentSynchronizationContext()
            // in order to use the UI thread synchronization context
            var modalResult = NavigationManager.ShowModal<string>(ViewId.About + "/" + ViewId.SendMail);
            modalResult.AsyncResult.ContinueWith(r =>
                                                {
                                                    txtDisplayModalResult.Text = "Modal Result : " + r.Result;
                                                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            description.Text = GetDescription();
        }

        private string GetDescription()
        {
            return "By clicking on 'Send mail', you will see a custom view that will be shown as a modal view.\r\nModal views can return values.\r\n\r\n" +
                   "Features demonstrated : " +
                   "\r\n > Custom views displayed as modal";
        }
    }
}
