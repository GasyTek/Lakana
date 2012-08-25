using System.Linq;
using System.Windows.Input;
using GasyTek.Lakana.Common.Extensions;

namespace GasyTek.Lakana.Mvvm.ViewModels
{
    /// <summary>
    /// Base class for view models that supports edition.
    /// </summary>
    /// <typeparam name="TModel">The type of the model that will be the data source for this view model</typeparam>
    /// <typeparam name="TViewModel">The type of the view model that inherits from this view model </typeparam>
    public abstract class EditViewModelBase<TViewModel, TModel> : ViewModelBase
        where TViewModel : ViewModelBase
        where TModel : class, new() 
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
                this.SetPropertyValue(ref _model, value, o => o.Model);
                CreateAll();
            }
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

        //#region Validation rule management

        ///// <summary>
        ///// Initial method to use when defining the rules for a property.
        ///// </summary>
        ///// <param name="propertyExpression">The property expression.(ex : vm => vm.MyProperty ) </param>
        ///// <returns></returns>
        //protected IPropertyDefitionPart ForProperty(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression) 
        //{
        //    var viewModelProperty = ExtractViewModelPropertyInfo(propertyExpression).PropertyInstance;
        //    return new PropertyDefinitionPart(viewModelProperty);
        //}

        ///// <summary>
        ///// Initial method to use when defining the rules for many related properties. 
        ///// Those rules are global rules for the object, that is they concern many related properties.
        ///// </summary>
        ///// <typeparam name="TViewModel">The type of the view model.</typeparam>
        ///// <param name="propertyExpression">The default property expression. eg : vm => vm.MyProperty</param>
        ///// <param name="propertyExpressions">Other properties on which to attach the rule.</param>
        ///// <returns></returns>
        //protected IPropertyGroupDefinitionPart<TViewModel> ForProperties(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression,
        //            params Expression<Func<TViewModel, IViewModelProperty>> [] propertyExpressions)
        //{
        //    IEnumerable<Expression<Func<TViewModel, IViewModelProperty>>> allPropertyExpressions 
        //        = new List<Expression<Func<TViewModel, IViewModelProperty>>> { propertyExpression };
        //    allPropertyExpressions = allPropertyExpressions.Concat(propertyExpressions);
            
        //    var ownerViewModelProperties = allPropertyExpressions.Select(ExtractViewModelPropertyInfo);
        //    return new PropertyGroupDefinitionPart<TViewModel>((TViewModel)(object)this, ownerViewModelProperties);
        //}

        //private ViewModelPropertyInfo ExtractViewModelPropertyInfo(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
        //{
        //    if (propertyExpression == null)
        //        throw new ArgumentNullException("propertyExpression", "propertyExpression is null.");

        //    var memberExpr = propertyExpression.Body as MemberExpression;
        //    if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
        //    {
        //        var propertyName = memberExpr.Member.Name;
        //        if (string.IsNullOrEmpty(propertyName))
        //            throw new InvalidOperationException("The property name was null");
        //        var propertyInfo = GetType().GetProperty(propertyName);
        //        if (propertyInfo == null)
        //            throw new InvalidOperationException(string.Format("The property with name '{0}' was not found on '{1}'", propertyName, GetType()));
        //        var viewModelProperty = propertyInfo.GetValue(this, null) as IViewModelProperty;
        //        if (viewModelProperty == null)
        //            throw new InvalidOperationException("Only property of type IViewModelProperty are allowed");

        //        return new ViewModelPropertyInfo { PropertyName = propertyName, PropertyInstance = viewModelProperty};
        //    }

        //    throw new ArgumentException("No property reference expression was found.", "propertyExpression");
        //}

        //#endregion

        protected sealed override void OnCreateCommands()
        {
            _saveCommand = CreateCommand(OnSaveCommandExecute, OnSaveCommandCanExecute);
            _cancelCommand = CreateCommand(OnCancelCommandExecute);
        }

        #region Abstract methods

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
