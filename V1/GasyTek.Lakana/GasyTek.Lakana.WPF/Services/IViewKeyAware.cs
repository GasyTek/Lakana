namespace GasyTek.Lakana.WPF.Services
{
    /// <summary>
    /// Supported by objects that should be aware of the key of the view with which it is tied.
    /// </summary>
    public interface IViewKeyAware
    {
        string ViewKey { get; set; }
    }
}
