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
    /// <typeparam name="TPropertyValue">The type of the property value.</typeparam>
    public interface IFluentProperty<TViewModel, TPropertyValue> : IFluentInterface where TViewModel : ViewModelBase
    {
        /// <summary>
        /// Defines the view model property.
        /// </summary>
        /// <param name="propertyExpression">The expression that retrieve the view model property.</param>
        /// <returns></returns>
        IFluentVerb<TViewModel, TPropertyValue> Property(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression);
    }

    /// <summary>
    /// The equivalent of verb part in the world of fluent api.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the targeted view model.</typeparam>
    /// <typeparam name="TPropertyValue">The type of the property value.</typeparam>
    public interface IFluentVerb<TViewModel, TPropertyValue> : IFluentInterface where TViewModel : ViewModelBase
    {
        /// <summary>
        /// Is.
        /// </summary>
        IFluentOperator<TViewModel, TPropertyValue> Is { get; }
        /// <summary>
        /// IsNot.
        /// </summary>
        IFluentOperator<TViewModel, TPropertyValue> IsNot { get; }
        /// <summary>
        /// Has.
        /// </summary>
        IFluentOperator<TViewModel, TPropertyValue> Has { get; }
        /// <summary>
        /// HasNot.
        /// </summary>
        IFluentOperator<TViewModel, TPropertyValue> HasNot { get; }
    }

    /// <summary>
    /// Evaluable expressions that is used with fluent api.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the targeted view model.</typeparam>
    /// <typeparam name="TPropertyValue">The type of the property value.</typeparam>
    public interface IFluentOperator<TViewModel, TPropertyValue> : IFluentInterface where TViewModel : ViewModelBase
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IFluentOtherwise : IFluentInterface
    {
        /// <summary>
        /// Defines the message to display when the rule is broken.
        /// </summary>
        /// <param name="message">The message.</param>
        void Otherwise(string message);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <typeparam name="TPropertyValue">The type of the property value.</typeparam>
    public interface IFluentConjunction<TViewModel, TPropertyValue> : IFluentOtherwise where TViewModel : ViewModelBase
    {
        /// <summary>
        /// And conjunction.
        /// </summary>
        IFluentVerb<TViewModel, TPropertyValue> And { get; }

        /// <summary>
        /// Or conjunction.
        /// </summary>
        IFluentVerb<TViewModel, TPropertyValue> Or { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model to which the rule is attached to.</typeparam>
    /// <typeparam name="TPropertyValue">The type of the property value.</typeparam>
    public interface IFluentContinuation<TViewModel, TPropertyValue> : IFluentConjunction<TViewModel, TPropertyValue> where TViewModel : ViewModelBase
    {

    }
}
