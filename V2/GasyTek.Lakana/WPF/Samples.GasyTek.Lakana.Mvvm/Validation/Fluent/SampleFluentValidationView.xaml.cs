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
            DataContext = CreateViewModel();
        }

        private SampleFluentValidationViewModel _viewModel;
        private SampleFluentValidationViewModel CreateViewModel()
        {
            if (_viewModel == null)
            {
                // 1 - create the model
                var employee = new Employee { Code = "EMP", Age = 32, Country = Database.GetCountry(4), Rank = Rank.Boss };

                // 2 - create the view model
                _viewModel = new SampleFluentValidationViewModel();

                // 3 - attach him the model to edit
                _viewModel.Model = employee;
            }

            return _viewModel;
        }
    }
}
