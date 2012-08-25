using System.Collections.ObjectModel;

namespace GasyTek.Lakana.Mvvm.ViewModelProperties
{
    /// <summary>
    /// A view model property that picks its value from a given lookup.
    /// Expected to be used with ItemsControl.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to use.</typeparam>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    public interface ILookupViewModelProperty<TValue, TItem> : IViewModelProperty
    {
        /// <summary>
        /// Gets or sets the selected value.
        /// </summary>
        /// <value>
        /// The selected value.
        /// </value>
        TValue SelectedValue { get; set; }

        /// <summary>
        /// Gets the original value.
        /// </summary>
        TValue OriginalValue { get; }

        /// <summary>
        /// Gets the items source.
        /// </summary>
        ObservableCollection<TItem> ItemsSource { get; }
    }
}