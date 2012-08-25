using System;
using System.Linq.Expressions;
using GasyTek.Lakana.Common.Base;

namespace GasyTek.Lakana.Common.Extensions
{
    public static class NotifyPropertyChangedExtensions
    {
        /// <summary>
        /// Extension method which is applied on a NotifyPropertyChangedBase derived class.
        /// </summary>
        public static void NotifyPropertyChanged<TPropertyOwner>(this TPropertyOwner owner
                                                                 , params Expression<Func<TPropertyOwner, object>> [] propertyExpressions) where TPropertyOwner : NotifyPropertyChangedBase
        {
            foreach (var expression in propertyExpressions)
            {
                var propertyName = owner.GetPropertyName(expression);
                owner.RaisePropertyChanged(propertyName);
            }
        }

        /// <summary>
        /// Sets the property value then notify property changed
        /// </summary>
        /// <typeparam name="TPropertyOwner">The type of the property owner.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="owner">The owner.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="propertyExpressions">The property expr.</param>
        public static void SetPropertyValue<TPropertyOwner, TProperty>(this TPropertyOwner owner
                                                                       , ref TProperty oldValue
                                                                       , TProperty newValue
                                                                       , params Expression<Func<TPropertyOwner, object>> [] propertyExpressions) where TPropertyOwner : NotifyPropertyChangedBase
        {
            if (!Equals(oldValue, newValue))
            {
                oldValue = newValue;
                owner.NotifyPropertyChanged(propertyExpressions);
            }
        }
    }
}