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

        #region Factory methods

        public static EqualToExpression CreateGeneric(Func<object> evaluatedValueProvider, Func<object> valueProvider)
        {
            return new EqualToExpression(evaluatedValueProvider, valueProvider);
        }

        public static EqualToExpression CreateUsingValue(IViewModelProperty evaluatedProperty, object value)
        {
            var evaluatedValueProvider = new Func<object>(evaluatedProperty.GetValue);
            var valueProvider = new Func<object>(() => value);
            return CreateGeneric(evaluatedValueProvider, valueProvider);
        }

        public static EqualToExpression CreateUsingProperty(IViewModelProperty evaluatedProperty, IViewModelProperty valueProperty)
        {
            var evaluatedValueProvider = new Func<object>(evaluatedProperty.GetValue);
            var valueProvider = new Func<object>(valueProperty.GetValue);
            return CreateGeneric(evaluatedValueProvider, valueProvider);
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

        #region Factory methods

        public static GreaterThanExpression CreateGeneric(Func<object> evaluatedValueProvider, Func<object> valueProvider)
        {
            return new GreaterThanExpression(evaluatedValueProvider, valueProvider);
        }

        public static GreaterThanExpression CreateUsingValue(IViewModelProperty evaluatedProperty, object value)
        {
            var evaluatedValueProvider = new Func<object>(evaluatedProperty.GetValue);
            var valueProvider = new Func<object>(() => value);
            return CreateGeneric(evaluatedValueProvider, valueProvider);
        }

        public static GreaterThanExpression CreateUsingProperty(IViewModelProperty evaluatedProperty, IViewModelProperty valueProperty)
        {
            var evaluatedValueProvider = new Func<object>(evaluatedProperty.GetValue);
            var valueProvider = new Func<object>(valueProperty.GetValue);
            return CreateGeneric(evaluatedValueProvider, valueProvider);
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

        #region Factory methods

        public static LessThanExpression CreateGeneric(Func<object> evaluatedValueProvider, Func<object> valueProvider)
        {
            return new LessThanExpression(evaluatedValueProvider, valueProvider);
        }

        public static LessThanExpression CreateUsingValue(IViewModelProperty evaluatedProperty, object value)
        {
            var evaluatedValueProvider = new Func<object>(evaluatedProperty.GetValue);
            var valueProvider = new Func<object>(() => value);
            return CreateGeneric(evaluatedValueProvider, valueProvider);
        }

        public static LessThanExpression CreateUsingProperty(IViewModelProperty evaluatedProperty, IViewModelProperty valueProperty)
        {
            var evaluatedValueProvider = new Func<object>(evaluatedProperty.GetValue);
            var valueProvider = new Func<object>(valueProperty.GetValue);
            return CreateGeneric(evaluatedValueProvider, valueProvider);
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

        #region Factory methods

        public static MatchingExpression CreateGeneric(Func<object> evaluatedValueProvider, string pattern)
        {
            return new MatchingExpression(evaluatedValueProvider, pattern);
        }

        public static MatchingExpression CreateUsingProperty(IViewModelProperty evaluatedProperty, string pattern)
        {
            var evaluatedValueProvider = new Func<object>(evaluatedProperty.GetValue);
            return CreateGeneric(evaluatedValueProvider, pattern);
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
