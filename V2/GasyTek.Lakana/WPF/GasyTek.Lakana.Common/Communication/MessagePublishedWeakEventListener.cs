using System;
using System.Windows;

namespace GasyTek.Lakana.Common.Communication
{
    /// <summary>
    /// 
    /// </summary>
    internal class MessagePublishedWeakEventListener<TMessage> : IWeakEventListener, ISubscriptionToken<TMessage>, IDisposable where TMessage : Message
    {
        private bool _disposed;
        private readonly object _source;
        private readonly IMessageListener<TMessage> _messageListener;

        #region Constructor / Destructor

        public MessagePublishedWeakEventListener(MessageBus.MessageBusImpl<TMessage> source, IMessageListener<TMessage> messageListener)
        {
            _source = source;
            _messageListener = messageListener;

            _messageListener.SubscriptionToken = this;  // TODO : review !
        }

        ~MessagePublishedWeakEventListener()
        {
            Dispose(false);
        }

        #endregion

        #region IWeakEventListener members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(MessagePublishedEventManager) && typeof(TMessage) == e.GetType())
            {
                OnMessagePublised(e as TMessage);
                return true;
            }
            return false;
        }

        private void OnMessagePublised(TMessage message)
        {
            _messageListener.OnMessageReceived(message);
        }

        #endregion

        #region ISubscriptionToken members

        public void Unsubscribe()
        {
            Dispose();
        }

        #endregion

        #region IDisposable members

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (_disposed == false)
            {
                MessagePublishedEventManager.RemoveListener(_source, this);
                _disposed = true;
            }
        }

        #endregion
    }
}
