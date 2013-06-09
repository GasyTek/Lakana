using System;
using System.Collections.Generic;
using System.Linq;

namespace GasyTek.Lakana.Common.Communication
{
    /// <summary>
    /// Singleton object that represents a message bus for the application.
    /// Views and View models communicate messages via the message bus.
    /// </summary>
    public static class MessageBus
    {
        private static readonly MessageBusImpl MessageBusInstance;

        static MessageBus()
        {
            MessageBusInstance = new MessageBusImpl();
        }

        /// <summary>
        /// Publishes the specified message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="message">The message.</param>
        public static void Publish<TMessage>(TMessage message) where TMessage : Message
        {
            MessageBusInstance.Publish(typeof(TMessage), message);
        }

        /// <summary>
        /// Subscribes to the specified message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="messageListener"></param>
        /// <returns></returns>
        public static void Subscribe<TMessage>(IMessageListener messageListener) where TMessage : Message
        {
            MessageBusInstance.Subscribe(typeof(TMessage), messageListener);
        }

        #region Private class MessageBusImpl

        /// <summary>
        /// Default implementation.
        /// </summary>
        internal class MessageBusImpl
        {
            public event MessagePublishedEventHandler MessagePublished;

            private List<WeakReference> _observers;

            internal MessageBusImpl()
            {
                _observers = new List<WeakReference>();
            }

            internal void Publish(Type messageType, Message message)
            {
                var purgedObservers = _observers.Where(o => o.IsAlive).ToList();
                _observers = purgedObservers;

                OnMessagePublished(message);
            }

            internal IDisposable Subscribe(Type messageType, IMessageListener messageListener)
            {
                var observer = new MessagePublishedWeakEventListener(this, messageType, messageListener);
                _observers.Add(new WeakReference(observer));

                MessagePublishedEventManager.AddListener(this, observer);

                return observer;
            }

            protected virtual void OnMessagePublished(Message message)
            {
                var handler = MessagePublished;
                if (handler != null) handler(this, message);
            }
        }

        #endregion
    }
}
