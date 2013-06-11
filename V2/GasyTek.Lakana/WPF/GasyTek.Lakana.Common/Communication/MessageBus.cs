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
        private static readonly Dictionary<Type, object> MessageBusInstances;

        static MessageBus()
        {
            MessageBusInstances = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Publishes the specified message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="message">The message.</param>
        public static void Publish<TMessage>(TMessage message) where TMessage : Message
        {
            var messageType = typeof (TMessage);
            if (MessageBusInstances.ContainsKey(messageType))
            {
                ((MessageBusImpl<TMessage>)MessageBusInstances[messageType]).Publish(message);
            }
        }

        /// <summary>
        /// Subscribes to the specified message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="messageListener"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<TMessage>(IMessageListener<TMessage> messageListener) where TMessage : Message
        {
            var messageType = typeof(TMessage);
            if (!MessageBusInstances.ContainsKey(messageType))
            {
                MessageBusInstances.Add(messageType, new MessageBusImpl<TMessage>());
            }

            return ((MessageBusImpl<TMessage>)MessageBusInstances[messageType]).Subscribe(messageListener);
        }

        #region Internal class MessageBusImpl

        internal interface IMessageBusImpl
        {
            event MessagePublishedEventHandler MessagePublished;
        } 

        /// <summary>
        /// Default implementation.
        /// </summary>
        internal class MessageBusImpl<TMessage> : IMessageBusImpl where TMessage : Message
        {
            public event MessagePublishedEventHandler MessagePublished;

            private List<WeakReference> _observers;

            internal MessageBusImpl()
            {
                _observers = new List<WeakReference>();
            }

            internal void Publish(TMessage message)
            {
                _observers = _observers.Where(o => o.IsAlive).ToList();

                OnMessagePublished(message);
            }

            internal IDisposable Subscribe(IMessageListener<TMessage> messageListener)
            {
                var observer = new MessagePublishedWeakEventListener<TMessage>(this, messageListener);
                _observers.Add(new WeakReference(observer));

                MessagePublishedEventManager.AddListener(this, observer);

                return observer;
            }

            protected virtual void OnMessagePublished(TMessage message)
            {
                var handler = MessagePublished;
                if (handler != null) handler(this, message);
            }
        }

        #endregion
    }
}
