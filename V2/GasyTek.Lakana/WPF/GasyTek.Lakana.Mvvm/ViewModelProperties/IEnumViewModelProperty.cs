namespace GasyTek.Lakana.Mvvm.ViewModelProperties
{
    /// <summary>
    /// A view model property that wraps an enum with its values.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum.</typeparam>
    public interface IEnumViewModelProperty<TEnum>
        : ILookupViewModelProperty<TEnum, IEnumItem<TEnum>> where TEnum : struct
    {
    }
}