using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GasyTek.Lakana.Common.Communication
{
    /// <summary>
    /// Base class for all messages that will be exchanged through the application.
    /// </summary>
    public abstract class Message
    {
        /// <summary>
        /// Gets the original sender of the message.
        /// </summary>
        public object Sender
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        protected Message(object sender)
        {
            Sender = sender;
        }
    }
}
