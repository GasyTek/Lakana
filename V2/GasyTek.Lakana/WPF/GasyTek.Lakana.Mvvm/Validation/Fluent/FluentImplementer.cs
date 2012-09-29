using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GasyTek.Lakana.Mvvm.ViewModelProperties;
using GasyTek.Lakana.Mvvm.ViewModels;

namespace GasyTek.Lakana.Mvvm.Validation.Fluent
{
    internal interface IFluentImplementer<out TViewModel> 
        where TViewModel : ViewModelBase
    {
        List<ExpressionNode> InternalTokens { get; }
        FluentImplementerContext Context { get; }
        TViewModel ViewModel { get; }

        bool IsExpressionValid();
    }

    /// <summary>
    /// The object that is used to build fluent sentences.
    /// It implements all fluent interfaces.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <typeparam name="TPropertyValue">The type of the property value.</typeparam>
    internal sealed class FluentImplementer<TViewModel, TPropertyValue>
        : IFluentImplementer<TViewModel>
        , IFluentProperty<TViewModel, TPropertyValue>
        , IFluentVerb<TViewModel, TPropertyValue>
        , IFluentOperator<TViewModel, TPropertyValue>
        , IFluentContinuation<TViewModel, TPropertyValue> where TViewModel : ViewModelBase
    {
        private readonly List<ExpressionNode> _internalTokens;
        private readonly TViewModel _viewModel;
        private readonly FluentImplementerContext _context;

        #region Properties

        /// <summary>
        /// Used by the infrastructure only.
        /// </summary>
        public List<ExpressionNode> InternalTokens
        {
            get { return _internalTokens; }
        }

        public FluentImplementerContext Context
        {
            get { return _context; }
        }

        public TViewModel ViewModel
        {
            get { return _viewModel; }
        }

        #endregion

        public FluentImplementer(TViewModel viewModel)
        {
            _internalTokens = new List<ExpressionNode>();
            _context = new FluentImplementerContext();
            _viewModel = viewModel;
        }

        /// <summary>
        /// Determines whether the expression defined by this fluent api is valid or not.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if expression is valid] otherwise, <c>false</c>.
        /// </returns>
        public bool IsExpressionValid()
        {
            if (_internalTokens.Count > 0)
            {
                var lastToken = _internalTokens.Last();
                if (lastToken is CloseParenthesis) return true;
                if (lastToken is EvaluableExpression) return true;
            }
            return false;
        }

        #region Fluent api property

        /// <summary>
        /// Properties the specified property expression.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns></returns>
        public IFluentVerb<TViewModel, TPropertyValue> Property(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
        {
            UpdateContext(propertyExpression, true, true);
            return this;
        }

        #endregion

        #region Fluent api verbs

        public IFluentOperator<TViewModel, TPropertyValue> Is
        {
            get { return this; }
        }

        public IFluentOperator<TViewModel, TPropertyValue> IsNot
        {
            get
            {
                AddToken(ExpressionNode.Not());
                return this;
            }
        }

        public IFluentOperator<TViewModel, TPropertyValue> Has
        {
            get { return this; }
        }

        public IFluentOperator<TViewModel, TPropertyValue> HasNot
        {
            get
            {
                AddToken(ExpressionNode.Not());
                return this;
            }
        }

        #endregion

        #region Fluent api continuation

        public void Otherwise(string message)
        {
            _context.Message = message;
        }

        public IFluentVerb<TViewModel, TPropertyValue> And
        {
            get
            {
                AddToken(ExpressionNode.And());
                return this;
            }
        }

        public IFluentVerb<TViewModel, TPropertyValue> Or
        {
            get
            {
                AddToken(ExpressionNode.Or());
                return this;
            }
        }

        #endregion

        #region Internal methods

        internal void AddToken(ExpressionNode token)
        {
            _internalTokens.Add(token) ;
        }

        internal void UpdateContext(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression
            , bool isCurrentProperty = false
            , bool isInitialProperty = false)
        {
            var viewModelProperty = propertyExpression.Compile()(_viewModel);
            _context.Properties.Add(viewModelProperty);
            if(isCurrentProperty) _context.CurrentProperty = viewModelProperty;
            if(isInitialProperty) _context.OwnerProperty = viewModelProperty;
        }

        internal void EnsureContextCurrentPropertyIsNotNull()
        {
            if (_context.CurrentProperty == null)
                throw new InvalidOperationException("The current property must not be null.");
        }

        #endregion
    }
}
