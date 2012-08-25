using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GasyTek.Lakana.Mvvm.ViewModelProperties;

namespace GasyTek.Lakana.Mvvm.Validation.Fluent
{
    /// <summary>
    /// EqualTo operator.
    /// </summary>
    internal class EqualToExpression : EvaluableExpression
    {
        private readonly Func<object> _valueProvider;

        public object Value
        {
            get { return _valueProvider != null ? _valueProvider() : null; }
        }

        private EqualToExpression(Func<object> evaluatedValueProvider, Func<object> valueProvider)
            : base(evaluatedValueProvider)
        {
            _valueProvider = valueProvider;
        }

        private EqualToExpression(IViewModelProperty evaluatedProperty, object value)
            : this(evaluatedProperty.GetValue, () => value)
        {
        }

        private EqualToExpression(IViewModelProperty evaluatedProperty, IViewModelProperty valueProperty)
            : this(evaluatedProperty.GetValue, valueProperty.GetValue)
        {
        }

        #region Factory methods

        public static EqualToExpression CreateGeneric(Func<object> evaluatedValueProvider, Func<object> valueProvider)
        {
            return new EqualToExpression(evaluatedValueProvider, valueProvider);
        }

        public static EqualToExpression CreateUsingValue(IViewModelProperty evaluatedProperty, object value)
        {
            return new EqualToExpression(evaluatedProperty, value);
        }

        public static EqualToExpression CreateUsingProperty(IViewModelProperty evaluatedProperty, IViewModelProperty valueProperty)
        {
            return new EqualToExpression(evaluatedProperty, valueProperty);
        }

        #endregion

        public override Task<bool> Evaluate()
        {
            return new Task<bool>(() => Equals(EvaluatedValue, Value));
        }
    }

    /// <summary>
    /// GreaterThan operator.
    /// </summary>
    /// <remarks>Applies only on objects that implements IComparable.</remarks>
    internal class GreaterThanExpression : EvaluableExpression
    {
        private readonly Func<object> _valueProvider;

        public object Value
        {
            get { return _valueProvider != null ? _valueProvider() : null; }
        }

        private GreaterThanExpression(Func<object> evaluatedValueProvider, Func<object> valueProvider)
            : base(evaluatedValueProvider)
        {
            _valueProvider = valueProvider;
        }

        private GreaterThanExpression(IViewModelProperty evaluatedProperty, object value)
            : this(evaluatedProperty.GetValue, () => value)
        {
        }

        private GreaterThanExpression(IViewModelProperty evaluatedProperty, IViewModelProperty valueProperty)
            : this(evaluatedProperty.GetValue, valueProperty.GetValue)
        {
        }

        #region Factory methods

        public static GreaterThanExpression CreateGeneric(Func<object> evaluatedValueProvider, Func<object> valueProvider)
        {
            return new GreaterThanExpression(evaluatedValueProvider, valueProvider);
        }

        public static GreaterThanExpression CreateUsingValue(IViewModelProperty evaluatedProperty, object value)
        {
            return new GreaterThanExpression(evaluatedProperty, value);
        }

        public static GreaterThanExpression CreateUsingProperty(IViewModelProperty evaluatedProperty, IViewModelProperty valueProperty)
        {
            return new GreaterThanExpression(evaluatedProperty, valueProperty);
        }

        #endregion

        public override Task<bool> Evaluate()
        {
            return new Task<bool>(() =>
                                      {
                                          var cmp1 = EvaluatedValue as IComparable;
                                          var cmp2 = Value as IComparable;
                                          if (cmp1 == null || cmp2 == null)
                                              throw new InvalidOperationException("GreaterThan operator : one of the operand is not comparable. Make them implement IComparable.");
                                          return cmp1.CompareTo(cmp2) > 0;
                                      });
        }
    }

    /// <summary>
    /// LessThan operator.
    /// </summary>
    /// /// <remarks>Applies only on objects that implements IComparable.</remarks>
    internal class LessThanExpression : EvaluableExpression
    {
        private readonly Func<object> _valueProvider;

        public object Value
        {
            get { return _valueProvider != null ? _valueProvider() : null; }
        }

        private LessThanExpression(Func<object> evaluatedValueProvider, Func<object> valueProvider)
            : base(evaluatedValueProvider)
        {
            _valueProvider = valueProvider;
        }

        private LessThanExpression(IViewModelProperty evaluatedProperty, object value)
            : this(evaluatedProperty.GetValue, () => value)
        {
        }

        private LessThanExpression(IViewModelProperty evaluatedProperty, IViewModelProperty valueProperty)
            : this(evaluatedProperty.GetValue, valueProperty.GetValue)
        {
        }

        #region Factory methods

        public static LessThanExpression CreateGeneric(Func<object> evaluatedValueProvider, Func<object> valueProvider)
        {
            return new LessThanExpression(evaluatedValueProvider, valueProvider);
        }

        public static LessThanExpression CreateUsingValue(IViewModelProperty evaluatedProperty, object value)
        {
            return new LessThanExpression(evaluatedProperty, value);
        }

        public static LessThanExpression CreateUsingProperty(IViewModelProperty evaluatedProperty, IViewModelProperty valueProperty)
        {
            return new LessThanExpression(evaluatedProperty, valueProperty);
        }

        #endregion

        public override Task<bool> Evaluate()
        {
            return new Task<bool>(() =>
            {
                var cmp1 = EvaluatedValue as IComparable;
                var cmp2 = Value as IComparable;
                if (cmp1 == null || cmp2 == null)
                    throw new InvalidOperationException("LessThan operator : one of the operand is not comparable. Make them implement IComparable.");
                return cmp1.CompareTo(cmp2) < 0;
            });
        }
    }

    /// <summary>
    /// Matching operator.
    /// </summary>
    /// <remarks>Applies only on strings.</remarks>
    internal class MatchingExpression : EvaluableExpression
    {
        public string Pattern { get; private set; }

        private MatchingExpression(Func<object> evaluatedValueProvider, string pattern)
            : base(evaluatedValueProvider)
        {
            Pattern = pattern;
        }

        private MatchingExpression(IViewModelProperty evaluatedProperty, string pattern)
            : this(evaluatedProperty.GetValue, pattern)
        {
        }

        #region Factory methods

        public static MatchingExpression CreateGeneric(Func<object> evaluatedValueProvider, string pattern)
        {
            return new MatchingExpression(evaluatedValueProvider, pattern);
        }

        public static MatchingExpression CreateUsingProperty(IViewModelProperty evaluatedProperty, string pattern)
        {
            return new MatchingExpression(evaluatedProperty, pattern);
        }

        #endregion

        public override Task<bool> Evaluate()
        {
            return new Task<bool>(() =>
            {
                if (EvaluatedValue is string == false)
                    throw new InvalidOperationException("Matching operator : matching to a regular expression can be applied only on string values. ");

                return Regex.IsMatch(EvaluatedValue.ToString(), Pattern);
            });
        }
    }
}
