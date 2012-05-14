using System;
using System.ComponentModel;
using System.Linq.Expressions;
using GasyTek.Lakana.WPF.Extensions;

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

    public static class NotifyPropertyChangedUtil
    {
        /// <summary>
        /// Extension method which is applied on a NotifyPropertyChangedBase derived class.
        /// </summary>
        public static void NotifyPropertyChanged<TPropertyOwner>(this TPropertyOwner owner
            , Expression<Func<TPropertyOwner, object>> propertyExpr) where TPropertyOwner : NotifyPropertyChangedBase
        {
            var propertyName = owner.GetPropertyName(propertyExpr);
            owner.RaisePropertyChanged(propertyName);
        }

        /// <summary>
        /// Sets the property value then notify property changed
        /// </summary>
        /// <typeparam name="TPropertyOwner">The type of the property owner.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="owner">The owner.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="propertyExpr">The property expr.</param>
        public static void SetPropertyValue<TPropertyOwner, TProperty>(this TPropertyOwner owner
            , ref TProperty oldValue
            , TProperty newValue
            , Expression<Func<TPropertyOwner, object>> propertyExpr) where TPropertyOwner : NotifyPropertyChangedBase
        {
            if (!Equals(oldValue, newValue))
            {
                oldValue = newValue;
                owner.NotifyPropertyChanged(propertyExpr);
            }
        }
    }
}
