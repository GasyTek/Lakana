using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GasyTek.Lakana.Mvvm.Validation.Fluent
{
    /// <summary>
    /// EqualTo operator.
    /// </summary>
    // [DebuggerDisplay("LeftValue = {LeftValue}, RightValue = {RightValue}")]
    internal class EqualToExpression : EvaluableExpression
    {
        private readonly Func<object> _rightValueProvider;

        public object RightValue
        {
            get { return _rightValueProvider != null ? _rightValueProvider() : default(object); }
        }

        internal EqualToExpression(Func<object> leftValueProvider, Func<object> rightValueProvider)
            : base(leftValueProvider)
        {
            _rightValueProvider = rightValueProvider;
        }

        public override Task<bool> Evaluate(CancellationToken cancellationToken)
        {
            return new Task<bool>(() => Equals(LeftValue, RightValue), cancellationToken, TaskCreationOptions.AttachedToParent);
        }
    }

    /// <summary>
    /// Strictly GreaterThan operator.
    /// </summary>
    /// <remarks>Applies only on objects that implements IComparable.</remarks>
    // [DebuggerDisplay("LeftValue = {LeftValue}, RightValue = {RightValue}")]
    internal class GreaterThanExpression : EvaluableExpression
    {
        private readonly Func<object> _rightValueProvider;

        public object RightValue
        {
            get { return _rightValueProvider != null ? _rightValueProvider() : null; }
        }

        internal GreaterThanExpression(Func<object> leftValueProvider, Func<object> rightValueProvider)
            : base(leftValueProvider)
        {
            _rightValueProvider = rightValueProvider;
        }

        public override Task<bool> Evaluate(CancellationToken cancellationToken)
        {
            return new Task<bool>(() =>
                                      {
                                          var cmp1 = LeftValue as IComparable;
                                          var cmp2 = RightValue as IComparable;
                                          if (cmp1 == null || cmp2 == null)
                                              throw new InvalidOperationException("GreaterThan operator : one of the operand is not comparable. Make them implement IComparable.");
                                          return cmp1.CompareTo(cmp2) > 0;
                                      }, cancellationToken, TaskCreationOptions.AttachedToParent);
        }
    }

    /// <summary>
    /// Strictly LessThan operator.
    /// </summary>
    /// /// <remarks>Applies only on objects that implements IComparable.</remarks>
    // [DebuggerDisplay("LeftValue = {LeftValue}, RightValue = {RightValue}")]
    internal class LessThanExpression : EvaluableExpression
    {
        private readonly Func<object> _rightValueProvider;

        public object RightValue
        {
            get { return _rightValueProvider != null ? _rightValueProvider() : null; }
        }

        internal LessThanExpression(Func<object> leftValueProvider, Func<object> rightValueProvider)
            : base(leftValueProvider)
        {
            _rightValueProvider = rightValueProvider;
        }

        public override Task<bool> Evaluate(CancellationToken cancellationToken)
        {
            return new Task<bool>(() =>
                                    {
                                        var cmp1 = LeftValue as IComparable;
                                        var cmp2 = RightValue as IComparable;
                                        if (cmp1 == null || cmp2 == null)
                                            throw new InvalidOperationException("LessThan operator : one of the operand is not comparable. Make them implement IComparable.");
                                        return cmp1.CompareTo(cmp2) < 0;
                                    }, cancellationToken, TaskCreationOptions.AttachedToParent);
        }
    }

    /// <summary>
    /// Matching operator.
    /// </summary>
    /// <remarks>Applies only on strings.</remarks>
    internal class MatchingExpression : EvaluableExpression
    {
        public string Pattern { get; private set; }

        internal MatchingExpression(Func<object> leftValueProvider, string pattern)
            : base(leftValueProvider)
        {
            Pattern = pattern;
        }

        public override Task<bool> Evaluate(CancellationToken cancellationToken)
        {
            return new Task<bool>(() =>
                                    {
                                        if (LeftValue is string == false)
                                            throw new InvalidOperationException("Matching operator : matching to a regular expression can be applied only on string values. ");

                                        return Regex.IsMatch(LeftValue.ToString(), Pattern);
                                    }, cancellationToken, TaskCreationOptions.AttachedToParent);
        }
    }

    /// <summary>
    /// Custom rule operator.
    /// </summary>
    internal class CustomValidationExpression : EvaluableExpression
    {
        private readonly CustomValidator _customValidator;

        public CustomValidator CustomValidator
        {
            get { return _customValidator; }
        }

        public CustomValidationExpression(Func<object> leftValueProvider, CustomValidator customValidator) 
            : base(leftValueProvider)
        {
            _customValidator = customValidator;
        }

        public override Task<bool> Evaluate(CancellationToken cancellationToken)
        {
            if (_customValidator == null)
                throw new ArgumentNullException("customValidator");

            return new Task<bool>(() => _customValidator (LeftValue, cancellationToken), cancellationToken, TaskCreationOptions.AttachedToParent);
        }
    }
}
