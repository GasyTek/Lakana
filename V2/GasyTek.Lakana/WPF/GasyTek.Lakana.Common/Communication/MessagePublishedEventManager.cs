using System.Windows;

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

        protected override void StartListening(object source)
        {
            var manager = (MessageBus.IMessageBusImpl)source;
            manager.MessagePublished += OnMessagePublished;
        }

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