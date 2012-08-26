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
    internal class FluentImplementer<TViewModel> 
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
                AddToken(new NotExpression());
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
                AddToken(new NotExpression());
                return this;
            }
        }

        #endregion

        #region Fluent api evaluables

        public IFluentOtherwise<TViewModel> GreaterThan(object value)
        {
            EnsureContextCurrentPropertyIsNotNull();
            EnsureContextCurrentPropertyValueIsOfType(typeof(IComparable), "GreaterThan");

            AddToken(GreaterThanExpression.CreateUsingValue(_context.CurrentProperty, value));
            return this;
        }

        public IFluentOtherwise<TViewModel> GreaterThan(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
        {
            EnsureContextCurrentPropertyIsNotNull();
            EnsureContextCurrentPropertyValueIsOfType(typeof(IComparable), "GreaterThan");

            UpdateContext(propertyExpression);

            AddToken(GreaterThanExpression.CreateUsingProperty(_context.CurrentProperty, propertyExpression.Compile()(_viewModel)));
            return this;
        }

        public IFluentOtherwise<TViewModel> LessThan(object value)
        {
            EnsureContextCurrentPropertyIsNotNull();
            EnsureContextCurrentPropertyValueIsOfType(typeof(IComparable), "LessThan");

            AddToken(LessThanExpression.CreateUsingValue(_context.CurrentProperty, value));
            return this;
        }

        public IFluentOtherwise<TViewModel> LessThan(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
        {
            EnsureContextCurrentPropertyIsNotNull();
            EnsureContextCurrentPropertyValueIsOfType(typeof(IComparable), "LessThan");

            UpdateContext(propertyExpression);

            AddToken(LessThanExpression.CreateUsingProperty(_context.CurrentProperty, propertyExpression.Compile()(_viewModel)));
            return this;
        }

        public IFluentOtherwise<TViewModel> EqualTo(object value)
        {
            EnsureContextCurrentPropertyIsNotNull();

            AddToken(EqualToExpression.CreateUsingValue(_context.CurrentProperty, value));
            return this;
        }

        public IFluentOtherwise<TViewModel> EqualTo(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
        {
            EnsureContextCurrentPropertyIsNotNull();

            UpdateContext(propertyExpression);

            AddToken(EqualToExpression.CreateUsingProperty(_context.CurrentProperty, propertyExpression.Compile()(_viewModel)));
            return this;
        }

        public IFluentOtherwise<TViewModel> Null()
        {
            EnsureContextCurrentPropertyIsNotNull();

            AddToken(EqualToExpression.CreateUsingValue(_context.CurrentProperty, null));
            return this;
        }

        public IFluentOtherwise<TViewModel> NullOrEmpty()
        {
            EnsureContextCurrentPropertyIsNotNull();
            
            AddToken(EqualToExpression.CreateUsingValue(_context.CurrentProperty, null));
            AddToken(new OrExpression());
            AddToken(EqualToExpression.CreateUsingValue(_context.CurrentProperty, string.Empty));
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

            AddToken(MatchingExpression.CreateUsingProperty(_context.CurrentProperty, pattern));
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

            AddToken(new OpenParenthesis());
            AddToken(LessThanExpression.CreateGeneric(evaluatedValueProvider, valueProvider));
            AddToken(new OrExpression());
            AddToken(EqualToExpression.CreateGeneric(evaluatedValueProvider, valueProvider));
            AddToken(new CloseParenthesis());

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

            AddToken(new OpenParenthesis());
            AddToken(GreaterThanExpression.CreateGeneric(evaluatedValueProvider, valueProvider));
            AddToken(new OrExpression());
            AddToken(EqualToExpression.CreateGeneric(evaluatedValueProvider, valueProvider));
            AddToken(new CloseParenthesis());

            return this;
        }

        public IFluentOtherwise<TViewModel> Required()
        {
            EnsureContextCurrentPropertyIsNotNull();
            
            // required = is neither null nor empty
            AddToken(new NotExpression());
            AddToken(new OpenParenthesis());
            AddToken(EqualToExpression.CreateUsingValue(_context.CurrentProperty, null));
            AddToken(new OrExpression());
            AddToken(EqualToExpression.CreateUsingValue(_context.CurrentProperty, string.Empty));
            AddToken(new CloseParenthesis());
            return this;
        }

        public IFluentOtherwise<TViewModel> DifferentOf(object value)
        {
            EnsureContextCurrentPropertyIsNotNull();

            // different of = not equal to value
            AddToken(new NotExpression());
            AddToken(EqualToExpression.CreateUsingValue(_context.CurrentProperty, value));
            return this;
        }

        public IFluentOtherwise<TViewModel> DifferentOf(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
        {
            EnsureContextCurrentPropertyIsNotNull();

            UpdateContext(propertyExpression);

            // different of = not equal to property
            AddToken(new NotExpression());
            AddToken(EqualToExpression.CreateUsingProperty(_context.CurrentProperty, propertyExpression.Compile()(_viewModel)));
            return this;
        }

        public IFluentOtherwise<TViewModel> Between(object @from, object to)
        {
            EnsureContextCurrentPropertyIsNotNull();

            // uses parenthesis to satisfy syntax : IsNot.Between (...)
            AddToken(new OpenParenthesis());
            AddToken(GreaterThanExpression.CreateUsingValue(_context.CurrentProperty, @from));
            AddToken(new AndExpression());
            AddToken(LessThanExpression.CreateUsingValue(_context.CurrentProperty, to));
            AddToken(new CloseParenthesis());

            return this;
        }

        public IFluentOtherwise<TViewModel> BetweenPoperties(Expression<Func<TViewModel, IViewModelProperty>> @from, Expression<Func<TViewModel, IViewModelProperty>> to)
        {
            EnsureContextCurrentPropertyIsNotNull();

            UpdateContext(@from);
            UpdateContext(to);

            // uses parenthesis to satisfy syntax : IsNot.Between (...)
            AddToken(new OpenParenthesis());
            AddToken(GreaterThanExpression.CreateUsingProperty(_context.CurrentProperty, @from.Compile()(_viewModel)));
            AddToken(new AndExpression());
            AddToken(LessThanExpression.CreateUsingProperty(_context.CurrentProperty, to.Compile()(_viewModel)));
            AddToken(new CloseParenthesis());

            return this;
        }

        #endregion

        #region Fluent api otherwise

        public IFluentVerb<TViewModel> And
        {
            get
            {
                AddToken(new AndExpression());
                return this;
            }
        }

        public IFluentVerb<TViewModel> Or
        {
            get
            {
                AddToken(new OrExpression());
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
            var valueType = _context.CurrentProperty.GetValueType();
            if (expectedType.IsAssignableFrom(valueType) == false)
                throw new InvalidOperationException(String.Format("The type of the value of the property [{0}] do not support the operator [{1}].", 
                    _context.CurrentProperty.GetValueType().FullName, currentOperatorName));
        }

        #endregion
    }
}
