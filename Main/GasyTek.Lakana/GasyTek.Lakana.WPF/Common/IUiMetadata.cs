using System;
using System.Windows.Media;

namespace GasyTek.Lakana.WPF.Common
{
    /// <summary>
    /// Metadata used by views and view models to display informations such as view title etc.
    /// </summary>
    public interface IUiMetadata
    {
        /// <summary>
        /// Gets the label.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Gets or sets the label provider.
        /// </summary>
        /// <value>
        /// The label provider.
        /// </value>
        Func<string> LabelProvider { get; set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets or sets the description provider.
        /// </summary>
        /// <value>
        /// The description provider.
        /// </value>
        Func<string> DescriptionProvider { get; set; }

        /// <summary>
        /// Gets the icon.
        /// </summary>
        ImageSource Icon { get; }

        /// <summary>
        /// Gets or sets the icon provider.
        /// </summary>
        /// <value>
        /// The icon provider.
        /// </value>
        Func<ImageSource> IconProvider { get; set; }
    }
}
