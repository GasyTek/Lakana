using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GasyTek.Lakana.Mvvm.Validation
{
    /// <summary>
    /// Base class for validation engines.
    /// </summary>
    public abstract class ValidationEngineBase : IValidationEngine
    {
        /// <summary>
        /// Gets the errors organized per property name.
        /// </summary>
        protected ConcurrentDictionary<string, List<string>> Errors { get; private set; }

        protected ValidationEngineBase()
        {
            Errors = new ConcurrentDictionary<string, List<string>>();
        }

        protected abstract void OnValidateAsync(PropertyInfo property, object propertyValue);

        protected virtual void OnRaiseErrorsChangedEvent(string propertyName)
        {
            if (ErrorsChangedEvent != null)
                ErrorsChangedEvent(this, new ErrorsChangedEventArgs(propertyName));
        }

        #region IValidationEngine members

        public event ErrorsChangedEventHandler ErrorsChangedEvent;

        public IEnumerable<string> GetErrors(string propertyName)
        {
            if (!Errors.ContainsKey(propertyName)) return Enumerable.Empty<string>();
            if (Errors[propertyName] == null || !Errors[propertyName].Any()) return Enumerable.Empty<string>();
            return Errors[propertyName];
        }

        public bool IsValid(string propertyName)
        {
            return GetErrors(propertyName).Any() == false;
        }

        public void ValidateAsync(PropertyInfo property, object value)
        {
            OnValidateAsync(property, value);
        }

        #endregion
    }
}