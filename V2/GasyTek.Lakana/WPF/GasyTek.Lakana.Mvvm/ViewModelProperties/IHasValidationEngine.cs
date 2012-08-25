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
    }
}