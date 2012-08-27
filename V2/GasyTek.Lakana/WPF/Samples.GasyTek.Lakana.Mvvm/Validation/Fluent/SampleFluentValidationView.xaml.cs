namespace Samples.GasyTek.Lakana.Mvvm.Validation.Fluent
{
    /// <summary>
    /// Interaction logic for SampleFluentValidationView.xaml
    /// </summary>
    public partial class SampleFluentValidationView
    {
        public SampleFluentValidationView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DataContext = new SampleFluentValidationViewModel();
        }
    }
}
