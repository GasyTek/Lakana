using System;
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
        protected ConcurrentDictionary<string, ErrorCollection> Errors { get; private set; }

        protected ValidationEngineBase()
        {
            Errors = new ConcurrentDictionary<string, ErrorCollection>();
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
            if (Errors.ContainsKey(propertyName) == false) return Enumerable.Empty<string>();
            if (Errors[propertyName] == null || Errors[propertyName].IsEmpty) return Enumerable.Empty<string>();
            return Errors[propertyName].GetErrors();
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

        #region Protected class

        /// <summary>
        /// Class that contains information about the validation error.
        /// </summary>
        protected sealed class ErrorCollection
        {
            private readonly Dictionary<object, string> _errors;

            public string this[object key]
            {
                get { return _errors.ContainsKey(key) ? _errors[key] : null; }
            }

            public bool IsEmpty
            {
                get { return _errors.Count == 0; }
            }

            public ErrorCollection()
            {
                _errors = new Dictionary<object, string>();
            }

            public ErrorCollection(IEnumerable<string> errorMessages)
                : this()
            {
                AddErrors(errorMessages);
            }

            public void Clear()
            {
                _errors.Clear();
            }

            public void AddError(object key, string errorMessage)
            {
                if(_errors.ContainsKey(key) == false)
                    _errors.Add(key, errorMessage);
            }

            public void AddError(string errorMessage)
            {
                AddError(Guid.NewGuid().ToString(), errorMessage);
            }

            public void AddErrors(IEnumerable<string> errorMessages)
            {
                errorMessages.ToList().ForEach(AddError);
            }

            public void RemoveError(object key)
            {
                if (_errors.ContainsKey(key))
                    _errors.Remove(key);
            }

            public IEnumerable<string> GetErrors()
            {
                return _errors.Select(e => e.Value).ToList();
            }
        }

        #endregion
    }
}