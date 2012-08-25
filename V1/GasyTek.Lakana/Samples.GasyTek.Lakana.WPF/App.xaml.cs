using System.Windows;

namespace Samples.GasyTek.Lakana.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            var mainWindowView = new MainWindowView {DataContext = new MainWindowViewModel()};
            mainWindowView.Show();
        }
    }
}
