namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// Allows views or viewmodels to be notified when it is brought on top or hidden.
    /// </summary>
    public interface IActiveAware
    {
        /// <summary>
        /// Called just before activating the element.
        /// </summary>
        void OnActivating();

        /// <summary>
        /// Called after the element is activated.
        /// </summary>
        void OnActivated();
        
        /// <summary>
        /// Called just before deactivating the element.
        /// </summary>
        void OnDeactivating();

        /// <summary>
        /// Called after the element is deactivated.
        /// </summary>
        void OnDeactivated();
    }
}
