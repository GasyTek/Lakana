﻿using System.Windows;

namespace GasyTek.Lakana.Common.Communication
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="message">The message.</param>
    internal delegate void MessagePublishedEventHandler(object sender, Message message);

    /// <summary>
    /// 
    /// </summary>
    public class MessagePublishedEventManager : WeakEventManager
    {
        private static MessagePublishedEventManager CurrentManager
        {
            get
            {
                var managerType = typeof(MessagePublishedEventManager);
                var manager = GetCurrentManager(managerType) as MessagePublishedEventManager;
                if (manager == null)
                {
                    manager = new MessagePublishedEventManager();
                    SetCurrentManager(managerType, manager);
                }
                return manager;
            }
        }

        /// <summary>
        /// Adds the listener.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="listener">The listener.</param>
        public static void AddListener(object source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedAddListener(source, listener);
        }

        /// <summary>
        /// Removes the listener.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="listener">The listener.</param>
        public static void RemoveListener(object source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        /// <summary>
        /// When overridden in a derived class, starts listening for the event being managed. After <see cref="M:System.Windows.WeakEventManager.StartListening(System.Object)" />  is first called, the manager should be in the state of calling <see cref="M:System.Windows.WeakEventManager.DeliverEvent(System.Object,System.EventArgs)" /> or <see cref="M:System.Windows.WeakEventManager.DeliverEventToList(System.Object,System.EventArgs,System.Windows.WeakEventManager.ListenerList)" /> whenever the relevant event from the provided source is handled.
        /// </summary>
        /// <param name="source">The source to begin listening on.</param>
        protected override void StartListening(object source)
        {
            var manager = (MessageBus.IMessageBusImpl)source;
            manager.MessagePublished += OnMessagePublished;
        }

        /// <summary>
        /// When overridden in a derived class, stops listening on the provided source for the event being managed.
        /// </summary>
        /// <param name="source">The source to stop listening on.</param>
        protected override void StopListening(object source)
        {
            var manager = (MessageBus.IMessageBusImpl)source;
            manager.MessagePublished -= OnMessagePublished;
        }

        private void OnMessagePublished(object sender, Message message)
        {
            DeliverEvent(sender, message);
        }
    }
}