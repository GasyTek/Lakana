using GasyTek.Lakana.Common.UI;

namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// Class that provides presentation metadatas for objects that can be displayed.
    /// </summary>
    public interface IPresentable
    {
        IUIMetadata UIMetadata { get; }
    }
}
