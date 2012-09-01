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

        private void UserControlLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DataContext = CreateViewModel();
        }

        private SampleDataAnnotationValidationViewModel _viewModel;
        private SampleDataAnnotationValidationViewModel CreateViewModel()
        {
            if(_viewModel == null)
            {
                // 1 - create the model
                var employee = new Employee { Code = "EMP", Age = 32, Country = Database.GetCountry(4), Rank = Rank.Boss };

                // 2 - create the view model
                _viewModel = new SampleDataAnnotationValidationViewModel();

                // 3 - attach him the model to edit
                _viewModel.Model = employee;
            }

            return _viewModel;
        }
    }
}
