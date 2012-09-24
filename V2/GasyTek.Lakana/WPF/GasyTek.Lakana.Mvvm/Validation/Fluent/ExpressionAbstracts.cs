using System;
using System.Threading;
using System.Threading.Tasks;
using GasyTek.Lakana.Mvvm.ViewModelProperties;

namespace GasyTek.Lakana.Mvvm.Validation.Fluent
{
    /// <summary>
    /// Base class for expressions.
    /// </summary>
    internal abstract class ExpressionNode
    {
        public abstract Task<bool> Evaluate(CancellationToken cancellationToken);

        #region Factory : Parenthesis Expressions

        public static OpenParenthesis OpenParenthesis()
        {
            return new OpenParenthesis();
        }

        public static CloseParenthesis CloseParenthesis()
        {
            return new CloseParenthesis();
        }

        #endregion

        #region Factory : Constants Expressions

        public static TrueExpression True()
        {
            return new TrueExpression();
        }

        public static FalseExpression False()
        {
            return new FalseExpression();
        }

        #endregion

        #region Factory : Operator Expressions

        public static AndExpression And()
        {
            return new AndExpression();
        }

        public static OrExpression Or()
        {
            return new OrExpression();
        }

        public static NotExpression Not()
        {
            return  new NotExpression();
        }

        public static IfThen IfThen()
        {
            return new IfThen();
        }

        #endregion

        #region Factory : Evaluable Expressions

        public static  EqualToExpression EqualToGeneric(Func<object> leftValueProvider, Func<object> rightValueProvider)
        {
            return new EqualToExpression(leftValueProvider, rightValueProvider);
        }

        public static EqualToExpression EqualToValue(IViewModelProperty property, object value)
        {
            var leftValueProvider = new Func<object>(property.GetValue);
            var rightValueProvider = new Func<object>(() => value);
            return EqualToGeneric(leftValueProvider, rightValueProvider);
        }

        public static EqualToExpression EqualToProperty(IViewModelProperty property, IViewModelProperty valueProperty)
        {
            var leftValueProvider = new Func<object>(property.GetValue);
            var rightValueProvider = new Func<object>(valueProperty.GetValue);
            return EqualToGeneric(leftValueProvider, rightValueProvider);
        }

        public static EqualToExpression EqualToLateValue(IViewModelProperty property, LateValue lateValue)
        {
            var leftValueProvider = new Func<object>(property.GetValue);
            var rightValueProvider = new Func<object>(() => lateValue());
            return EqualToGeneric(leftValueProvider, rightValueProvider);
        }

        public static GreaterThanExpression GreaterThanGeneric(Func<object> leftValueProvider, Func<object> rightValueProvider)
        {
            return new GreaterThanExpression(leftValueProvider, rightValueProvider);
        }

        public static GreaterThanExpression GreaterThanValue(IViewModelProperty property, object value)
        {
            var leftValueProvider = new Func<object>(property.GetValue);
            var rightValueProvider = new Func<object>(() => value);
            return GreaterThanGeneric(leftValueProvider, rightValueProvider);
        }

        public static GreaterThanExpression GreaterThanProperty(IViewModelProperty property, IViewModelProperty valueProperty)
        {
            var leftValueProvider = new Func<object>(property.GetValue);
            var rightValueProvider = new Func<object>(valueProperty.GetValue);
            return GreaterThanGeneric(leftValueProvider, rightValueProvider);
        }

        public static GreaterThanExpression GreaterThanLateValue(IViewModelProperty property, LateValue lateValue)
        {
            var leftValueProvider = new Func<object>(property.GetValue);
            var rightValueProvider = new Func<object>(() => lateValue());
            return GreaterThanGeneric(leftValueProvider, rightValueProvider);
        }

        public static LessThanExpression LessThanGeneric(Func<object> leftValueProvider, Func<object> rightValueProvider)
        {
            return new LessThanExpression(leftValueProvider, rightValueProvider);
        }

        public static LessThanExpression LessThanValue(IViewModelProperty property, object value)
        {
            var leftValueProvider = new Func<object>(property.GetValue);
            var rightValueProvider = new Func<object>(() => value);
            return LessThanGeneric(leftValueProvider, rightValueProvider);
        }

