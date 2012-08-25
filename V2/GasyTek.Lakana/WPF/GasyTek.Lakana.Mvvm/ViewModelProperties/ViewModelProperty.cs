using System;
using System.Reflection;
using GasyTek.Lakana.Common.Base;
using GasyTek.Lakana.Common.Extensions;
using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Common.Utils;
using GasyTek.Lakana.Mvvm.Validation;

namespace GasyTek.Lakana.Mvvm.ViewModelProperties
{
    /// <summary>
    /// Default implementation for <seealso cref="IViewModelProperty"/>. It is the base class for view model rich properties.
    /// </summary>
    public abstract class ViewModelProperty : NotifyPropertyChangedBase, IViewModelProperty
    {
        #region Fields

        private IUIMetadata _uiMetadata;
        private readonly ObservableValidationEngine _internalObservableValidationEngine;
        
        #endregion

        #region Properties

        /// <summary>
        /// Shortcut for label property of presentation metadata.
        /// </summary>
        public string Label
        {
            get { return (UIMetadata != null) ? UIMetadata.Label : GlobalConstants.LocalizationNoText; }
        }

        public IUIMetadata UIMetadata
        {
            get { return _uiMetadata; }
            set { this.SetPropertyValueAndNotify(ref _uiMetadata, value, o => o.UIMetadata); }
        }

        public PropertyInfo PropertyMetadata
        {
            get { return ((IHasPropertyMetadata)this).InternalPropertyMetadata; }
        }

        public bool HasChanged
        {
            get { return OnAskHasChanged(); }
        }

        public bool IsValid
        {
            get
            {
                var veng = (IHasValidationEngine) this;
                var ipmeta = (IHasPropertyMetadata) this;
                if (veng.InternalObservableValidationEngine.Object != null && (ipmeta.InternalPropertyMetadata != null))
                    return veng.InternalObservableValidationEngine.Object.IsValid(ipmeta.InternalPropertyMetadata.Name);
                return true;
            }
        }

        #endregion

        #region Constructor

        protected ViewModelProperty(ObservableValidationEngine internalObservableValidationEngine)
        {
            _internalObservableValidationEngine = internalObservableValidationEngine;
            InitializeObservableValidationEngine();
        }

        private void InitializeObservableValidationEngine()
        {
            if (_internalObservableValidationEngine.Object != null)
            {
                _internalObservableValidationEngine.Object.
                        ErrorsChangedEvent +=
                        OnValidationEngineErrorsChanged;
            }
            _internalObservableValidationEngine.ObjectChanged += (sender, args) =>
            {
                if (args.OldValue != null)
                {
                    args.OldValue.ErrorsChangedEvent -=
                        OnValidationEngineErrorsChanged;
                }

                if (args.NewValue != null)
                {
                    args.NewValue.
                        ErrorsChangedEvent +=
                        OnValidationEngineErrorsChanged;
                }
            };
        }

        #endregion

        #region IHasPropertyMetadata members

        PropertyInfo IHasPropertyMetadata.InternalPropertyMetadata { get; set; }

        #endregion

        #region IHasValidationEngine members

        ObservableValidationEngine IHasValidationEngine.InternalObservableValidationEngine
        {
            get { return _internalObservableValidationEngine; }
        }

        #endregion

        #region IHasValue members

        public object GetValue()
        {
            return OnGetValue();
        }

        public Type GetValueType()
        {
            return OnGetValueType();
        }

        #endregion

        #region IDataErrorInfo Membres

        public string Error
        {
            get { return String.Empty; }
        }

        public string this[string columnName]
        {
            get
            {
                var veng = (IHasValidationEngine) this;
                var inpmeta = (IHasPropertyMetadata) this;
                if (veng.InternalObservableValidationEngine.Object != null && inpmeta.InternalPropertyMetadata != null)
                {
                    var errors = veng.InternalObservableValidationEngine.Object.GetErrors(inpmeta.InternalPropertyMetadata.Name);
                    return String.Join("\r\n", errors);
                }
                return string.Empty;
            }
        }

        #endregion

        void OnValidationEngineErrorsChanged(object sender, ErrorsChangedEventArgs e)
        {
            var ipmeta = this as IHasPropertyMetadata;
            if (ipmeta.InternalPropertyMetadata != null && (e.PropertyName == ipmeta.InternalPropertyMetadata.Name))
            {
                // if the errors concerns this property then refresh eventual bindings
                OnNotifyValueProperty();
            }
        }
        
        protected void OnValidatePropertyValueAsync(object oldValue, object newValue)
        {
            // TODO : consider string.empty == null
            if(Equals(oldValue, newValue)) return;

            // process an async validation
            var veng = (IHasValidationEngine)this;
            var inpmeta = (IHasPropertyMetadata)this;
            if (veng.InternalObservableValidationEngine.Object != null && inpmeta.InternalPropertyMetadata != null)
                veng.InternalObservableValidationEngine.Object.ValidateAsync(inpmeta.InternalPropertyMetadata, newValue);
        }

        #region Overridable methods

        /// <summary>
        /// Raise INotifyPropertyChanged.PropertyChanged on the value property of this view model property.
        /// </summary>
        protected abstract void OnNotifyValueProperty();

        /// <summary>
        /// Generic method that is called to get the actual value of the property.
        /// </summary>
        /// <returns></returns>
        protected abstract object OnGetValue();

        /// <summary>
        /// Generic method that is called to get the actual type of the property value.
        /// </summary>
        /// <returns></returns>
        protected abstract Type OnGetValueType();

        /// <summary>
        /// Called when the system checks for changes.
        /// </summary>
        /// <returns></returns>
        protected abstract bool OnAskHasChanged();

        #endregion
    }
}
