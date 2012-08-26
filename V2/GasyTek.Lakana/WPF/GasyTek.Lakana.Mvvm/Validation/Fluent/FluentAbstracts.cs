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
        IFluentEvaluable<TViewModel> Is { get; }
        IFluentEvaluable<TViewModel> IsNot { get; }
        IFluentEvaluable<TViewModel> Has { get; }
        IFluentEvaluable<TViewModel> HasNot { get; }
    }

    /// <summary>
    /// Evaluable expressions that is used with fluent api.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    public interface IFluentEvaluable<TViewModel> : IFluentInterface where TViewModel : ViewModelBase
    {
        IFluentOtherwise<TViewModel> GreaterThan(object value);
        IFluentOtherwise<TViewModel> GreaterThan(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression);
        IFluentOtherwise<TViewModel> LessThan(object value);
        IFluentOtherwise<TViewModel> LessThan(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression);
        IFluentOtherwise<TViewModel> EqualTo(object value);
        IFluentOtherwise<TViewModel> EqualTo(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression);
        IFluentOtherwise<TViewModel> Null();
        IFluentOtherwise<TViewModel> NullOrEmpty();
        IFluentOtherwise<TViewModel> Matching(string pattern);
        
        IFluentOtherwise<TViewModel> ValidEmail();
        IFluentOtherwise<TViewModel> StartingWith(string start);
        IFluentOtherwise<TViewModel> EndingWith(string end);
        IFluentOtherwise<TViewModel> MaxLength(int maxLength);
        IFluentOtherwise<TViewModel> MinLength(int minLength);
        IFluentOtherwise<TViewModel> Required();
        IFluentOtherwise<TViewModel> DifferentOf(object value);
        IFluentOtherwise<TViewModel> DifferentOf(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression);
        IFluentOtherwise<TViewModel> Between(object @from, object to);
        IFluentOtherwise<TViewModel> BetweenPoperties(Expression<Func<TViewModel, IViewModelProperty>> @from, Expression<Func<TViewModel, IViewModelProperty>> to);
    }

    public interface IFluentOtherwise<TViewModel> : IFluentInterface where TViewModel : ViewModelBase
    {
        IFluentVerb<TViewModel> And { get; }
        IFluentVerb<TViewModel> Or { get; }
        IFluentVerb<TViewModel> When(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression);
        void Otherwise(string message);
    }
}
