using System.Windows.Data;

namespace GasyTek.Lakana.Mvvm.Utils
{
    /// <summary>
    /// A pre configured binding that can be used in xaml files to bind the view to view model properties.
    /// This binding is exactly the same as = "{Binding Path=..., Mode=TwoWay, UpdateSourceTrigger=PropertyChanged, ValidatesOnDataErrors=True}".\r\n
    /// You can still override the binding properties if not needed.
    /// </summary>
    public class Binding : System.Windows.Data.Binding
    {
        /// <summary>
        /// Initializes the pre-configured binding instance<see cref="Binding"/> class.
        /// </summary>
        public Binding ()
        {
            ValidatesOnDataErrors = true;
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            Mode = BindingMode.TwoWay;
        }

        /// <summary>
        /// Initializes the pre-configured binding instance<see cref="Binding"/> class.
        /// </summary>
        public Binding(string path)
            : base(path)
        {
            ValidatesOnDataErrors = true;
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            Mode = BindingMode.TwoWay;
        }
    }
}
