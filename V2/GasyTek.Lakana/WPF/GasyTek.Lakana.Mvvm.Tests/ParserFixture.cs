using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GasyTek.Lakana.Mvvm.Tests.Fakes;
using GasyTek.Lakana.Mvvm.Validation.Fluent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasyTek.Lakana.Mvvm.Tests
{
    [TestClass]
    public class ParserFixture
    {
        private const string FakeEditableViewModelProperty = "FakeEditableViewModelProperty";
        private const string ParserProperty = "ParserProperty";
        private const string FluentApiProperty = "FluentApiProperty";
        private const string TokensProperty = "TokensProperty";

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void OnSetup()
        {
            var fakeEditableViewModel = new FakeEditableViewModel { Model = new Product { Code = "PR001", Quantity = 20} };
            var fluentApi = new FluentImplementer<FakeEditableViewModel>(fakeEditableViewModel);
            var tokens = fluentApi.InternalTokens;
            var parser = new Parser();

            TestContext.Properties.Add(FakeEditableViewModelProperty, fakeEditableViewModel);
            TestContext.Properties.Add(FluentApiProperty, fluentApi);
            TestContext.Properties.Add(TokensProperty, tokens);
            TestContext.Properties.Add(ParserProperty, parser);
        }

        #region Helper methods

        private IFluentProperty<FakeEditableViewModel> FluentApi
        {
            get { return (FluentImplementer<FakeEditableViewModel>)TestContext.Properties[FluentApiProperty]; }
        }

        private Parser Parser
        {
            get { return (Parser)TestContext.Properties[ParserProperty]; }
        }

        private List<ExpressionNode> Tokens
        {
            get { return (List<ExpressionNode>)TestContext.Properties[TokensProperty]; }
        }

        #endregion

        [TestClass]
        public class TranslateInfixToPostfix : ParserFixture
        {
            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void NullExpressionIsNotAllowed()
            {
                // Prepare

                // Act
                Parser.TransformInfixToPostfix(null);

                // Verify
            }

            [TestMethod]
            public void CanTranslateExpressionWithSingleOperand()
            {
                // Prepare
                var x = new FakeEvaluableExpression();
                Tokens.Add(x);

                // Act
                var result = Parser.TransformInfixToPostfix(Tokens).ToList();

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
                Tokens.Add(not);
                Tokens.Add(x);

                // Act
                var result = Parser.TransformInfixToPostfix(Tokens).ToList();

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
                Tokens.Add(not1);
                Tokens.Add(not2);
                Tokens.Add(x);

                // Act
                var result = Parser.TransformInfixToPostfix(Tokens).ToList();

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
                Tokens.Add(x);
                Tokens.Add(and);
                Tokens.Add(y);

                // Act
                var result = Parser.TransformInfixToPostfix(Tokens).ToList();

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
                Tokens.Add(x);
                Tokens.Add(and1);
                Tokens.Add(y);
                Tokens.Add(and2);
                Tokens.Add(z);

                // Act
                var result = Parser.TransformInfixToPostfix(Tokens).ToList();

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
                Tokens.Add(x);
                Tokens.Add(or);
                Tokens.Add(y);
                Tokens.Add(and);
                Tokens.Add(z);

                // Act
                var result = Parser.TransformInfixToPostfix(Tokens).ToList();

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
                Tokens.Add(x);
                Tokens.Add(and1);
                Tokens.Add(not);
                Tokens.Add(y);
                Tokens.Add(and2);
                Tokens.Add(z);

                // Act
                var result = Parser.TransformInfixToPostfix(Tokens).ToList();

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
                var lp = new OpenParenthesis();
                var rp = new CloseParenthesis();
                Tokens.Add(lp);
                Tokens.Add(x);
                Tokens.Add(rp);

                // Act
                var result = Parser.TransformInfixToPostfix(Tokens).ToList();

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
                var lp = new OpenParenthesis();
                var rp = new CloseParenthesis();
                Tokens.Add(lp);
                Tokens.Add(not);
                Tokens.Add(x);
                Tokens.Add(rp);

                // Act
                var result = Parser.TransformInfixToPostfix(Tokens).ToList();

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
                var lp = new OpenParenthesis();
                var rp = new CloseParenthesis();
                Tokens.Add(lp);
                Tokens.Add(x);
                Tokens.Add(and);
                Tokens.Add(y);
                Tokens.Add(rp);

                // Act
                var result = Parser.TransformInfixToPostfix(Tokens).ToList();

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
                var lp = new OpenParenthesis();
                var rp = new CloseParenthesis();
                Tokens.Add(lp);
                Tokens.Add(x);
                Tokens.Add(or);
                Tokens.Add(y);
                Tokens.Add(rp);
                Tokens.Add(and);
                Tokens.Add(z);

                // Act
                var result = Parser.TransformInfixToPostfix(Tokens).ToList();

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
                FluentApi.Property(vm => vm.Quantity).Is.EqualTo(20);

                // Verify
                Assert.IsNotNull(Tokens);
                Assert.IsTrue(Tokens.Count == 1);
            }

            [TestMethod]
            public void CanInterpretSyntaxToExpression()
            {
                // Prepare
                FluentApi.Property(vm => vm.Quantity).Is.EqualTo(20);

                // Act
                var expressionBase = Parser.Parse(Tokens);

                // Verify
                Assert.IsNotNull(expressionBase);
            }

            [TestMethod]
            public void CanInterpretSimpleRule()
            {
                // Prepare
                FluentApi.Property(vm => vm.Quantity).Is.EqualTo(10);

                // Act
                var expressionBase = Parser.Parse(Tokens);
                var taskResult = expressionBase.Evaluate(new CancellationToken());
                taskResult.Start();

                // Verify
                Assert.IsInstanceOfType(expressionBase, typeof(EqualToExpression));
                Assert.IsFalse(taskResult.Result);
            }

            [TestMethod]
            public void CanInterpretRuleWithUnaryOperator()
            {
                // Prepare
                FluentApi.Property(vm => vm.Quantity).IsNot.EqualTo(10);

                // Act
                var expressionBase = Parser.Parse(Tokens);
                var taskResult = expressionBase.Evaluate(new CancellationToken());
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
                FluentApi.Property(vm => vm.Quantity).IsNot.EqualTo(10).And.Is.EqualTo(20);

                // Act
                var expressionBase = Parser.Parse(Tokens);
                var taskResult = expressionBase.Evaluate(new CancellationToken());
                taskResult.Start();

                // Verify
                Assert.IsInstanceOfType(expressionBase, typeof(AndExpression));
                Assert.IsTrue(taskResult.Result);
            }

            [TestMethod]
            public void CanInterpretRuleWithManyAndOperator()
            {
                // Prepare
                FluentApi.
                    Property(vm => vm.Quantity).
                        IsNot.EqualTo(10).
                        And.Is.EqualTo(20).
                        And.IsNot.EqualTo(5);

                // Act
                var expressionBase = Parser.Parse(Tokens);
                var taskResult = expressionBase.Evaluate(new CancellationToken());
                taskResult.Start();

                // Verify
                Assert.IsInstanceOfType(expressionBase, typeof(AndExpression));
                Assert.IsTrue(taskResult.Result);
            }

            [TestMethod]
            public void CanInterpretRuleWithOrOperator()
            {
                // Prepare
                FluentApi.Property(vm => vm.Quantity).IsNot.EqualTo(20).Or.Is.EqualTo(5);

                // Act
                var expressionBase = Parser.Parse(Tokens);
                var taskResult = expressionBase.Evaluate(new CancellationToken());
                taskResult.Start();

                // Verify
                Assert.IsInstanceOfType(expressionBase, typeof(OrExpression));
                Assert.IsFalse(taskResult.Result);
            }

            [TestMethod]
            public void CanInterpretRuleWithManyOrOperator()
            {
                // Prepare
                FluentApi.
                    Property(vm => vm.Quantity).
                        IsNot.EqualTo(20).
                        Or.Is.EqualTo(10).
                        Or.Is.EqualTo(5);

                // Act
                var expressionBase = Parser.Parse(Tokens);
                var taskResult = expressionBase.Evaluate(new CancellationToken());
                taskResult.Start();

                // Verify
                Assert.IsInstanceOfType(expressionBase, typeof(OrExpression));
                Assert.IsFalse(taskResult.Result);
            }

            [TestMethod]
            public void CanInterpretUsingOperatorPrecedence()
            {
                // Prepare
                FluentApi.Property(vm => vm.Quantity).Is.EqualTo(5).Or.IsNot.EqualTo(5).And.Is.EqualTo(6);

                // Act
                var expressionBase = Parser.Parse(Tokens);
                var taskResult = expressionBase.Evaluate(new CancellationToken());
                taskResult.Start();

                // Verify
                Assert.IsInstanceOfType(expressionBase, typeof(OrExpression));
                Assert.IsFalse(taskResult.Result);
            }
        }
    }
}
