namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// Supported by objects that should be aware of the key of the view with which they are tied.
    /// </summary>
    public interface IViewKeyAware
    {
        string ViewKey { get; set; }
    }
}
