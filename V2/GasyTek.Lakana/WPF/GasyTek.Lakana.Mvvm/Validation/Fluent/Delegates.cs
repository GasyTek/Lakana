using System.Threading;

namespace GasyTek.Lakana.Mvvm.Validation.Fluent
{
    /// <summary>
    /// A delegate that provides a value lately.
    /// </summary>
    /// <remarks>This is used typically wrap dynamic values, that can change over time.</remarks>
    public delegate object LateValue();


    /// <summary>
    /// A delegate that represents an asynchronous custom validation operation.
    /// </summary>
    /// <remarks>You should not manipulate UI components within the async operation.</remarks>
    /// <param name="value">The value to validate.</param>
    /// <param name="cancellationToken">The cancellation token that can be used to cancel the async operation.</param>
    /// <returns></returns>
    public delegate bool CustomValidator(object value, CancellationToken cancellationToken);
}