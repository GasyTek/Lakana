using System.Windows.Input;

namespace GasyTek.Lakana.Mvvm.ViewModelProperties
{
    /// <summary>
    /// Represents objects that provides a validation engine.
    /// <remarks>This is intended to be used internally by the infrastructure.</remarks>
    /// </summary>
    public interface IHasValidationEngine
    {
        /// <summary>
        /// Gets the validation engine object wrapper used to validate the property.
        /// </summary>
        /// <value>
        /// The validation engine.
        /// </value>
        ObservableValidationEngine InternalObservableValidationEngine { get; }

        /// <summary>
        /// Gets or sets a value indicating whether validation process is running or not.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is validating; otherwise, <c>false</c>.
        /// </value>
        bool IsValidating { get; set; }

        /// <summary>
        /// Gets the validation to cancel validation.
        /// </summary>
        ICommand CancelValidationCommand { get; }
    }
}