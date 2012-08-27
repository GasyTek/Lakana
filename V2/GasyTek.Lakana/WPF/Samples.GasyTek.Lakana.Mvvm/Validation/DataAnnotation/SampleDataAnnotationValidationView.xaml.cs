using System.Windows.Controls;

namespace Samples.GasyTek.Lakana.Mvvm.Validation.DataAnnotation
{
    /// <summary>
    /// Interaction logic for SampleDataAnnotationValidationView.xaml
    /// </summary>
    public partial class SampleDataAnnotationValidationView
    {
        public SampleDataAnnotationValidationView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DataContext = new SampleDataAnnotationValidationViewModel();
        }
    }
}