        public static LessThanExpression LessThanProperty(IViewModelProperty property, IViewModelProperty valueProperty)
        {
            var leftValueProvider = new Func<object>(property.GetValue);
            var rightValueProvider = new Func<object>(valueProperty.GetValue);
            return LessThanGeneric(leftValueProvider, rightValueProvider);
        }

        public static LessThanExpression LessThanLateValue(IViewModelProperty property, LateValue lateValue)
        {
            var leftValueProvider = new Func<object>(property.GetValue);
            var rightValueProvider = new Func<object>(() => lateValue());
            return LessThanGeneric(leftValueProvider, rightValueProvider);
        }

        public static MatchingExpression MatchingGeneric(Func<object> leftValueProvider, string pattern)
        {
            return new MatchingExpression(leftValueProvider, pattern);
        }

        public static MatchingExpression MatchingProperty(IViewModelProperty property, string pattern)
        {
            var evaluatedValueProvider = new Func<object>(property.GetValue);
            return MatchingGeneric(evaluatedValueProvider, pattern);
        }

        public static CustomValidationExpression CustomValidation(IViewModelProperty property, CustomValidator customValidator)
        {
            var leftValueProvider = new Func<object>(property.GetValue);
            return new CustomValidationExpression(leftValueProvider, customValidator);
        }

        #endregion
    }

    #region Parenthesis

    /// <summary>
    /// Reprensents parenthesis into the expression.
    /// </summary>
    internal abstract class ParenthesisExpression : ExpressionNode
    {
    }

    /// <summary>
    /// Reprensents left parenthesis.
    /// </summary>
    internal class OpenParenthesis : ParenthesisExpression
    {
        public override Task<bool> Evaluate(CancellationToken cancellationToken)
        {
            throw new InvalidOperationException();
        }
    }

    /// <summary>
    /// Reprensents right parenthesis.
    /// </summary>
    internal class CloseParenthesis : ParenthesisExpression
    {
        public override Task<bool> Evaluate(CancellationToken cancellationToken)
        {
            throw new InvalidOperationException();
        }
    }

    #endregion

    #region Constants

    /// <summary>
    /// Reprensents constants into the expression.
    /// </summary>
    internal abstract class ConstantExpression : ExpressionNode
    {
    }

    /// <summary>
    /// Reprensents True boolean constant.
    /// </summary>
    internal class TrueExpression : ConstantExpression
    {
        public override Task<bool> Evaluate(CancellationToken cancellationToken)
        {
            return new Task<bool>(() => true, cancellationToken, TaskCreationOptions.AttachedToParent);
        }
    }

    /// <summary>
    /// Reprensents False boolean constant.
    /// </summary>
    internal class FalseExpression : ConstantExpression
    {
        public override Task<bool> Evaluate(CancellationToken cancellationToken)
        {
            return new Task<bool>(() => false, cancellationToken, TaskCreationOptions.AttachedToParent);
        }
    }

    #endregion

    #region Operators

    /// <summary>
    /// Expression that represents an operator.
    /// </summary>
    internal abstract class OperatorExpression : ExpressionNode
    {
        public abstract Associativity Associativity { get; }
        public abstract OperatorType OperatorType { get; }
        public byte Precedence { get { return OperatorPrecedence.GetPrecedence(this); }}

        public ExpressionNode Left { get; set; }
        public ExpressionNode Right { get; set; }

        protected void EnsureBinaryOperandsInitialized()
        {
            if (Left == null || Right == null)
                throw new InvalidOperationException("Left and Right operands have to be initialized");
        }

        protected void EnsureUnaryOperandInitialized()
        {
            if (Left == null)
                throw new InvalidOperationException("Left operand have to be initialized");
        }
    }

    /// <summary>
    /// Logical "And" operator.
    /// </summary>
    internal class AndExpression : OperatorExpression
    {
        public override Associativity Associativity
        {
            get { return Associativity.Left; }
        }

        public override OperatorType OperatorType
        {
            get { return OperatorType.Binary; }
        }

