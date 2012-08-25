using System;

namespace GasyTek.Lakana.Mvvm.ViewModelProperties
{
    /// <summary>
    /// Offers possibility to retrieve the value of a property and its type.
    /// </summary>
    public interface IHasValue
    {
        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        object GetValue();
        
        /// <summary>
        /// Gets the type of the returned value.
        /// </summary>
        /// <remarks>This can be used to cast he generic value to a more specific type</remarks>
        Type GetValueType();
    }
}