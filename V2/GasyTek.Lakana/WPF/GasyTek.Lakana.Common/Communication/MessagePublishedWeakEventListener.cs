using System;
using System.Windows;

namespace GasyTek.Lakana.Common.Communication
{
    /// <summary>
    /// 
    /// </summary>
    internal class MessagePublishedWeakEventListener : IWeakEventListener, IDisposable
    {
        private bool _disposed;
        private readonly object _source;
        private readonly Type _messageType;
        private readonly IMessageListener _messageListener;

        public MessagePublishedWeakEventListener(MessageBus.MessageBusImpl source, Type messageType, IMessageListener messageListener)
        {
            _source = source;
            _messageType = messageType;
            _messageListener = messageListener;

            _messageListener.SubscriptionHandle = this;
        }

        ~MessagePublishedWeakEventListener()
        {
            Dispose(false);
        }

        #region IWeakEventListener members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(MessagePublishedEventManager) && _messageType == e.GetType())
            {
                OnMessagePublised(e as Message);
                return true;
            }
            return false;
        }

        private void OnMessagePublised(Message message)
        {
            _messageListener.OnMessageReceived(message);
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
