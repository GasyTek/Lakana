using System;

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

        private void UserControlLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DataContext = CreateViewModel();
        }

        private SampleCustomValidationViewModel _viewModel;
        private SampleCustomValidationViewModel CreateViewModel()
        {
            if (_viewModel == null)
            {
                // 1 - create the model
                var employee = new Employee
                                   {
                                       Code = "EMP",
                                       Age = 32,
                                       Country = Database.GetCountry(4),
                                       Rank = Rank.Boss,
                                       DateOfBirth = new DateTime(1982, 6, 3),
                                       DateOfHire = new DateTime(2000, 10, 24)
                                   };

                // 2 - create the view model
                _viewModel = new SampleCustomValidationViewModel();

                // 3 - attach him the model to edit
                _viewModel.Model = employee;
            }

            return _viewModel;
        }
    }
}
