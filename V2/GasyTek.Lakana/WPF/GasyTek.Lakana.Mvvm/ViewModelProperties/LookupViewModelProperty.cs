using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using GasyTek.Lakana.Common.Extensions;

namespace GasyTek.Lakana.Mvvm.ViewModelProperties
{
    [DebuggerDisplay("Name = {PropertyMetadata.Name}, CurrentValue = {SelectedValue}")]
    public class LookupViewModelProperty<TValue, TItem> : ViewModelProperty, ILookupViewModelProperty<TValue, TItem>
    {
        #region Fields

        private TValue _selectedValue;
        private TValue _originalValue;
        private ObservableCollection<TItem> _itemsSource;
        private Func<IEnumerable<TItem>> _fillItemsSource;

        #endregion

        #region Properties

        public TValue SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                var oldValue = _selectedValue;
                var newValue = value;
                this.SetPropertyValueAndNotify(ref _selectedValue, value, o => o.SelectedValue, o => o.HasChanged);
                OnValidatePropertyValueAsync(oldValue, newValue);
            }
        }

        public TValue OriginalValue
        {
            get { return _originalValue; }
        }

        public ObservableCollection<TItem> ItemsSource
        {
            get
            {
                if (_itemsSource == null)
                {
                    var handler = _fillItemsSource;
                    _itemsSource = handler != null ? new ObservableCollection<TItem>(handler()) : new ObservableCollection<TItem>();
                }
                return _itemsSource;
            }
        }

        #endregion

        #region Constructor

        internal LookupViewModelProperty(TValue originalValue, Func<IEnumerable<TItem>> fillItemsSource, ObservableValidationEngine internalObservableValidationEngine)
            : base(internalObservableValidationEngine)
        {
            AssignFillItemsSource(fillItemsSource);
            AssignOriginalValue(originalValue);
        }

        #endregion

        protected void AssignFillItemsSource(Func<IEnumerable<TItem>> fillItemsSource)
        {
            _fillItemsSource = fillItemsSource;
        }

        protected void AssignOriginalValue(TValue originalValue)
        {
            _originalValue = originalValue;
            this.SetPropertyValueAndNotify(ref _selectedValue, originalValue, o => o.SelectedValue);
        }

        #region Overriden methods

        protected override void OnNotifyValueProperty()
        {
            this.NotifyPropertyChanged(p => p.SelectedValue);
        }

        protected override object OnGetValue()
        {
            return SelectedValue;
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
                    && String.IsNullOrEmpty((string)(object)SelectedValue))
                    return false;
            }

            return !Equals(OriginalValue, SelectedValue);
        }

        #endregion
    }
}
