﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GasyTek.Lakana.Common.Base;
using GasyTek.Lakana.Common.Extensions;
using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Mvvm.Commands;
using GasyTek.Lakana.Mvvm.Validation;
using GasyTek.Lakana.Mvvm.ViewModelProperties;

namespace GasyTek.Lakana.Mvvm.ViewModels
{
    /// <summary>
    /// Base class for all view-models
    /// </summary>
    public abstract class ViewModelBase : NotifyPropertyChangedBase, IViewModel
    {
        #region Fields

        private IUIMetadata _uiMetadata;
        private readonly List<IViewModelProperty> _registeredProperties;
        private readonly List<ISimpleCommand> _registeredCommands;
        private readonly ObservableValidationEngine _observableValidationEngine;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the UI metadata, meaning localized text, icons etc.
        /// </summary>
        /// <value>
        /// The UI metadata.
        /// </value>
        public IUIMetadata UIMetadata
        {
            get { return _uiMetadata; }
            protected set
            {
                this.SetPropertyValueAndNotify(ref _uiMetadata, value, o => o.UIMetadata);
            }
        }

        private IValidationEngine ValidationEngine
        {
            set { _observableValidationEngine.Object = value; }
        }

        /// <summary>
        /// Gets the registered properties. Registered properties participates in automatically refreshing the <seealso cref="RegisteredCommands"/>
        /// </summary>
        public ReadOnlyCollection<IViewModelProperty> RegisteredProperties
        {
            get { return _registeredProperties.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the registered commands. Registered commands will be automatically refreshed when changes appear in <seealso cref="RegisteredProperties"/>
        /// </summary>
        public ReadOnlyCollection<ISimpleCommand> RegisteredCommands
        {
            get { return _registeredCommands.AsReadOnly(); }
        }

        #endregion

        #region Constructor

        protected ViewModelBase(bool createPropertiesAndCommandsImmediately)
        {
            _registeredProperties = new List<IViewModelProperty>();
            _registeredCommands = new List<ISimpleCommand>();
            _observableValidationEngine = new ObservableValidationEngine();

            UIMetadata = new UIMetadata { LabelProvider = () => "???" };

            if (createPropertiesAndCommandsImmediately) 
                InitializeAll();
        }

        protected ViewModelBase()
            : this (true)
        {
        }

        #endregion

        #region Register / Unregister Properties and Commands

        /// <summary>
        /// Register the view model property so that it can participate in refreshing command procedure.
        /// </summary>
        private void RegisterViewModelProperty(IViewModelProperty property)
        {
            _registeredProperties.Add(property);
        }

        private void RegisterViewModelCommand(ISimpleCommand command)
        {
            _registeredCommands.Add(command);
        }

        #endregion

        #region Create Properties

        protected void ClearViewmModelProperties()
        {
            DetachEventsFromProperty();
            _registeredProperties.Clear();
        }

        /// <summary>
        /// Creates the property.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="originalValue">The original value.</param>
        /// <returns></returns>
        protected IValueViewModelProperty<TValue> CreateValueProperty<TValue>(TValue originalValue)
        {
            // initialize the property
            var property = new ValueViewModelProperty<TValue>(originalValue, _observableValidationEngine);
            RegisterViewModelProperty(property);
            return property;
        }

        /// <summary>
        /// Creates the lookup property.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="originalValue">The original value.</param>
        /// <param name="fillItemsSourceAction">The action that fill the lookup.</param>
        /// <returns></returns>
        protected ILookupViewModelProperty<TValue, TItem> CreateLookupProperty<TValue, TItem>(TValue originalValue, Func<IEnumerable<TItem>> fillItemsSourceAction)
        {
            var property = new LookupViewModelProperty<TValue, TItem>(originalValue, fillItemsSourceAction, _observableValidationEngine);
            RegisterViewModelProperty(property);
            return property;
        }

        /// <summary>
        /// Creates the enum property.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="originalValue">The original value.</param>
        /// <param name="enumUIMetadataProvider">A custom provider that returns the ui metadata that corresponds to given value.</param>
        /// <returns></returns>
        protected IEnumViewModelProperty<TEnum> CreateEnumProperty<TEnum>(TEnum originalValue, Func<TEnum, IUIMetadata> enumUIMetadataProvider) where TEnum : struct
        {
            var property = new EnumViewModelProperty<TEnum>(originalValue, enumUIMetadataProvider, _observableValidationEngine);
            RegisterViewModelProperty(property);
            return property;
        }

        /// <summary>
        /// Creates the enum property.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="originalValue">The original value.</param>
        /// <returns></returns>
        protected IEnumViewModelProperty<TEnum> CreateEnumProperty<TEnum>(TEnum originalValue) where TEnum : struct
        {
            return CreateEnumProperty(originalValue, null);
        }

        #endregion

        #region Create Commands

        protected void ClearCommands()
        {
            _registeredCommands.Clear();
        }

        /// <summary>
        /// Creates a command that is owned by the view model and that will be refreshed every time a view model property changes.
        /// </summary>
        /// <typeparam name="TParam">The type of the param.</typeparam>
        /// <param name="executeAction">The execute action.</param>
        /// <param name="canExecuteAction">The can execute action.</param>
        /// <returns></returns>
        protected virtual ICommand CreateCommand<TParam>(Action<TParam> executeAction, Predicate<TParam> canExecuteAction)
        {
            var simpleCommand = new SimpleCommand<TParam>(executeAction, canExecuteAction);
            RegisterViewModelCommand(simpleCommand);
            return simpleCommand;
        }

        protected ICommand CreateCommand<TParam>(Action<TParam> executeAction)
        {
            return CreateCommand(executeAction, null);
        }

        protected ICommand CreateCommand(Action<object> executeAction, Predicate<object> canExecuteAction)
        {
            return CreateCommand<object>(executeAction, canExecuteAction);
        }

        protected ICommand CreateCommand(Action<object> executeAction)
        {
            return CreateCommand(executeAction, null);
        }

        #endregion

        /// <summary>
        /// Initialize properties, commands and all infrastructure wiring.
        /// </summary>
        protected void InitializeAll()
        {
            // build properties
            ClearViewmModelProperties();
            OnCreateViewModelProperties();
            CreateViewModelPropertyMetadatas();
            AttachEventsToViewModelProperty();

            // build commands
            ClearCommands();
            OnCreateViewModelCommands();
            OnRefreshCmmands();

            // initialize validation engine
            ValidationEngine = CreateValidationEngine();
        }

        protected void OnRefreshCmmands()
        {
            RegisteredCommands.ToList().ForEach(c => c.RaiseCanExecuteChanged());
        }
        
        private void CreateViewModelPropertyMetadatas()
        {
            // initializes metadatas for properties
            foreach (var registeredProperty in RegisteredProperties)
            {
                var memberInfos = GetType().
                                    FindMembers(MemberTypes.Property
                                          , BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance
                                          , (mi, obj) => ReferenceEquals(((PropertyInfo) mi).GetValue(this, null), obj)
                                          , registeredProperty);
                if(memberInfos.Any())
                {
                    registeredProperty.InternalPropertyMetadata = (PropertyInfo)memberInfos[0];
                }
            }
        }

        private void AttachEventsToViewModelProperty()
        {
            RegisteredProperties.ToList().ForEach(p => p.PropertyChanged += OnPropertyChanged);
        }

        private void DetachEventsFromProperty()
        {
            RegisteredProperties.ToList().ForEach(p => p.PropertyChanged -= OnPropertyChanged);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnRefreshCmmands();
        }

        #region Overridable methods

        /// <summary>
        /// Called when building properties for the view model..
        /// </summary>
        protected abstract void OnCreateViewModelProperties();

        /// <summary>
        /// Called when building commands for the view model..
        /// </summary>
        protected virtual void OnCreateViewModelCommands()
        {
            
        }

        protected virtual IValidationEngine CreateValidationEngine()
        {
            return null;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            OnDispose();
        }

        /// <summary>
        /// Child classes can override this method to perform 
        /// clean-up logic, such as removing event handlers.
        /// </summary>
        protected virtual void OnDispose()
        {
            DetachEventsFromProperty();
        }

        #endregion
    }
}
