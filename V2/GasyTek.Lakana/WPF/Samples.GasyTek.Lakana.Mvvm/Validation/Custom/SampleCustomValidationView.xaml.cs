namespace Samples.GasyTek.Lakana.Mvvm.Validation.Custom
{
    /// <summary>
    /// Interaction logic for SampleCustomValidationView.xaml
    /// </summary>
    public partial class SampleCustomValidationView
    {
        public SampleCustomValidationView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DataContext = new SampleCustomValidationViewModel();
        }
    }
}
