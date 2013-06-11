namespace GasyTek.Lakana.Common.Communication
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface ISubscriptionToken<TMessage> where TMessage : Message
    {
        /// <summary>
        /// Unsubscribe from the message bus.
        /// </summary>
        void Unsubscribe();
    }
}