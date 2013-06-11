namespace GasyTek.Lakana.Common.Communication
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageListener<TMessage> where TMessage : Message
    {
        /// <summary>
        /// Token that can be used to unsusbscribe to the message bus.
        /// </summary>
        ISubscriptionToken<TMessage> SubscriptionToken { get; set; }

        /// <summary>
        /// Called when message is received.
        /// </summary>
        /// <param name="message">The message.</param>
        void OnMessageReceived(TMessage message);
    }
}