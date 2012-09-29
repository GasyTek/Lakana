using System;
using System.Linq.Expressions;
using GasyTek.Lakana.Mvvm.ViewModelProperties;
using GasyTek.Lakana.Mvvm.ViewModels;

namespace GasyTek.Lakana.Mvvm.Validation.Fluent
{
    /// <summary>
    /// Fluent operators.
    /// </summary>
    public static class FluentComparisonOperator
    {
        public static IFluentContinuation<TViewModel, TPropertyValue> GreaterThan<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, TPropertyValue value)
            where TViewModel : ViewModelBase
            where TPropertyValue : IComparable
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();
            implementerInstance.AddToken(ExpressionNode.GreaterThanValue(implementerInstance.Context.CurrentProperty, value));
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> GreaterThan<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
            where TViewModel : ViewModelBase
            where TPropertyValue : IComparable
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            implementerInstance.UpdateContext(propertyExpression);

            implementerInstance.AddToken(ExpressionNode.GreaterThanProperty(implementerInstance.Context.CurrentProperty, propertyExpression.Compile()(implementerInstance.ViewModel)));
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> GreaterThan<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, LateValue<TPropertyValue> lateValue)
            where TViewModel : ViewModelBase
            where TPropertyValue : IComparable
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();
            
            implementerInstance.AddToken(ExpressionNode.GreaterThanLateValue(implementerInstance.Context.CurrentProperty, () => lateValue()));
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> GreaterThanOrEqualTo<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, TPropertyValue value)
            where TViewModel : ViewModelBase
            where TPropertyValue : IComparable
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            implementerInstance.AddToken(ExpressionNode.OpenParenthesis());
            implementerInstance.AddToken(ExpressionNode.GreaterThanValue(implementerInstance.Context.CurrentProperty, value));
            implementerInstance.AddToken(ExpressionNode.Or());
            implementerInstance.AddToken(ExpressionNode.EqualToValue(implementerInstance.Context.CurrentProperty, value));
            implementerInstance.AddToken(ExpressionNode.CloseParenthesis());
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> GreaterThanOrEqualTo<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
            where TViewModel : ViewModelBase
            where TPropertyValue : IComparable
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            implementerInstance.UpdateContext(propertyExpression);

            implementerInstance.AddToken(ExpressionNode.OpenParenthesis());
            implementerInstance.AddToken(ExpressionNode.GreaterThanProperty(implementerInstance.Context.CurrentProperty, propertyExpression.Compile()(implementerInstance.ViewModel)));
            implementerInstance.AddToken(ExpressionNode.Or());
            implementerInstance.AddToken(ExpressionNode.EqualToProperty(implementerInstance.Context.CurrentProperty, propertyExpression.Compile()(implementerInstance.ViewModel)));
            implementerInstance.AddToken(ExpressionNode.CloseParenthesis());
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> GreaterThanOrEqualTo<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, LateValue<TPropertyValue> lateValue)
            where TViewModel : ViewModelBase
            where TPropertyValue : IComparable
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            implementerInstance.AddToken(ExpressionNode.OpenParenthesis());
            implementerInstance.AddToken(ExpressionNode.GreaterThanLateValue(implementerInstance.Context.CurrentProperty, () => lateValue()));
            implementerInstance.AddToken(ExpressionNode.Or());
            implementerInstance.AddToken(ExpressionNode.EqualToLateValue(implementerInstance.Context.CurrentProperty, () => lateValue()));
            implementerInstance.AddToken(ExpressionNode.CloseParenthesis());
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> LessThan<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, TPropertyValue value)
            where TViewModel : ViewModelBase
            where TPropertyValue : IComparable
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            implementerInstance.AddToken(ExpressionNode.LessThanValue(implementerInstance.Context.CurrentProperty, value));
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> LessThan<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
            where TViewModel : ViewModelBase
            where TPropertyValue : IComparable
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            implementerInstance.UpdateContext(propertyExpression);

            implementerInstance.AddToken(ExpressionNode.LessThanProperty(implementerInstance.Context.CurrentProperty, propertyExpression.Compile()(implementerInstance.ViewModel)));
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> LessThan<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, LateValue<TPropertyValue> lateValue)
            where TViewModel : ViewModelBase
            where TPropertyValue : IComparable
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            implementerInstance.AddToken(ExpressionNode.LessThanLateValue(implementerInstance.Context.CurrentProperty, () => lateValue()));
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> LessThanOrEqualTo<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, TPropertyValue value)
            where TViewModel : ViewModelBase
            where TPropertyValue : IComparable
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            implementerInstance.AddToken(ExpressionNode.OpenParenthesis());
            implementerInstance.AddToken(ExpressionNode.LessThanValue(implementerInstance.Context.CurrentProperty, value));
            implementerInstance.AddToken(ExpressionNode.Or());
            implementerInstance.AddToken(ExpressionNode.EqualToValue(implementerInstance.Context.CurrentProperty, value));
            implementerInstance.AddToken(ExpressionNode.CloseParenthesis());
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> LessThanOrEqualTo<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
            where TViewModel : ViewModelBase
            where TPropertyValue : IComparable
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            implementerInstance.UpdateContext(propertyExpression);

            implementerInstance.AddToken(ExpressionNode.OpenParenthesis());
            implementerInstance.AddToken(ExpressionNode.LessThanProperty(implementerInstance.Context.CurrentProperty, propertyExpression.Compile()(implementerInstance.ViewModel)));
            implementerInstance.AddToken(ExpressionNode.Or());
            implementerInstance.AddToken(ExpressionNode.EqualToProperty(implementerInstance.Context.CurrentProperty, propertyExpression.Compile()(implementerInstance.ViewModel)));
            implementerInstance.AddToken(ExpressionNode.CloseParenthesis());
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> LessThanOrEqualTo<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, LateValue<TPropertyValue> lateValue)
            where TViewModel : ViewModelBase
            where TPropertyValue : IComparable
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            implementerInstance.AddToken(ExpressionNode.OpenParenthesis());
            implementerInstance.AddToken(ExpressionNode.LessThanLateValue(implementerInstance.Context.CurrentProperty, () => lateValue()));
            implementerInstance.AddToken(ExpressionNode.Or());
            implementerInstance.AddToken(ExpressionNode.EqualToLateValue(implementerInstance.Context.CurrentProperty, () => lateValue()));
            implementerInstance.AddToken(ExpressionNode.CloseParenthesis());
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }
    }
    
    public static class FluentEqualityOperator
    {
        public static IFluentContinuation<TViewModel, TPropertyValue> EqualTo<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, TPropertyValue value)
            where TViewModel : ViewModelBase
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            implementerInstance.AddToken(ExpressionNode.EqualToValue(implementerInstance.Context.CurrentProperty, value));
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> EqualTo<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
            where TViewModel : ViewModelBase
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            implementerInstance.UpdateContext(propertyExpression);

            implementerInstance.AddToken(ExpressionNode.EqualToProperty(implementerInstance.Context.CurrentProperty, propertyExpression.Compile()(implementerInstance.ViewModel)));
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> EqualTo<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, LateValue<TPropertyValue> lateValue)
            where TViewModel : ViewModelBase
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            implementerInstance.AddToken(ExpressionNode.EqualToLateValue(implementerInstance.Context.CurrentProperty, () => lateValue()));
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> DifferentOf<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, TPropertyValue value)
            where TViewModel : ViewModelBase
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            // different of = not equal to value
            implementerInstance.AddToken(ExpressionNode.Not());
            implementerInstance.AddToken(ExpressionNode.EqualToValue(implementerInstance.Context.CurrentProperty, value));
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> DifferentOf<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
            where TViewModel : ViewModelBase
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            implementerInstance.UpdateContext(propertyExpression);

            // different of = not equal to property
            implementerInstance.AddToken(ExpressionNode.Not());
            implementerInstance.AddToken(ExpressionNode.EqualToProperty(implementerInstance.Context.CurrentProperty, propertyExpression.Compile()(implementerInstance.ViewModel)));
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> DifferentOf<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, LateValue<TPropertyValue> lateValue)
            where TViewModel : ViewModelBase
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            implementerInstance.AddToken(ExpressionNode.Not());
            implementerInstance.AddToken(ExpressionNode.EqualToLateValue(implementerInstance.Context.CurrentProperty, () => lateValue()));
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }



    }

    public static class FluentStringOperator
    {
        public static IFluentContinuation<TViewModel, TPropertyValue> NullOrEmpty<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance)
            where TViewModel : ViewModelBase
            where TPropertyValue : IComparable<string>, IEquatable<string>
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            implementerInstance.AddToken(ExpressionNode.OpenParenthesis());
            implementerInstance.AddToken(ExpressionNode.EqualToValue(implementerInstance.Context.CurrentProperty, null));
            implementerInstance.AddToken(ExpressionNode.Or());
            implementerInstance.AddToken(ExpressionNode.EqualToValue(implementerInstance.Context.CurrentProperty, string.Empty));
            implementerInstance.AddToken(ExpressionNode.CloseParenthesis());
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        /// <summary>
        /// Matches the specified regular expression pattern.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <typeparam name="TPropertyValue">The type of the property value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        public static IFluentContinuation<TViewModel, TPropertyValue> Matching<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, string pattern)
            where TViewModel : ViewModelBase
            where TPropertyValue : IComparable<string>, IEquatable<string>
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            implementerInstance.AddToken(ExpressionNode.MatchingProperty(implementerInstance.Context.CurrentProperty, pattern));
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> ValidEmail<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance)
            where TViewModel : ViewModelBase
            where TPropertyValue : IComparable<string>, IEquatable<string>
        {
            return instance.Matching(ValidationConstants.EmailPattern);
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> StartingWith<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, string start)
            where TViewModel : ViewModelBase
            where TPropertyValue : IComparable<string>, IEquatable<string>
        {
            return instance.Matching(string.Format("^({0})", start));
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> EndingWith<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, string end)
            where TViewModel : ViewModelBase
            where TPropertyValue : IComparable<string>, IEquatable<string>
        {
            return instance.Matching(string.Format("({0})$", end));
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> MaxLength<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, int maxLength)
            where TViewModel : ViewModelBase
            where TPropertyValue : IComparable<string>, IEquatable<string>
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            if (maxLength <= 0)
                throw new InvalidOperationException("maxLength must be greater than 0");

            var evaluatedValueProvider = new Func<object>(() =>
            {
                var value = (string)implementerInstance.Context.CurrentProperty.GetValue();
                return value == null ? 0 : value.Length;
            });
            var valueProvider = new Func<object>(() => maxLength);

            implementerInstance.AddToken(ExpressionNode.OpenParenthesis());
            implementerInstance.AddToken(ExpressionNode.LessThanGeneric(evaluatedValueProvider, valueProvider));
            implementerInstance.AddToken(ExpressionNode.Or());
            implementerInstance.AddToken(ExpressionNode.EqualToGeneric(evaluatedValueProvider, valueProvider));
            implementerInstance.AddToken(ExpressionNode.CloseParenthesis());
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> MinLength<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, int minLength)
            where TViewModel : ViewModelBase
            where TPropertyValue : IComparable<string>, IEquatable<string>
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            if (minLength <= 0)
                throw new InvalidOperationException("minLength must be greater than 0");

            var evaluatedValueProvider = new Func<object>(() =>
            {
                var value = (string)implementerInstance.Context.CurrentProperty.GetValue();
                return value == null ? 0 : value.Length;
            });
            var valueProvider = new Func<object>(() => minLength);

            implementerInstance.AddToken(ExpressionNode.OpenParenthesis());
            implementerInstance.AddToken(ExpressionNode.GreaterThanGeneric(evaluatedValueProvider, valueProvider));
            implementerInstance.AddToken(ExpressionNode.Or());
            implementerInstance.AddToken(ExpressionNode.EqualToGeneric(evaluatedValueProvider, valueProvider));
            implementerInstance.AddToken(ExpressionNode.CloseParenthesis());
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }



    }

    public static class FluentMiscOperator
    {
        public static IFluentContinuation<TViewModel, TPropertyValue> Valid<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance, CustomValidator<TPropertyValue> customValidator)
            where TViewModel : ViewModelBase
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            implementerInstance.AddToken(ExpressionNode.CustomValidation(implementerInstance.Context.CurrentProperty, (value, token) => customValidator((TPropertyValue)value, token)));
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> Null<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance)
            where TViewModel : ViewModelBase
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            implementerInstance.AddToken(ExpressionNode.EqualToValue(implementerInstance.Context.CurrentProperty, null));
            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> Required<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance)
            where TViewModel : ViewModelBase
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            // required = is neither null nor empty
            implementerInstance.AddToken(ExpressionNode.Not());
            implementerInstance.AddToken(ExpressionNode.OpenParenthesis());
            implementerInstance.AddToken(ExpressionNode.EqualToValue(implementerInstance.Context.CurrentProperty, null));
            implementerInstance.AddToken(ExpressionNode.Or());
            implementerInstance.AddToken(ExpressionNode.EqualToValue(implementerInstance.Context.CurrentProperty, string.Empty));
            implementerInstance.AddToken(ExpressionNode.CloseParenthesis());

            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> BetweenValues<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance,
            object @from,
            object to)
            where TViewModel : ViewModelBase
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            // uses parenthesis to satisfy syntax : IsNot.BetweenValues (...)
            implementerInstance.AddToken(ExpressionNode.OpenParenthesis());
            implementerInstance.AddToken(ExpressionNode.OpenParenthesis());
            implementerInstance.AddToken(ExpressionNode.GreaterThanValue(implementerInstance.Context.CurrentProperty, @from));
            implementerInstance.AddToken(ExpressionNode.Or());
            implementerInstance.AddToken(ExpressionNode.EqualToValue(implementerInstance.Context.CurrentProperty, @from));
            implementerInstance.AddToken(ExpressionNode.CloseParenthesis());
            implementerInstance.AddToken(ExpressionNode.And());
            implementerInstance.AddToken(ExpressionNode.OpenParenthesis());
            implementerInstance.AddToken(ExpressionNode.LessThanValue(implementerInstance.Context.CurrentProperty, to));
            implementerInstance.AddToken(ExpressionNode.Or());
            implementerInstance.AddToken(ExpressionNode.EqualToValue(implementerInstance.Context.CurrentProperty, to));
            implementerInstance.AddToken(ExpressionNode.CloseParenthesis());
            implementerInstance.AddToken(ExpressionNode.CloseParenthesis());

            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

        public static IFluentContinuation<TViewModel, TPropertyValue> BetweenPoperties<TViewModel, TPropertyValue>(
            this IFluentOperator<TViewModel, TPropertyValue> instance,
            Expression<Func<TViewModel, IViewModelProperty>> @from,
            Expression<Func<TViewModel, IViewModelProperty>> to)
            where TViewModel : ViewModelBase
        {
            var implementerInstance = (FluentImplementer<TViewModel, TPropertyValue>)instance;
            implementerInstance.EnsureContextCurrentPropertyIsNotNull();

            implementerInstance.UpdateContext(@from);
            implementerInstance.UpdateContext(to);

            // uses parenthesis to satisfy syntax : IsNot.BetweenValues (...)
            implementerInstance.AddToken(ExpressionNode.OpenParenthesis());
            implementerInstance.AddToken(ExpressionNode.OpenParenthesis());
            implementerInstance.AddToken(ExpressionNode.GreaterThanProperty(implementerInstance.Context.CurrentProperty, @from.Compile()(implementerInstance.ViewModel)));
            implementerInstance.AddToken(ExpressionNode.Or());
            implementerInstance.AddToken(ExpressionNode.EqualToProperty(implementerInstance.Context.CurrentProperty, @from.Compile()(implementerInstance.ViewModel)));
            implementerInstance.AddToken(ExpressionNode.CloseParenthesis());
            implementerInstance.AddToken(ExpressionNode.And());
            implementerInstance.AddToken(ExpressionNode.OpenParenthesis());
            implementerInstance.AddToken(ExpressionNode.LessThanProperty(implementerInstance.Context.CurrentProperty, to.Compile()(implementerInstance.ViewModel)));
            implementerInstance.AddToken(ExpressionNode.Or());
            implementerInstance.AddToken(ExpressionNode.EqualToProperty(implementerInstance.Context.CurrentProperty, to.Compile()(implementerInstance.ViewModel)));
            implementerInstance.AddToken(ExpressionNode.CloseParenthesis());
            implementerInstance.AddToken(ExpressionNode.CloseParenthesis());

            return (IFluentContinuation<TViewModel, TPropertyValue>)instance;
        }

    }

}
