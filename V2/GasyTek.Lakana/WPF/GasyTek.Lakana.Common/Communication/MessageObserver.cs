using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GasyTek.Lakana.Common.Communication
{
    /// <summary>
    /// 
    /// </summary>
    internal class MessageObserver : IObserver<Message>, IDisposable
    {
        private List<MessageObserver> _observers;
        private Action<Message> _onNext;

        public MessageObserver(List<MessageObserver> observers, Action<Message> onNext)
        {
            _observers = observers;
            _onNext = onNext;
        }

        #region IObserver members

        public void OnNext(Message value)
        {
            _onNext(value);
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable members

        public void Dispose()
        {
            _observers.Remove(this);
            _onNext = null;
        }

        #endregion
    }
}
