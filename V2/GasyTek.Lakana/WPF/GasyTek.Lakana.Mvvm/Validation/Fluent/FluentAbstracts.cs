using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
    /// The first fluent api part that must be used. It defines the property to configure.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
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
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
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
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    public interface IFluentEvaluable<TViewModel> : IFluentInterface where TViewModel : ViewModelBase
    {
        /// <summary>
        /// GreaterThan.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> GreaterThan(object value);

        /// <summary>
        /// GreaterThan
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        IFluentOtherwise<TViewModel> GreaterThan(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression);

        /// <summary>
        /// GreaterThan.
        /// </summary>
        /// <param name="valueProvider">The async value.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> GreaterThan(Task<object> valueProvider);
        
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
        /// <param name="valueProvider">The async value.</param>
        /// <returns></returns>
        IFluentOtherwise<TViewModel> GreaterThanOrEqualTo(Task<object> valueProvider);

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
        IFluentOtherwise<TViewModel> LessThan(Task<object> valueProvider);

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
        IFluentOtherwise<TViewModel> LessThanOrEqualTo(Task<object> valueProvider);

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
        IFluentOtherwise<TViewModel> EqualTo(Task<object> valueProvider);

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
        
        IFluentOtherwise<TViewModel> ValidEmail();
        IFluentOtherwise<TViewModel> StartingWith(string start);
        IFluentOtherwise<TViewModel> EndingWith(string end);
        IFluentOtherwise<TViewModel> MaxLength(int maxLength);
        IFluentOtherwise<TViewModel> MinLength(int minLength);
        IFluentOtherwise<TViewModel> Required();
        IFluentOtherwise<TViewModel> DifferentOf(object value);
        IFluentOtherwise<TViewModel> DifferentOf(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression);
        IFluentOtherwise<TViewModel> DifferentOf(Task<object> valueProvider);
        IFluentOtherwise<TViewModel> Between(object @from, object to);
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
