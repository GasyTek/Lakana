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

        protected abstract void OnValidate(PropertyInfo property, object propertyValue);

        protected virtual void OnRaiseErrorsChangedEvent(string propertyName, params string [] optionalPropertyNames)
        {
            if (ErrorsChangedEvent != null)
            {
                var propertyNames = new List<string> {propertyName};
                propertyNames.AddRange(optionalPropertyNames);
                foreach (var pName in propertyNames)
                {
                    ErrorsChangedEvent(this, new ErrorsChangedEventArgs(pName));
                }
            }
        }

        #region IValidationEngine members

        /// <summary>
        /// Occurs when Errors where update.
        /// </summary>
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

        public void Validate(PropertyInfo property, object value)
        {
            OnValidate(property, value);
        }

        #endregion
    }
}