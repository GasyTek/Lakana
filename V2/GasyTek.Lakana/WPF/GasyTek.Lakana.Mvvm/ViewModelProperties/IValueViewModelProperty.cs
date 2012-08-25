namespace GasyTek.Lakana.Mvvm.ViewModelProperties
{
    /// <summary>
    /// A view model property that wraps single value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface IValueViewModelProperty<TValue> : IViewModelProperty
    {
        /// <summary>
        /// The strong typed value.
        /// </summary>
        TValue Value { get; set; }

        /// <summary>
        /// The strong typed original value.
        /// </summary>
        TValue OriginalValue { get; }

    }
}
