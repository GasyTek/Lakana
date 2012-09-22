using System;
using System.ComponentModel;
using System.Linq.Expressions;
using GasyTek.Lakana.Mvvm.ViewModelProperties;
using GasyTek.Lakana.Mvvm.ViewModels;

namespace GasyTek.Lakana.Mvvm.Validation.Fluent
{
    /// <summary>
    /// Base interface for fluent api.
    /// </summary>
    /// <remarks> 
    /// Hides standard Object members to make fluent interfaces easier to read.
    /// Based on blog post by @kzu here:
    /// http://www.clariusconsulting.net/blogs/kzu/archive/2008/03/10/58301.aspx
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentInterface
    {
        /// <summary>
        /// Standard System.Object member.
        /// </summary>
        /// <returns>Standard result.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        /// <summary>
        /// Standard System.Object member.
        /// </summary>
        /// <returns>Standard result.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        /// <summary>
        /// Standard System.Object member.
        /// </summary>
        /// <returns>Standard result.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

        /// <summary>
        /// Standard System.Object member.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>Standard result.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object other);
    }

    /// <summary>
    /// The first fluent api part that must be used. It defines the property to configure and begins the fluent rule definition.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the targeted view model.</typeparam>
    public interface IFluentProperty<TViewModel> : IFluentInterface where TViewModel : ViewModelBase
    {
        /// <summary>
        /// Defines the view model property.
        /// </summary>
        /// <param name="propertyExpression">The expression that retrieve the view model property.</param>
        /// <returns></returns>
        IFluentVerb<TViewModel> Property(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression);
    }

    /// <summary>
    /// The equivalent of verb part in the world of fluent api.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the targeted view model.</typeparam>
    public interface IFluentVerb<TViewModel> : IFluentInterface where TViewModel : ViewModelBase
    {
        /// <summary>
        /// Is.
        /// </summary>
        IFluentEvaluable<TViewModel> Is { get; }
        /// <summary>
        /// IsNot.
        /// </summary>
        IFluentEvaluable<TViewModel> IsNot { get; }
        /// <summary>
        /// Has.
        /// </summary>
        IFluentEvaluable<TViewModel> Has { get; }
        /// <summary>
        /// HasNot.
        /// </summary>
        IFluentEvaluable<TViewModel> HasNot { get; }
    }

    /// <summary>
    /// Evaluable expressions that is used with fluent api.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the targeted view model.</typeparam>
    public interface IFluentEvaluable<TViewModel> : IFluentInterface where TViewModel : ViewModelBase
    {
        /// <summary>
        /// The property must satisfy the custom rule.
        /// </summary>
        /// <param name="customValidator">The custom rule.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> Satisfying(CustomValidator customValidator);

        /// <summary>
        /// Assert that proeprty value is greater than provided value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> GreaterThan(object value);

        /// <summary>
        /// Assert that proeprty value is greater than provided property.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        IFluentOtherwise<TViewModel> GreaterThan(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression);

        /// <summary>
        /// Assert that proeprty value is greater than provided value.
        /// </summary>
        /// <param name="lateValue"> </param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> GreaterThan(LateValue lateValue);
        
        /// <summary>
        /// GreaterThanOrEqualn.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> GreaterThanOrEqualTo(object value);

        /// <summary>
        /// GreaterThanOrEqualTo
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        IFluentOtherwise<TViewModel> GreaterThanOrEqualTo(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression);

        /// <summary>
        /// GreaterThanOrEqualTo.
        /// </summary>
        /// <param name="lateValue"> </param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> GreaterThanOrEqualTo(LateValue lateValue);

        /// <summary>
        /// LessThan
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> LessThan(object value);

        /// <summary>
        /// LessThan
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> LessThan(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression);

        /// <summary>
        /// LessThan
        /// </summary>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> LessThan(LateValue lateValue);

        /// <summary>
        /// LessThanOrEqualTo
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> LessThanOrEqualTo(object value);

        /// <summary>
        /// LessThanOrEqualTo
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> LessThanOrEqualTo(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression);

        /// <summary>
        /// LessThanOrEqualTo
        /// </summary>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> LessThanOrEqualTo(LateValue lateValue);

        /// <summary>
        /// EqualTo
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> EqualTo(object value);

        /// <summary>
        /// EqualTo.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> EqualTo(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression);

        /// <summary>
        /// EqualTo
        /// </summary>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> EqualTo(LateValue lateValue);

        /// <summary>
        /// Null.
        /// </summary>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> Null();

        /// <summary>
        /// NullOrEmpty.
        /// </summary>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> NullOrEmpty();

        /// <summary>
        /// Matching.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> Matching(string pattern);

        /// <summary>
        /// Validates email property.
        /// </summary>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> ValidEmail();

        /// <summary>
        /// Matches if property value starts with provided pattern.
        /// </summary>
        /// <param name="start">The pattern.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> StartingWith(string start);

        /// <summary>
        /// Matches if property value ends with provided pattern.
        /// </summary>
        /// <param name="end">The pattern.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> EndingWith(string end);

        /// <summary>
        /// Validates max length of proeprty value.
        /// </summary>
        /// <param name="maxLength">Max length.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> MaxLength(int maxLength);

        /// <summary>
        /// Validates min length of property value.
        /// </summary>
        /// <param name="minLength">Min length.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> MinLength(int minLength);

        /// <summary>
        /// Assert that property value is required.
        /// </summary>
        /// <remarks>Strings must not be null nor empty.</remarks>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> Required();

        /// <summary>
        ///Assert that property value is different of provided value.
        /// </summary>
        IFluentOtherwise<TViewModel> DifferentOf(object value);
        
        /// <summary>
        /// Assert that property value is different of another property's value.
        /// </summary>
        /// <param name="propertyExpression">The other property.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> DifferentOf(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression);

        /// <summary>
        /// Assert that property value is different of a value provided by asynchrnoous operation.
        /// </summary>
        /// <param name="lateValue">The async value.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> DifferentOf(LateValue lateValue);

        /// <summary>
        /// Assert that property value is between provided values.
        /// </summary>
        /// <param name="from">Lower bound.</param>
        /// <param name="to">Upper bound.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> Between(object @from, object to);

        /// <summary>
        /// Assert that property value is between provided properties.
        /// </summary>
        /// <param name="from">Lower bound.</param>
        /// <param name="to">Upper bound.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> BetweenPoperties(Expression<Func<TViewModel, IViewModelProperty>> @from, Expression<Func<TViewModel, IViewModelProperty>> to);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model that the rule is attached to.</typeparam>
    public interface IFluentOtherwise<TViewModel> : IFluentInterface where TViewModel : ViewModelBase
    {
        /// <summary>
        /// And conjunction.
        /// </summary>
        IFluentVerb<TViewModel> And { get; }

        /// <summary>
        /// Or conjunction.
        /// </summary>
        IFluentVerb<TViewModel> Or { get; }

        /// <summary>
        /// Whens the specified property expression.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <remarks>Not implemented yet</remarks>
        /// <returns></returns>
        IFluentVerb<TViewModel> When(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression);

        /// <summary>
        /// Defines the message to display when the rule is broken.
        /// </summary>
        /// <param name="message">The message.</param>
        void Otherwise(string message);
    }
}
