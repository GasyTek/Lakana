using System.ComponentModel;

namespace GasyTek.Lakana.Common.Base
{
    /// <summary>
    /// Base class offering a default implementation of INotifyPropertyChanged
    /// </summary>
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected internal virtual void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}