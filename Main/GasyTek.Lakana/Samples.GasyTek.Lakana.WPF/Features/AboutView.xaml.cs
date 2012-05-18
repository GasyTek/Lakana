using System.Threading.Tasks;
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
            var navigationInfo = NavigationInfo.CreateComplex(ViewId.SendMail, ViewId.About, false);
            var modalResult = Singletons.NavigationService.ShowModal<SendMailView>(navigationInfo);
            modalResult.Result.ContinueWith(r =>
                                                {
                                                    txtDisplayModalResult.Text = "Modal Result : " + r.Result.ToString();
                                                }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
