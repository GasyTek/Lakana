using System.Collections.Generic;

namespace GasyTek.Lakana.Mvvm.Validation
{
    /// <summary>
    /// An object that manages validation rules on properties.
    /// </summary>
    public interface IValidationEngine
    {
        /// <summary>
        /// Occurs when errors for the specified property where updated.
        /// </summary>
        event ErrorsChangedEventHandler ErrorsChangedEvent;

        /// <summary>
        /// Gets the errors currently associated with the specified proeprty.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        IEnumerable<string> GetErrors(string propertyName);

        /// <summary>
        /// Determines whether the specified property is valid.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///   <c>true</c> if the specified property is valid; otherwise, <c>false</c>.
        /// </returns>
        bool IsValid(string propertyName);

        /// <summary>
        /// Validates the provided value of the provided property.
        /// </summary>
        /// <param name="validationParameter"> </param>
        void Validate(ValidationParameter validationParameter);
    }
}