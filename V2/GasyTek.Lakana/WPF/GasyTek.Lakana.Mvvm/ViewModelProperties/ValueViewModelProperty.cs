using System;
using System.Diagnostics;
using GasyTek.Lakana.Common.Extensions;

namespace GasyTek.Lakana.Mvvm.ViewModelProperties
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    [DebuggerDisplay("Name = {PropertyMetadata.Name}, CurrentValue = {Value}")]
    public class ValueViewModelProperty<TValue> : ViewModelProperty, IValueViewModelProperty<TValue>
    {
        #region Fields

        private TValue _value;
        private TValue _originalValue;

        #endregion

        #region Properties

        /// <summary>
        /// The strong typed value.
        /// </summary>
        public TValue Value
        {
            get { return _value; }
            set
            {
                var oldValue = _value;
                var newValue = value;
                this.SetPropertyValueAndNotify(ref _value, value, o => o.Value, o => o.HasChanged);
                OnValidatePropertyValueAsync(oldValue, newValue);
            }
        }

        /// <summary>
        /// The strong typed original value.
        /// </summary>
        public TValue OriginalValue
        {
            get { return _originalValue; }
        }

        #endregion

        #region Constructor

        internal ValueViewModelProperty(TValue originalValue, ObservableValidationEngine internalObservableValidationEngine)
            : base(internalObservableValidationEngine)
        {
            AssignOriginalValue(originalValue);
        }

        #endregion

        protected void AssignOriginalValue(TValue originalValue)
        {
            _originalValue = originalValue;
            this.SetPropertyValueAndNotify(ref _value, originalValue, o => o.Value);
        }

        #region Overriden methods

        protected override void OnNotifyValueProperty()
        {
            this.NotifyPropertyChanged(p => p.Value);
        }

        protected override object OnGetValue()
        {
            return Value;
        }

        protected override Type OnGetValueType()
        {
            return typeof (TValue);
        }

        protected override bool OnAskHasChanged()
        {
            // strings are special case. String.Empty and null are same
            if (typeof(TValue) == typeof(String))
            {
                if (String.IsNullOrEmpty((string)(object)OriginalValue) 
                    && String.IsNullOrEmpty((string)(object)Value))
                    return false;
            }

            return !Equals(OriginalValue, Value);
        }

        #endregion
    }
}
