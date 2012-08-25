using System.ComponentModel;

namespace GasyTek.Lakana.WPF.Common
{
    /// <summary>
    /// Base class offering a default implementation of INotifyPropertyChanged
    /// </summary>
    /// <remarks>Methods and events are marked "virtual" just to support NHibernate</remarks>
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
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
