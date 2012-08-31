using System;
using System.Windows.Controls;

namespace Samples.GasyTek.Lakana.Mvvm.Validation
{
    /// <summary>
    /// Interaction logic for ProductRegistrationView.xaml
    /// </summary>
    public partial class ProductRegistrationView : UserControl
    {
        public ProductRegistrationView()
        {
            InitializeComponent();

            //var product = new Model() { Code = "aaa", BeginDistributionDate = DateTime.Now.AddDays(-3), EndDistributionDate = DateTime.Now.AddDays(3) };

            //DataContext = new ProductRegistrationViewModel { Model = product };
        }
    }
}
