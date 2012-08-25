using GasyTek.Lakana.Common.UI;

namespace GasyTek.Lakana.Mvvm.ViewModelProperties
{
    /// <summary>
    /// Represents a member of a lookup for presentation.
    /// </summary>
    /// <typeparam name="TEnum">The type of the item.</typeparam>
    public interface IEnumItem<out TEnum> where TEnum : struct
    {
        /// <summary>
        /// Gets or sets the presentation metadata.
        /// </summary>
        /// <value>
        /// The presentation metadata.
        /// </value>
        IUIMetadata UIMetadata { get; }

        /// <summary>
        /// Shortcut to label property of presentation metadata.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        TEnum Value { get; }
    }
}