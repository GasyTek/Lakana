using System.ComponentModel;
using System.Reflection;
using GasyTek.Lakana.Common.UI;

namespace GasyTek.Lakana.Mvvm.ViewModelProperties
{
    /// <summary>
    /// Represents rich properties that are attached to a view model.
    /// </summary>
    public interface IViewModelProperty : IDataErrorInfo, INotifyPropertyChanged, IHasPropertyMetadata, IHasValidationEngine, IHasValue
    {
        /// <summary>
        /// Gets or sets the presentation metadata.
        /// </summary>
        /// <value>
        /// The presentation metadata.
        /// </value>
        IUIMetadata UIMetadata { get; set; }

        /// <summary>
        /// Gets the metadata informations for this property.
        /// </summary>
        /// <returns></returns>
        PropertyInfo PropertyMetadata { get; }

        /// <summary>
        /// Gets a value indicating whether this property value has changed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has changed; otherwise, <c>false</c>.
        /// </value>
        bool HasChanged { get; }
        
        /// <summary>
        /// Gets a value indicating whether this property value is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        bool IsValid { get; }
    }
}
