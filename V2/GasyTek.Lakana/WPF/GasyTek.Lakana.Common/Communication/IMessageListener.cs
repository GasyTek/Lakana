using System;

namespace GasyTek.Lakana.Common.Communication
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageListener
    {
        /// <summary>
        /// For infrastructure use only.
        /// </summary>
        IDisposable SubscriptionHandle { get; set; }

        /// <summary>
        /// Called when message is received.
        /// </summary>
        /// <param name="message">The message.</param>
        void OnMessageReceived(Message message);
    }
}