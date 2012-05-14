using GasyTek.Lakana.WPF.Common;

namespace GasyTek.Lakana.WPF.Services
{
    /// <summary>
    /// Class that provides presentation metadatas for objects that can be displayed.
    /// </summary>
    public interface IPresentable
    {
        IPresentationMetadata PresentationMetadata { get; }
    }
}
