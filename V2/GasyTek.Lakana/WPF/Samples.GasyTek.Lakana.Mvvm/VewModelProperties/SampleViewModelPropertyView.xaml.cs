namespace Samples.GasyTek.Lakana.Mvvm.VewModelProperties
{
    /// <summary>
    /// Interaction logic for SampleViewModelPropertyView.xaml
    /// </summary>
    public partial class SampleViewModelPropertyView
    {
        public SampleViewModelPropertyView()
        {
            InitializeComponent();
        }

        private void UserControlLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // 1 - create the model
            var employee = new Employee {Age = 32, Country = Database.GetCountry(4), Rank = Rank.Boss};

            // 2 - create the view model
             var sampleViewModelPropertyViewModel = new SampleViewModelPropertyViewModel();

            // 3 - attach him the model to edit
            sampleViewModelPropertyViewModel.Model = employee;

            // 4 - set the view model as datacontext of the viewss
            DataContext = sampleViewModelPropertyViewModel;
        }
    }
}
