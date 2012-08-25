using System.Reflection;

namespace GasyTek.Lakana.Mvvm.ViewModelProperties
{
    /// <summary>
    /// Allow view mode properties to supports property metadata.
    /// <remarks>This is used internally by the infrastructure.</remarks>
    /// </summary>
    public interface IHasPropertyMetadata
    {
        /// <summary>
        /// Gets or sets the informations for this member in the viewmodel.
        /// </summary>
        /// <value>
        /// The member info.
        /// </value>
        PropertyInfo InternalPropertyMetadata { get; set; }
    }
}