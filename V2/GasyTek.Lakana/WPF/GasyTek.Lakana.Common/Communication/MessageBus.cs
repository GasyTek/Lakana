using System;
using System.Collections.Generic;

namespace GasyTek.Lakana.Common.Communication
{
    /// <summary>
    /// Singleton object that represents a message bus for the application.
    /// Views and View models communicate messages via the message bus.
    /// </summary>
    public static class MessageBus
    {
        private static readonly MessageBusImpl _messageBusImpl;

        static MessageBus()
        {
            _messageBusImpl = new MessageBusImpl();
        }

        /// <summary>
        /// Publishes the specified message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="message">The message.</param>
        public static void Publish<TMessage>(TMessage message) where TMessage : Message
        {
            _messageBusImpl.Publish(typeof(TMessage), message);
        }

        /// <summary>
        /// Subscribes to the specified message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="onMessageReceived">The callback to use to process the message.</param>
        /// <returns></returns>
        public static IDisposable Subscribe<TMessage>(Action<TMessage> onMessageReceived) where TMessage : Message
        {
            // TODO : use weak references to avoid memory leak
            return _messageBusImpl.Subscribe(typeof(TMessage), message => onMessageReceived((TMessage)message));
        }

        #region Private class MessageBusImpl

        /// <summary>
        /// Default implementation.
        /// </summary>
        private class MessageBusImpl
        {
            private readonly IDictionary<Type, List<MessageObserver>> _observers;

            internal MessageBusImpl()
            {
                _observers = new Dictionary<Type, List<MessageObserver>>();
            }

            internal void Publish(Type messageType, Message message)
            {
                if (_observers.ContainsKey(messageType))
                {
                    foreach (var observer in _observers[messageType])
                    {
                        observer.OnNext(message);
                    }
                }
            }

            internal IDisposable Subscribe(Type messageType, Action<Message> onMessageReceived)
            {
                var messageObservers = new List<MessageObserver>();

                if (!_observers.ContainsKey(messageType))
                {
                    _observers.Add(messageType, messageObservers);
                }
                else
                {
                    messageObservers = _observers[messageType];
                }

                var observer = new MessageObserver(_observers[messageType], onMessageReceived);
                messageObservers.Add(observer);
                return observer;
            }

            internal void UnsubscribeAll(Type messageType)
            {
                if (_observers.ContainsKey(messageType))
                {
                    foreach (var observer in _observers[messageType])
                    {
                        observer.Dispose();
                    }
                }
            }
        }

        #endregion
    }
}