        public override Task<bool> Evaluate(CancellationToken cancellationToken)
        {
            EnsureBinaryOperandsInitialized();

            return new Task<bool>(() =>
                                    {
                                        var leftTask = Left.Evaluate(cancellationToken);
                                        var rightTask = Right.Evaluate(cancellationToken);

                                        leftTask.Start();
                                        rightTask.Start();
                
                                        return leftTask.Result && rightTask.Result;
                                    }, cancellationToken, TaskCreationOptions.AttachedToParent);
        }
    }

    /// <summary>
    /// Logical "Or" operator.
    /// </summary>
    internal class OrExpression : OperatorExpression
    {
        public override Associativity Associativity
        {
            get { return Associativity.Left; }
        }

        public override OperatorType OperatorType
        {
            get { return OperatorType.Binary; }
        }

        public override Task<bool> Evaluate(CancellationToken cancellationToken)
        {
            EnsureBinaryOperandsInitialized();

            return new Task<bool>(() =>
                                    {
                                        var leftTask = Left.Evaluate(cancellationToken);
                                        var rightTask = Right.Evaluate(cancellationToken);

                                        leftTask.Start();
                                        rightTask.Start();

                                        return leftTask.Result || rightTask.Result;
                                    }, cancellationToken, TaskCreationOptions.AttachedToParent);
        }
    }

    /// <summary>
    /// Logical "Not" operator.
    /// </summary>
    internal class NotExpression : OperatorExpression
    {
        public override Associativity Associativity
        {
            get { return Associativity.Right; }
        }

        public override OperatorType OperatorType
        {
            get { return OperatorType.Unary; }
        }

        public override Task<bool> Evaluate(CancellationToken cancellationToken)
        {
            EnsureUnaryOperandInitialized();

            return new Task<bool>(() =>
                                    {
                                        var leftTask = Left.Evaluate(cancellationToken);
                                        leftTask.Start();

                                        return !leftTask.Result;
                                    }, cancellationToken, TaskCreationOptions.AttachedToParent);
        }
    }

    /// <summary>
    /// "If Then" operator.
    /// </summary>
    internal class IfThen : OperatorExpression
    {
        public override Associativity Associativity
        {
            get { return Associativity.Right;}
        }

        public override OperatorType OperatorType
        {
            get { return OperatorType.Binary; }
        }

        public override Task<bool> Evaluate(CancellationToken cancellationToken)
        {
            EnsureBinaryOperandsInitialized();

            return new Task<bool>(() =>
                                    {
                                        var conditionTask = Left.Evaluate(cancellationToken);
                                        if(conditionTask.Result)
                                        {
                                            var rightTask = Right.Evaluate(cancellationToken);
                                            rightTask.Start();
                                            return rightTask.Result;
                                        }

                                        return true;
                                    }, cancellationToken, TaskCreationOptions.AttachedToParent);
        }
    }

    #endregion

    /// <summary>
    /// Represents an expression that can be evaluated.
    /// </summary>
    internal abstract class EvaluableExpression : ExpressionNode
    {
        private readonly Func<object> _leftValueProvider;

        /// <summary>
        /// Gets the value that will be evaluated.
        /// </summary>
        public object LeftValue
        {
            get { return _leftValueProvider != null ? _leftValueProvider() : null; }
        }

        protected EvaluableExpression(Func<object> leftValueProvider)
        {
            _leftValueProvider = leftValueProvider;
        }

        public override Task<bool> Evaluate(CancellationToken cancellationToken)
        {
            return new Task<bool>(() => true, TaskCreationOptions.AttachedToParent);
        }
    }

    /// <summary>
    /// Defines associativity of operators.
    /// </summary>
    internal enum Associativity
    {
        Left,
        Right
    }

    /// <summary>
    /// Defines the type of operators.
    /// </summary>
    internal enum OperatorType
    {
        Unary,
        Binary
    }

    /// <summary>
    /// Defines operator precedence.
    /// </summary>
    internal static class OperatorPrecedence
    {
        public static byte GetPrecedence(OperatorExpression @operator)
        {
            if (@operator is NotExpression) return 4;
            if (@operator is AndExpression) return 3;
            if (@operator is OrExpression) return 2;
            if (@operator is IfThen) return 1;

            throw new InvalidOperationException("Unknown operator " + @operator.GetType().Name);
        }
    }
}
