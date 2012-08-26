using System;
using System.Collections.Generic;
using System.Linq;

namespace GasyTek.Lakana.Mvvm.Validation.Fluent
{
    /// <summary>
    /// Operators precedence and associativity follow rules defined at : http://msdn.microsoft.com/en-us/library/aa691310%28v=vs.71%29.aspx.
    /// </summary>
    internal class Parser
    {
        /// <summary>
        /// Parses the specified tokens and produces an abstract syntax tree of expressions that can be evaluated.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <returns></returns>
        internal ExpressionNode Parse(IEnumerable<ExpressionNode> tokens)
        {
            var tokenList = tokens.ToList();

            if (tokenList.Count == 1) return tokenList[0];

            var stack = new Stack<ExpressionNode>();

            try
            {
                // infix to postfix to make the expression more easy to manipulate
                var inputTokens = TransformInfixToPostfix(tokenList);

                foreach (var iToken in inputTokens)
                {
                    if (iToken is EvaluableExpression)
                    {
                        stack.Push(iToken);
                    }
                    else
                    {
                        // the input token must be an OperatorToken
                        var opExpression = (OperatorExpression)iToken;
                        if (opExpression.OperatorType == OperatorType.Unary)
                            opExpression.Left = stack.Pop();
                        else
                        {
                            opExpression.Left = stack.Pop();
                            opExpression.Right = stack.Pop();
                        }

                        stack.Push(opExpression);
                    }
                }

                if (stack.Count != 1)
                    throw new InvalidOperationException("The parsing stack contains more or less than 1 item.");
            }
            catch (Exception e)
            {
                throw new ParseException(e);
            }

            return stack.Peek();
        }

        /// <summary>
        /// Transforms the infix notation of the expression to its correspondant postfix notation (Reverse Polish Notation).
        /// </summary>
        /// <remarks>Uses Djikstra's "Shunting Yard algorithm".</remarks>
        /// <param name="tokens">The input notation in the form of enumerable of tokens</param>
        /// <returns></returns>
        internal IEnumerable<ExpressionNode> TransformInfixToPostfix(IEnumerable<ExpressionNode> tokens)
        {
            if (tokens == null) throw new ArgumentException();

            var inputTokens = tokens.ToList();
            var outputTokens = new List<ExpressionNode>();
            var stack = new Stack<ExpressionNode>();

            foreach (var iToken in inputTokens)
            {
                if (iToken is ParenthesisExpression)
                {
                    if (iToken is LeftParenthesis) stack.Push(iToken);
                    else
                    {
                        while (stack.Count > 0)
                        {
                            var currentToken = stack.Peek() as LeftParenthesis;
                            if (currentToken == null)
                                outputTokens.Add(stack.Pop());
                            else
                            {
                                stack.Pop();    // pop the right parenthesis
                                break;
                            }
                        }
                    }
                }
                else if (iToken is OperatorExpression)
                {
                    var inputOpToken = (OperatorExpression)iToken;

                    while (stack.Count > 0)
                    {
                        var currentToken = stack.Peek() as OperatorExpression;
                        if (currentToken != null && ShouldPopStack(inputOpToken, currentToken))
                            outputTokens.Add(stack.Pop());
                        else break;
                    }

                    stack.Push(inputOpToken);
                }
                else
                {
                    // the input token must be an EvaluableToken
                    outputTokens.Add(iToken);
                }
            }

            // empty the stack
            while (stack.Count > 0)
            {
                outputTokens.Add(stack.Pop());
            }

            return outputTokens;
        }

        private bool ShouldPopStack(OperatorExpression inputOpToken, OperatorExpression currentOpToken)
        {
            if (inputOpToken.Associativity == Associativity.Left
                && inputOpToken.Precedence <= currentOpToken.Precedence) return true;
            if (inputOpToken.Associativity == Associativity.Right
                && inputOpToken.Precedence < currentOpToken.Precedence) return true;
            return false;
        }
    }

    /// <summary>
    /// Thrown when there is error during parsing process.
    /// </summary>
    internal class ParseException : ApplicationException
    {
        public ParseException(Exception innerException)
            : base("Error during parsing parsing process", innerException)
        {
            
        }
    }
}
