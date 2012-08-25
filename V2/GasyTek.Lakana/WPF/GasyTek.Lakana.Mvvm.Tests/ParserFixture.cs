using System;
using System.Collections.Generic;
using System.Linq;
using GasyTek.Lakana.Mvvm.Tests.Fakes;
using GasyTek.Lakana.Mvvm.Validation.Fluent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasyTek.Lakana.Mvvm.Tests
{
    [TestClass]
    public class ParserFixture
    {
        private IFluentProperty<FakeEditableViewModel> _fluentProperty;
        private List<ExpressionNode> _tokens;
        private Parser _parser;
        private FakeEditableViewModel _fakeEditableViewModel;

        [TestInitialize]
        public void OnSetup()
        {
            _fakeEditableViewModel = new FakeEditableViewModel { Model = new Product { Code = "PR001", Quantity = 20} };
            _fluentProperty = new FluentImplementer<FakeEditableViewModel>(_fakeEditableViewModel);
            _tokens = ((FluentImplementer<FakeEditableViewModel>)_fluentProperty).InternalTokens;
            _parser = new Parser();
        }

        [TestClass]
        public class TranslateInfixToPostfix : ParserFixture
        {
            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void NullExpressionIsNotAllowed()
            {
                // Prepare

                // Act
                _parser.TransformInfixToPostfix(null);

                // Verify
            }

            [TestMethod]
            public void CanTranslateExpressionWithSingleOperand()
            {
                // Prepare
                var x = new FakeEvaluableExpression();
                _tokens.Add(x);

                // Act
                var result = _parser.TransformInfixToPostfix(_tokens).ToList();

                // Verify
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count() == 1);
                Assert.IsTrue(result.First() == x);
            }

            [TestMethod]
            public void CanTranslateExpressionWithUnaryOperator()
            {
                // Prepare (NOT x)
                var x = new FakeEvaluableExpression();
                var not = new NotExpression();
                _tokens.Add(not);
                _tokens.Add(x);

                // Act
                var result = _parser.TransformInfixToPostfix(_tokens).ToList();

                // Verify
                Assert.AreSame(x, result[0]);
                Assert.AreSame(not, result[1]);
            }

            [TestMethod]
            public void CanTranslateExpressionWithManyUnaryOperator()
            {
                // Prepare
                var x = new FakeEvaluableExpression();
                var not1 = new NotExpression();
                var not2 = new NotExpression();
                _tokens.Add(not1);
                _tokens.Add(not2);
                _tokens.Add(x);

                // Act
                var result = _parser.TransformInfixToPostfix(_tokens).ToList();

                // Verify
                Assert.AreSame(x, result[0]);
                Assert.AreSame(not2, result[1]);
                Assert.AreSame(not1, result[2]);
            }

            [TestMethod]
            public void CanTranslateExpressionWithBinaryOperator()
            {
                // Prepare (x AND y)
                var x = new FakeEvaluableExpression();
                var y = new FakeEvaluableExpression();
                var and = new AndExpression();
                _tokens.Add(x);
                _tokens.Add(and);
                _tokens.Add(y);

                // Act
                var result = _parser.TransformInfixToPostfix(_tokens).ToList();

                // Verify
                Assert.AreSame(x, result[0]);
                Assert.AreSame(y, result[1]);
                Assert.AreSame(and, result[2]);
            }

            [TestMethod]
            public void CanTranslateExpressionWithManyBinaryOperators()
            {
                // Prepare (x AND y AND z)
                var x = new FakeEvaluableExpression();
                var y = new FakeEvaluableExpression();
                var z = new FakeEvaluableExpression();
                var and1 = new AndExpression();
                var and2 = new AndExpression();
                _tokens.Add(x);
                _tokens.Add(and1);
                _tokens.Add(y);
                _tokens.Add(and2);
                _tokens.Add(z);

                // Act
                var result = _parser.TransformInfixToPostfix(_tokens).ToList();

                // Verify
                Assert.AreSame(x, result[0]);
                Assert.AreSame(y, result[1]);
                Assert.AreSame(and1, result[2]);
                Assert.AreSame(z, result[3]);
                Assert.AreSame(and2, result[4]);
            }

            [TestMethod]
            public void CanTranslateExpressionWithManyBinaryOperatorsThatHaveDifferentPrecendence()
            {
                // Prepare (x OR y AND z)
                var x = new FakeEvaluableExpression();
                var y = new FakeEvaluableExpression();
                var z = new FakeEvaluableExpression();
                var or = new OrExpression();
                var and = new AndExpression();
                _tokens.Add(x);
                _tokens.Add(or);
                _tokens.Add(y);
                _tokens.Add(and);
                _tokens.Add(z);

                // Act
                var result = _parser.TransformInfixToPostfix(_tokens).ToList();

                // Verify
                Assert.AreSame(x, result[0]);
                Assert.AreSame(y, result[1]);
                Assert.AreSame(z, result[2]);
                Assert.AreSame(and, result[3]);
                Assert.AreSame(or, result[4]);
            }

            [TestMethod]
            public void CanTranslateExpressionWithUnaryAndBinaryOperators()
            {
                // Prepare
                var x = new FakeEvaluableExpression();
                var y = new FakeEvaluableExpression();
                var z = new FakeEvaluableExpression();
                var not = new NotExpression();
                var and1 = new AndExpression();
                var and2 = new AndExpression();
                _tokens.Add(x);
                _tokens.Add(and1);
                _tokens.Add(not);
                _tokens.Add(y);
                _tokens.Add(and2);
                _tokens.Add(z);

                // Act
                var result = _parser.TransformInfixToPostfix(_tokens).ToList();

                // Verify
                Assert.AreSame(x, result[0]);
                Assert.AreSame(y, result[1]);
                Assert.AreSame(not, result[2]);
                Assert.AreSame(and1, result[3]);
                Assert.AreSame(z, result[4]);
                Assert.AreSame(and2, result[5]);
            }

            [TestMethod]
            public void CanTranslateExpressionWithParenthesis()
            {
                // Prepare (x)
                var x = new FakeEvaluableExpression();
                var lp = new LeftParenthesis();
                var rp = new RightParenthesis();
                _tokens.Add(lp);
                _tokens.Add(x);
                _tokens.Add(rp);

                // Act
                var result = _parser.TransformInfixToPostfix(_tokens).ToList();

                // Verify
                Assert.IsTrue(result.Count == 1);
                Assert.AreSame(x, result[0]);
            }

            [TestMethod]
            public void CanTranslateExpressionWithParenthesisAndUnaryOperator()
            {
                // Prepare (NOT x)
                var x = new FakeEvaluableExpression();
                var not = new NotExpression();
                var lp = new LeftParenthesis();
                var rp = new RightParenthesis();
                _tokens.Add(lp);
                _tokens.Add(not);
                _tokens.Add(x);
                _tokens.Add(rp);

                // Act
                var result = _parser.TransformInfixToPostfix(_tokens).ToList();

                // Verify
                Assert.IsTrue(result.Count == 2);
                Assert.AreSame(x, result[0]);
                Assert.AreSame(not, result[1]);
            }

            [TestMethod]
            public void CanTranslateExpressionWithParenthesisAndBinaryOperator()
            {
                // Prepare (x AND y)
                var x = new FakeEvaluableExpression();
                var y = new FakeEvaluableExpression();
                var and = new AndExpression();
                var lp = new LeftParenthesis();
                var rp = new RightParenthesis();
                _tokens.Add(lp);
                _tokens.Add(x);
                _tokens.Add(and);
                _tokens.Add(y);
                _tokens.Add(rp);

                // Act
                var result = _parser.TransformInfixToPostfix(_tokens).ToList();

                // Verify
                Assert.IsTrue(result.Count == 3);
                Assert.AreSame(x, result[0]);
                Assert.AreSame(y, result[1]);
                Assert.AreSame(and, result[2]);
            }

            [TestMethod]
            public void CanTranslateExpressionUsingParenthesisPrecedence()
            {
                // Prepare (x OR y) AND z
                var x = new FakeEvaluableExpression();
                var y = new FakeEvaluableExpression();
                var z = new FakeEvaluableExpression();
                var and = new AndExpression();
                var or = new OrExpression();
                var lp = new LeftParenthesis();
                var rp = new RightParenthesis();
                _tokens.Add(lp);
                _tokens.Add(x);
                _tokens.Add(or);
                _tokens.Add(y);
                _tokens.Add(rp);
                _tokens.Add(and);
                _tokens.Add(z);

                // Act
                var result = _parser.TransformInfixToPostfix(_tokens).ToList();

                // Verify
                Assert.IsTrue(result.Count == 5);
                Assert.AreSame(x, result[0]);
                Assert.AreSame(y, result[1]);
                Assert.AreSame(or, result[2]);   
                Assert.AreSame(z, result[3]);
                Assert.AreSame(and, result[4]);
            }
        }

        [TestClass]
        public class Parse : ParserFixture
        {
            [TestMethod]
            public void CanExtractTokens()
            {
                // Prepare

                // Act
                _fluentProperty.Property(vm => vm.Quantity).Is.EqualTo(20);

                // Verify
                Assert.IsNotNull(_tokens);
                Assert.IsTrue(_tokens.Count == 1);
            }

            [TestMethod]
            public void CanInterpretSyntaxToExpression()
            {
                // Prepare
                _fluentProperty.Property(vm => vm.Quantity).Is.EqualTo(20);

                // Act
                var expressionBase = _parser.Parse(_tokens);

                // Verify
                Assert.IsNotNull(expressionBase);
            }

            [TestMethod]
            public void CanInterpretSimpleRule()
            {
                // Prepare
                _fluentProperty.Property(vm => vm.Quantity).Is.EqualTo(10);

                // Act
                var expressionBase = _parser.Parse(_tokens);
                var taskResult = expressionBase.Evaluate();
                taskResult.Start();

                // Verify
                Assert.IsInstanceOfType(expressionBase, typeof(EqualToExpression));
                Assert.IsFalse(taskResult.Result);
            }

            [TestMethod]
            public void CanInterpretRuleWithUnaryOperator()
            {
                // Prepare
                _fluentProperty.Property(vm => vm.Quantity).IsNot.EqualTo(10);

                // Act
                var expressionBase = _parser.Parse(_tokens);
                var taskResult = expressionBase.Evaluate();
                taskResult.Start();

                // Verify
                Assert.IsInstanceOfType(expressionBase, typeof(NotExpression));
                Assert.IsInstanceOfType(((NotExpression)expressionBase).Left, typeof(EqualToExpression));
                Assert.IsTrue(taskResult.Result);
            }

            [TestMethod]
            public void CanInterpretRuleWithAndOperator()
            {
                // Prepare
                _fluentProperty.Property(vm => vm.Quantity).IsNot.EqualTo(10).And.Is.EqualTo(20);

                // Act
                var expressionBase = _parser.Parse(_tokens);
                var taskResult = expressionBase.Evaluate();
                taskResult.Start();

                // Verify
                Assert.IsInstanceOfType(expressionBase, typeof(AndExpression));
                Assert.IsTrue(taskResult.Result);
            }

            [TestMethod]
            public void CanInterpretRuleWithManyAndOperator()
            {
                // Prepare
                _fluentProperty.
                    Property(vm => vm.Quantity).
                        IsNot.EqualTo(10).
                        And.Is.EqualTo(20).
                        And.IsNot.EqualTo(5);

                // Act
                var expressionBase = _parser.Parse(_tokens);
                var taskResult = expressionBase.Evaluate();
                taskResult.Start();

                // Verify
                Assert.IsInstanceOfType(expressionBase, typeof(AndExpression));
                Assert.IsTrue(taskResult.Result);
            }

            [TestMethod]
            public void CanInterpretRuleWithOrOperator()
            {
                // Prepare
                _fluentProperty.Property(vm => vm.Quantity).IsNot.EqualTo(20).Or.Is.EqualTo(5);

                // Act
                var expressionBase = _parser.Parse(_tokens);
                var taskResult = expressionBase.Evaluate();
                taskResult.Start();

                // Verify
                Assert.IsInstanceOfType(expressionBase, typeof(OrExpression));
                Assert.IsFalse(taskResult.Result);
            }

            [TestMethod]
            public void CanInterpretRuleWithManyOrOperator()
            {
                // Prepare
                _fluentProperty.
                    Property(vm => vm.Quantity).
                        IsNot.EqualTo(20).
                        Or.Is.EqualTo(10).
                        Or.Is.EqualTo(5);

                // Act
                var expressionBase = _parser.Parse(_tokens);
                var taskResult = expressionBase.Evaluate();
                taskResult.Start();

                // Verify
                Assert.IsInstanceOfType(expressionBase, typeof(OrExpression));
                Assert.IsFalse(taskResult.Result);
            }

            [TestMethod]
            public void CanInterpretUsingOperatorPrecedence()
            {
                // Prepare
                _fluentProperty.Property(vm => vm.Quantity).Is.EqualTo(5).Or.IsNot.EqualTo(5).And.Is.EqualTo(6);

                // Act
                var expressionBase = _parser.Parse(_tokens);
                var taskResult = expressionBase.Evaluate();
                taskResult.Start();

                // Verify
                Assert.IsInstanceOfType(expressionBase, typeof(OrExpression));
                Assert.IsFalse(taskResult.Result);
            }
        }
    }
}
