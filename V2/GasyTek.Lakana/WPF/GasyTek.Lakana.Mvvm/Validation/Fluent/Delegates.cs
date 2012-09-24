using System.Threading;
using GasyTek.Lakana.Mvvm.ViewModels;

namespace GasyTek.Lakana.Mvvm.Validation.Fluent
{
    /// <summary>
    /// A delegate that provides a value lately.
    /// </summary>
    /// <remarks>This is used typically to retrieve dynamic values that can change over time.</remarks>
    public delegate object LateValue();

    /// <summary>
    /// A delegate that represents a custom validator that executes asynchronously.
    /// </summary>
    /// <remarks>You should not manipulate UI components within the async operation.</remarks>
    /// <param name="value">The property's value to validate.</param>
    /// <param name="cancellationToken">The cancellation token that can be used to cancel the async operation.</param>
    /// <returns></returns>
    public delegate bool CustomValidator(object value, CancellationToken cancellationToken);
}