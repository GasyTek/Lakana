using System.Threading.Tasks;

namespace GasyTek.Lakana.WPF.Services
{
    /// <summary>
    /// An object that represents the result returned by a modal view that was shown.
    /// </summary>
    public class ModalResult
    {
        public Task<object> Result { get; internal set; }
        public ViewInfo ViewInfo { get; internal set; }
    }
}