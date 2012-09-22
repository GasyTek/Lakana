using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GasyTek.Lakana.Mvvm.ViewModelProperties;
using GasyTek.Lakana.Mvvm.ViewModels;

namespace GasyTek.Lakana.Mvvm.Validation.Fluent
{
    /// <summary>
    /// The object that is used to build fluent sentences.
    /// It implements all fluent interfaces.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    internal sealed class FluentImplementer<TViewModel> 
        : IFluentProperty<TViewModel>, IFluentVerb<TViewModel>, IFluentEvaluable<TViewModel>, IFluentOtherwise<TViewModel> where TViewModel : ViewModelBase
    {
        private readonly List<ExpressionNode> _internalTokens;
        private readonly TViewModel _viewModel;
        private readonly FluentImplementerContext _context;

        /// <summary>
        /// Used by the infrastructure only.
        /// </summary>
        internal List<ExpressionNode> InternalTokens
        {
            get { return _internalTokens; }
        }

        internal FluentImplementerContext Context
        {
            get { return _context; }
        }

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

        public IFluentVerb<TViewModel> Property(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
        {
            UpdateContext(propertyExpression, true, true);
            return this;
        }

        #region Fluent api verbs

        public IFluentEvaluable<TViewModel> Is
        {
            get { return this; }
        }

        public IFluentEvaluable<TViewModel> IsNot
        {
            get
            {
                AddToken(ExpressionNode.Not());
                return this;
            }
        }

        public IFluentEvaluable<TViewModel> Has
        {
            get { return this; }
        }

        public IFluentEvaluable<TViewModel> HasNot
        {
            get
            {
                AddToken(ExpressionNode.Not());
                return this;
            }
        }

        #endregion

        #region Fluent api evaluables

        public IFluentOtherwise<TViewModel> Satisfying(CustomValidator customValidator)
        {
            EnsureContextCurrentPropertyIsNotNull();

            AddToken(ExpressionNode.CustomValidation(_context.CurrentProperty, customValidator));
            return this;
        }

        public IFluentOtherwise<TViewModel> GreaterThan(object value)
        {
            EnsureContextCurrentPropertyIsNotNull();
            EnsureContextCurrentPropertyValueIsOfType(typeof(IComparable), "GreaterThan");

            AddToken(ExpressionNode.GreaterThanValue(_context.CurrentProperty, value));
            return this;
        }

        public IFluentOtherwise<TViewModel> GreaterThan(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
        {
            EnsureContextCurrentPropertyIsNotNull();
            EnsureContextCurrentPropertyValueIsOfType(typeof(IComparable), "GreaterThan");

            UpdateContext(propertyExpression);

            AddToken(ExpressionNode.GreaterThanProperty(_context.CurrentProperty, propertyExpression.Compile()(_viewModel)));
            return this;
        }

        public IFluentOtherwise<TViewModel> GreaterThan(LateValue lateValue)
        {
            EnsureContextCurrentPropertyIsNotNull();

            AddToken(ExpressionNode.GreaterThanLateValue(_context.CurrentProperty, lateValue));
            return this;
        }

        public IFluentOtherwise<TViewModel> GreaterThanOrEqualTo(object value)
        {
            EnsureContextCurrentPropertyIsNotNull();
            EnsureContextCurrentPropertyValueIsOfType(typeof(IComparable), "GreaterThanOrEqualn");

            AddToken(ExpressionNode.OpenParenthesis());
            AddToken(ExpressionNode.GreaterThanValue(_context.CurrentProperty, value));
            AddToken(ExpressionNode.Or());
            AddToken(ExpressionNode.EqualToValue(_context.CurrentProperty, value));
            AddToken(ExpressionNode.CloseParenthesis());
            return this;
        }

        public IFluentOtherwise<TViewModel> GreaterThanOrEqualTo(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
        {
            EnsureContextCurrentPropertyIsNotNull();
            EnsureContextCurrentPropertyValueIsOfType(typeof(IComparable), "GreaterThanOrEqualTo");

            UpdateContext(propertyExpression);

            AddToken(ExpressionNode.OpenParenthesis());
            AddToken(ExpressionNode.GreaterThanProperty(_context.CurrentProperty, propertyExpression.Compile()(_viewModel)));
            AddToken(ExpressionNode.Or());
            AddToken(ExpressionNode.EqualToProperty(_context.CurrentProperty, propertyExpression.Compile()(_viewModel)));
            AddToken(ExpressionNode.CloseParenthesis());
            return this;
        }

        public IFluentOtherwise<TViewModel> GreaterThanOrEqualTo(LateValue lateValue)
        {
            EnsureContextCurrentPropertyIsNotNull();

            AddToken(ExpressionNode.OpenParenthesis());
            AddToken(ExpressionNode.GreaterThanLateValue(_context.CurrentProperty, lateValue));
            AddToken(ExpressionNode.Or());
            AddToken(ExpressionNode.EqualToLateValue(_context.CurrentProperty, lateValue));
            AddToken(ExpressionNode.CloseParenthesis());
            return this;
        }

        public IFluentOtherwise<TViewModel> LessThan(object value)
        {
            EnsureContextCurrentPropertyIsNotNull();
            EnsureContextCurrentPropertyValueIsOfType(typeof(IComparable), "LessThan");

            AddToken(ExpressionNode.LessThanValue(_context.CurrentProperty, value));
            return this;
        }

        public IFluentOtherwise<TViewModel> LessThan(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
        {
            EnsureContextCurrentPropertyIsNotNull();
            EnsureContextCurrentPropertyValueIsOfType(typeof(IComparable), "LessThan");

            UpdateContext(propertyExpression);

            AddToken(ExpressionNode.LessThanProperty(_context.CurrentProperty, propertyExpression.Compile()(_viewModel)));
            return this;
        }

        public IFluentOtherwise<TViewModel> LessThan(LateValue lateValue)
        {
            EnsureContextCurrentPropertyIsNotNull();

            AddToken(ExpressionNode.LessThanLateValue(_context.CurrentProperty, lateValue));
            return this;
        }

        public IFluentOtherwise<TViewModel> LessThanOrEqualTo(object value)
        {
            EnsureContextCurrentPropertyIsNotNull();
            EnsureContextCurrentPropertyValueIsOfType(typeof(IComparable), "LessThanOrEqualTo");

            AddToken(ExpressionNode.OpenParenthesis());
            AddToken(ExpressionNode.LessThanValue(_context.CurrentProperty, value));
            AddToken(ExpressionNode.Or());
            AddToken(ExpressionNode.EqualToValue(_context.CurrentProperty, value));
            AddToken(ExpressionNode.CloseParenthesis());
            return this;
        }

        public IFluentOtherwise<TViewModel> LessThanOrEqualTo(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
        {
            EnsureContextCurrentPropertyIsNotNull();
            EnsureContextCurrentPropertyValueIsOfType(typeof(IComparable), "LessThanOrEqualTo");

            UpdateContext(propertyExpression);

            AddToken(ExpressionNode.OpenParenthesis());
            AddToken(ExpressionNode.LessThanProperty(_context.CurrentProperty, propertyExpression.Compile()(_viewModel)));
            AddToken(ExpressionNode.Or());
            AddToken(ExpressionNode.EqualToProperty(_context.CurrentProperty, propertyExpression.Compile()(_viewModel)));
            AddToken(ExpressionNode.CloseParenthesis());
            return this;
        }

        public IFluentOtherwise<TViewModel> LessThanOrEqualTo(LateValue lateValue)
        {
            EnsureContextCurrentPropertyIsNotNull();

            AddToken(ExpressionNode.OpenParenthesis());
            AddToken(ExpressionNode.LessThanLateValue(_context.CurrentProperty, lateValue));
            AddToken(ExpressionNode.Or());
            AddToken(ExpressionNode.EqualToLateValue(_context.CurrentProperty, lateValue));
            AddToken(ExpressionNode.CloseParenthesis());
            return this;
        }

        public IFluentOtherwise<TViewModel> EqualTo(object value)
        {
            EnsureContextCurrentPropertyIsNotNull();

            AddToken(ExpressionNode.EqualToValue(_context.CurrentProperty, value));
            return this;
        }

        public IFluentOtherwise<TViewModel> EqualTo(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
        {
            EnsureContextCurrentPropertyIsNotNull();

            UpdateContext(propertyExpression);

            AddToken(ExpressionNode.EqualToProperty(_context.CurrentProperty, propertyExpression.Compile()(_viewModel)));
            return this;
        }

        public IFluentOtherwise<TViewModel> EqualTo(LateValue lateValue)
        {
            EnsureContextCurrentPropertyIsNotNull();

            AddToken(ExpressionNode.EqualToLateValue(_context.CurrentProperty, lateValue));
            return this;
        }

        public IFluentOtherwise<TViewModel> Null()
        {
            EnsureContextCurrentPropertyIsNotNull();

            AddToken(ExpressionNode.EqualToValue(_context.CurrentProperty, null));
            return this;
        }

        public IFluentOtherwise<TViewModel> NullOrEmpty()
        {
            EnsureContextCurrentPropertyIsNotNull();

            AddToken(ExpressionNode.EqualToValue(_context.CurrentProperty, null));
            AddToken(ExpressionNode.Or());
            AddToken(ExpressionNode.EqualToValue(_context.CurrentProperty, string.Empty));
            return this;
        }

        /// <summary>
        /// Matches the specified regular expression pattern.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        public IFluentOtherwise<TViewModel> Matching(string pattern)
        {
            EnsureContextCurrentPropertyIsNotNull();
            EnsureContextCurrentPropertyValueIsOfType(typeof(string), "Matching");

            AddToken(ExpressionNode.MatchingProperty(_context.CurrentProperty, pattern));
            return this;
        }

        public IFluentOtherwise<TViewModel> ValidEmail()
        {
            return Matching(ValidationConstants.EmailPattern);
        }

        public IFluentOtherwise<TViewModel> StartingWith(string start)
        {
            return Matching(string.Format("^({0})", start));
        }

        public IFluentOtherwise<TViewModel> EndingWith(string end)
        {
            return Matching(string.Format("({0})$", end));
        }

        public IFluentOtherwise<TViewModel> MaxLength(int maxLength)
        {
            EnsureContextCurrentPropertyIsNotNull();
            EnsureContextCurrentPropertyValueIsOfType(typeof(string), "MaxLength");

            if (maxLength <= 0)
                throw new InvalidOperationException("maxLength must be greater than 0");

            var evaluatedValueProvider = new Func<object>(() =>
            {
                var value = (string)_context.CurrentProperty.GetValue();
                return value == null ? 0 : value.Length;
            });
            var valueProvider = new Func<object>(() => maxLength);

            AddToken(ExpressionNode.OpenParenthesis());
            AddToken(ExpressionNode.LessThanGeneric(evaluatedValueProvider, valueProvider));
            AddToken(ExpressionNode.Or());
            AddToken(ExpressionNode.EqualToGeneric(evaluatedValueProvider, valueProvider));
            AddToken(ExpressionNode.CloseParenthesis());

            return this;
        }

        public IFluentOtherwise<TViewModel> MinLength(int minLength)
        {
            EnsureContextCurrentPropertyIsNotNull();
            EnsureContextCurrentPropertyValueIsOfType(typeof(string), "MinLength");

            if (minLength <= 0)
                throw new InvalidOperationException("minLength must be greater than 0");

            var evaluatedValueProvider = new Func<object>(() =>
            {
                var value = (string)_context.CurrentProperty.GetValue();
                return value == null ? 0 : value.Length;
            });
            var valueProvider = new Func<object>(() => minLength);

            AddToken(ExpressionNode.OpenParenthesis());
            AddToken(ExpressionNode.GreaterThanGeneric(evaluatedValueProvider, valueProvider));
            AddToken(ExpressionNode.Or());
            AddToken(ExpressionNode.EqualToGeneric(evaluatedValueProvider, valueProvider));
            AddToken(ExpressionNode.CloseParenthesis());

            return this;
        }

        public IFluentOtherwise<TViewModel> Required()
        {
            EnsureContextCurrentPropertyIsNotNull();
            
            // required = is neither null nor empty
            AddToken(ExpressionNode.Not());
            AddToken(ExpressionNode.OpenParenthesis());
            AddToken(ExpressionNode.EqualToValue(_context.CurrentProperty, null));
            AddToken(ExpressionNode.Or());
            AddToken(ExpressionNode.EqualToValue(_context.CurrentProperty, string.Empty));
            AddToken(ExpressionNode.CloseParenthesis());
            return this;
        }

        public IFluentOtherwise<TViewModel> DifferentOf(object value)
        {
            EnsureContextCurrentPropertyIsNotNull();

            // different of = not equal to value
            AddToken(ExpressionNode.Not());
            AddToken(ExpressionNode.EqualToValue(_context.CurrentProperty, value));
            return this;
        }

        public IFluentOtherwise<TViewModel> DifferentOf(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
        {
            EnsureContextCurrentPropertyIsNotNull();

            UpdateContext(propertyExpression);

            // different of = not equal to property
            AddToken(ExpressionNode.Not());
            AddToken(ExpressionNode.EqualToProperty(_context.CurrentProperty, propertyExpression.Compile()(_viewModel)));
            return this;
        }

        public IFluentOtherwise<TViewModel> DifferentOf(LateValue lateValue)
        {
            EnsureContextCurrentPropertyIsNotNull();

            AddToken(ExpressionNode.Not());
            AddToken(ExpressionNode.EqualToLateValue(_context.CurrentProperty, lateValue));
            return this;
        }

        public IFluentOtherwise<TViewModel> Between(object @from, object to)
        {
            EnsureContextCurrentPropertyIsNotNull();

            // uses parenthesis to satisfy syntax : IsNot.Between (...)
            AddToken(ExpressionNode.OpenParenthesis());
            AddToken(ExpressionNode.OpenParenthesis());
            AddToken(ExpressionNode.GreaterThanValue(_context.CurrentProperty, @from));
            AddToken(ExpressionNode.Or());
            AddToken(ExpressionNode.EqualToValue(_context.CurrentProperty, @from));
            AddToken(ExpressionNode.CloseParenthesis());
            AddToken(ExpressionNode.And());
            AddToken(ExpressionNode.OpenParenthesis());
            AddToken(ExpressionNode.LessThanValue(_context.CurrentProperty, to));
            AddToken(ExpressionNode.Or());
            AddToken(ExpressionNode.EqualToValue(_context.CurrentProperty, to));
            AddToken(ExpressionNode.CloseParenthesis());
            AddToken(ExpressionNode.CloseParenthesis());

            return this;
        }

        public IFluentOtherwise<TViewModel> BetweenPoperties(Expression<Func<TViewModel, IViewModelProperty>> @from, Expression<Func<TViewModel, IViewModelProperty>> to)
        {
            EnsureContextCurrentPropertyIsNotNull();

            UpdateContext(@from);
            UpdateContext(to);

            // uses parenthesis to satisfy syntax : IsNot.Between (...)
            AddToken(ExpressionNode.OpenParenthesis());
            AddToken(ExpressionNode.OpenParenthesis());
            AddToken(ExpressionNode.GreaterThanProperty(_context.CurrentProperty, @from.Compile()(_viewModel)));
            AddToken(ExpressionNode.Or());
            AddToken(ExpressionNode.EqualToProperty(_context.CurrentProperty, @from.Compile()(_viewModel)));
            AddToken(ExpressionNode.CloseParenthesis());
            AddToken(ExpressionNode.And());
            AddToken(ExpressionNode.OpenParenthesis());
            AddToken(ExpressionNode.LessThanProperty(_context.CurrentProperty, to.Compile()(_viewModel)));
            AddToken(ExpressionNode.Or());
            AddToken(ExpressionNode.EqualToProperty(_context.CurrentProperty, to.Compile()(_viewModel)));
            AddToken(ExpressionNode.CloseParenthesis());
            AddToken(ExpressionNode.CloseParenthesis());

            return this;
        }

        #endregion

        #region Fluent api otherwise

        public IFluentVerb<TViewModel> And
        {
            get
            {
                AddToken(ExpressionNode.And());
                return this;
            }
        }

        public IFluentVerb<TViewModel> Or
        {
            get
            {
                AddToken(ExpressionNode.Or());
                return this;
            }
        }
        
        public IFluentVerb<TViewModel> When(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
        {
            // TODO : finish the implementation
            throw new NotImplementedException();
            //UpdateContext(propertyExpression, true);

            //// p1.Is.EqualTo(3).When(p2).Is.GreaterThan(5) means
            //// p1 == 3 && p2 > 5 .... ??
            //AddToken(new AndExpression());
            //return this;
        }

        public void Otherwise(string message)
        {
            _context.Message = message;
        }

        #endregion

        #region Private methods

        private void AddToken(ExpressionNode token)
        {
            _internalTokens.Add(token) ;
        }

        private void UpdateContext(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression
            , bool isCurrentProperty = false
            , bool isInitialProperty = false)
        {
            var viewModelProperty = propertyExpression.Compile()(_viewModel);
            _context.Properties.Add(viewModelProperty);
            if(isCurrentProperty) _context.CurrentProperty = viewModelProperty;
            if(isInitialProperty) _context.OwnerProperty = viewModelProperty;
        }

        private void EnsureContextCurrentPropertyIsNotNull()
        {
            if (_context.CurrentProperty == null)
                throw new InvalidOperationException("The current property must not be null.");
        }

        private void EnsureContextCurrentPropertyValueIsOfType(Type expectedType, string currentOperatorName)
        {
            if (expectedType == null) throw new ArgumentNullException("expectedType");

            var valueType = _context.CurrentProperty.GetValueType();
            if (expectedType.IsAssignableFrom(valueType) == false)
                throw new InvalidOperationException(String.Format("The type of the value of the property [{0}] do not support the operator [{1}].", 
                    _context.CurrentProperty.GetValueType().FullName, currentOperatorName));
        }

        #endregion
    }
}
