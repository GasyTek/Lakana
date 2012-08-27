using System.Linq;
using System.Windows.Input;
using GasyTek.Lakana.Common.Extensions;
using GasyTek.Lakana.Mvvm.Validation;

namespace GasyTek.Lakana.Mvvm.ViewModels
{
    /// <summary>
    /// Base class for view models that supports edition.
    /// You must assign a model to this class, in order to initialize it correctly.
    /// </summary>
    /// <typeparam name="TModel">The type of the model that will be the data source for this view model</typeparam>
    public abstract class EditableViewModelBase<TModel> : ViewModelBase where TModel : class, new() 
    {
        #region Fields

        // Datas
        private TModel _model;

        // Commands
        private ICommand _saveCommand;
        private ICommand _cancelCommand;

        #endregion

        #region Properties

        public bool HasChanged
        {
            get { return RegisteredProperties.Any(o => o.HasChanged); }
        }

        public bool IsValid
        {
            get { return RegisteredProperties.All(o => o.IsValid); }
        }

        public ICommand SaveCommand
        {
            get { return _saveCommand; }
        }

        public ICommand CancelCommand
        {
            get { return _cancelCommand; }
        }

        /// <summary>
        /// Gets or sets the data source that will be edited within this view model
        /// </summary>
        public TModel Model
        {
            get { return _model; }
            set 
            { 
                this.SetPropertyValueAndNotify(ref _model, value, o => o.Model);
                InitializeAll();
            }
        }

        #endregion

        #region Constructor

        protected EditableViewModelBase()
            : base (false)
        {
            
        }

        #endregion

        #region Command handlers

        private bool OnSaveCommandCanExecute(object param)
        {
            return HasChanged && IsValid;
        }

        private void OnSaveCommandExecute(object param)
        {
            if (HasChanged && IsValid) OnSave();
        }

        private void OnCancelCommandExecute(object param)
        {
            OnCancel();
        }

        #endregion

        protected override void OnCreateCommands()
        {
            _saveCommand = CreateCommand(OnSaveCommandExecute, OnSaveCommandCanExecute);
            _cancelCommand = CreateCommand(OnCancelCommandExecute);
        }

        protected sealed override IValidationEngine CreateValidationEngine()
        {
            return OnCreateValidationEngine();
        }

        #region Abstract methods

        protected abstract IValidationEngine OnCreateValidationEngine();

        /// <summary>
        /// Saves the current edition..
        /// </summary>
        protected abstract void OnSave();

        /// <summary>
        /// Cancel the current edition..
        /// </summary>
        protected abstract void OnCancel();

        #endregion
    }
}
