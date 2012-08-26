using System;
using System.Threading.Tasks;

namespace GasyTek.Lakana.Mvvm.Validation.Fluent
{
    /// <summary>
    /// Base class for expressions.
    /// </summary>
    internal abstract class ExpressionNode
    {
        public abstract Task<bool> Evaluate();
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
    internal class LeftParenthesis : ParenthesisExpression
    {
        public override Task<bool> Evaluate()
        {
            throw new InvalidOperationException();
        }
    }

    /// <summary>
    /// Reprensents right parenthesis.
    /// </summary>
    internal class RightParenthesis : ParenthesisExpression
    {
        public override Task<bool> Evaluate()
        {
            throw new InvalidOperationException();
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

        public override Task<bool> Evaluate()
        {
            EnsureBinaryOperandsInitialized();

            return new Task<bool>(() =>
            {
                var leftTask = Left.Evaluate();
                var rightTask = Right.Evaluate();

                leftTask.Start();
                rightTask.Start();
                
                return leftTask.Result && rightTask.Result;
            });
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

        public override Task<bool> Evaluate()
        {
            EnsureBinaryOperandsInitialized();

            return new Task<bool>(() =>
            {
                var leftTask = Left.Evaluate();
                var rightTask = Right.Evaluate();

                leftTask.Start();
                rightTask.Start();
                
                return leftTask.Result || rightTask.Result;
            });
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

        public override Task<bool> Evaluate()
        {
            EnsureUnaryOperandInitialized();

            return new Task<bool>(() =>
            {
                var leftTask = Left.Evaluate();
                leftTask.Start();

                return !leftTask.Result;
            });
        }
    }

    /// <summary>
    /// "If Then Else" operator.
    /// </summary>
    internal class ConditionalOperator : OperatorExpression
    {
        public ExpressionNode Condition { get; set; }

        public override Associativity Associativity
        {
            get { return Associativity.Right;}
        }

        public override OperatorType OperatorType
        {
            get { return OperatorType.Ternary; }
        }

        public override Task<bool> Evaluate()
        {
            EnsureTernaryOperandsInitialized();

            return new Task<bool>(() =>
            {
                var conditionTask = Condition.Evaluate();
                if(conditionTask.Result)
                {
                    var leftTask = Left.Evaluate();
                    leftTask.Start();
                    return leftTask.Result;
                }
                else
                {
                    var rightTask = Right.Evaluate();
                    rightTask.Start();
                    return rightTask.Result;
                }
            });
        }

        protected void EnsureTernaryOperandsInitialized()
        {
            if (Condition == null || Left == null || Right == null)
                throw new InvalidOperationException("Condition, Left and Right operands have to be initialized");
        }
    }

    #endregion

    /// <summary>
    /// Represents an expression that can be evaluated.
    /// </summary>
    internal abstract class EvaluableExpression : ExpressionNode
    {
        private readonly Func<object> _evaluatedValueProvider;

        /// <summary>
        /// Gets the value that will be evaluated.
        /// </summary>
        public object EvaluatedValue
        {
            get { return _evaluatedValueProvider != null ? _evaluatedValueProvider() : null; }
        }

        protected EvaluableExpression(Func<object> evaluatedValueProvider)
        {
            _evaluatedValueProvider = evaluatedValueProvider;
        }

        public override Task<bool> Evaluate()
        {
            return new Task<bool>(() => true);
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
        Binary,
        Ternary
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
            if (@operator is ConditionalOperator) return 1;

            throw new InvalidOperationException("Unknown operator " + @operator.GetType().Name);
        }
    }
}
