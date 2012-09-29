using System;
using System.Threading;
using GasyTek.Lakana.Mvvm.Tests.Fakes;
using GasyTek.Lakana.Mvvm.Validation.Fluent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasyTek.Lakana.Mvvm.Tests
{
    [TestClass]
    public class ExpressionFixture
    {
        [TestMethod]
        public void CanEvaluateAsynchronously()
        {
            // Prepare
            var leaf1 = new FakeEvaluableExpression(true, TimeSpan.FromSeconds(1));
            var leaf2 = new FakeEvaluableExpression(true);
            var leaf3 = new FakeEvaluableExpression(false);

            var notOp = new NotExpression { Left = leaf1 };
            var andOp = new AndExpression { Left = notOp, Right = leaf2 };

            var ast = new OrExpression { Left = andOp, Right = leaf3 };
            var taskResult = ast.Evaluate(new CancellationToken());

            // Act
            taskResult.RunSynchronously();

            // Verify
            Assert.IsFalse(taskResult.Result);
        }

        [TestClass]
        public class EqualTo : ExpressionFixture
        {
            [TestMethod]
            public void EqualToExprCanEvaluate()
            {
                // Prepare
                var viewModelProperty = new FakeViewModelProperty(10);
                var equalsToExpression = ExpressionNode.EqualToValue(viewModelProperty, 10);

                // Act
                var taskResult = equalsToExpression.Evaluate(new CancellationToken());
                taskResult.RunSynchronously();

                // Verify
                Assert.IsTrue(taskResult.Result);
            }

            [TestMethod]
            public void EqualToLateValueExprCanEvaluate()
            {
                // Prepare
                int[] dynamicValue = { 5 };
                var viewModelProperty = new FakeViewModelProperty(10);
                var equalsToExpression = ExpressionNode.EqualToLateValue(viewModelProperty, () => dynamicValue[0]);

                // Act
                dynamicValue[0] = 10;
                var taskResult = equalsToExpression.Evaluate(new CancellationToken());
                taskResult.RunSynchronously();

                // Verify
                Assert.IsTrue(taskResult.Result);
            }

            [TestMethod]
            public void EqualToLateValueExprCanEvaluateMoreThanOnce()
            {
                // Prepare
                int[] dynamicValue = { 5 };
                var viewModelProperty = new FakeViewModelProperty(10);
                var equalsToExpression = ExpressionNode.EqualToLateValue(viewModelProperty, () => dynamicValue[0]);

                // Act
                dynamicValue[0] = 10;
                var t = equalsToExpression.Evaluate(new CancellationToken());
                t.RunSynchronously();
                var taskResult = equalsToExpression.Evaluate(new CancellationToken());
                taskResult.RunSynchronously();

                // Verify
                Assert.IsTrue(taskResult.Result);
            }

            [TestMethod]
            public void EqualToNullExprCanEvaluate()
            {
                // Prepare
                var viewModelProperty = new FakeViewModelProperty(new object());
                var equalsToExpression = ExpressionNode.EqualToValue(viewModelProperty, null);

                // Act
                var taskResult = equalsToExpression.Evaluate(new CancellationToken());
                taskResult.RunSynchronously();

                // Verify
                Assert.IsFalse(taskResult.Result);
            }

            [TestMethod]
            public void EqualToNullOrEmptyExprCanEvaluate()
            {
                // Prepare
                var viewModelProperty = new FakeViewModelProperty(string.Empty);
                var equalsToExpression = ExpressionNode.EqualToValue(viewModelProperty, "");

                // Act
                var taskResult = equalsToExpression.Evaluate(new CancellationToken());
                taskResult.RunSynchronously();

                // Verify
                Assert.IsTrue(taskResult.Result);
            }
        }

        [TestClass]
        public class GreaterThan : ExpressionFixture
        {
            [TestMethod]
            public void GreaterThanExprCanEvaluate()
            {
                // Prepare
                var viewModelProperty = new FakeViewModelProperty(10);
                var greaterThanExpression = ExpressionNode.GreaterThanValue(viewModelProperty, 5);

                // Act
                var taskResult = greaterThanExpression.Evaluate(new CancellationToken());
                taskResult.RunSynchronously();

                // Verify
                Assert.IsTrue(taskResult.Result);
            }

            [TestMethod]
            public void GreaterThanLateValueExprCanEvaluate()
            {
                // Prepare
                int[] dynamicValue = { 20 };
                var viewModelProperty = new FakeViewModelProperty(10);
                var equalsToExpression = ExpressionNode.GreaterThanLateValue(viewModelProperty, () => dynamicValue[0]);

                // Act
                dynamicValue[0] = 5;
                var taskResult = equalsToExpression.Evaluate(new CancellationToken());
                taskResult.RunSynchronously();

                // Verify
                Assert.IsTrue(taskResult.Result);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void GreaterThanExprShouldNotAllowNullOperands()
            {
                // Prepare
                var viewModelProperty = new FakeViewModelProperty(null);
                var greaterThanExpression = ExpressionNode.GreaterThanValue(viewModelProperty, null);

                // Act
                var taskResult = greaterThanExpression.Evaluate(new CancellationToken());
                taskResult.Start();
                try
                {
                    // to throw eventual exceptions
                    taskResult.Wait();
                }
                catch (AggregateException ae)
                {
                    ae.Flatten().Handle(ex =>
                    {
                        if (ex is InvalidOperationException)
                            throw ex;
                        return false;
                    });
                }
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void GreaterThanExprPropertyValueShouldBeComparable()
            {
                // Prepare
                var viewModelProperty = new FakeViewModelProperty(new object());
                var greaterThanExpression = ExpressionNode.GreaterThanValue(viewModelProperty, 2);

                // Act
                var taskResult = greaterThanExpression.Evaluate(new CancellationToken());
                taskResult.Start();
                try
                {
                    // to throw eventual exceptions
                    taskResult.Wait();
                }
                catch (AggregateException ae)
                {
                    ae.Flatten().Handle(ex =>
                    {
                        if (ex is InvalidOperationException)
                            throw ex;
                        return false;
                    });
                }
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GreaterThanExprShouldNotAllowComparingDifferentTypes()
            {
                // Prepare
                var viewModelProperty = new FakeViewModelProperty(10);
                var greaterThanExpression = ExpressionNode.GreaterThanValue(viewModelProperty, "NotAllowedValue");

                // Act
                var taskResult = greaterThanExpression.Evaluate(new CancellationToken());
                taskResult.Start();
                try
                {
                    // to throw eventual exceptions
                    taskResult.Wait();  
                }
                catch (AggregateException ae)
                {
                    ae.Flatten().Handle(ex =>
                                            {
                                                if (ex is ArgumentException)
                                                    throw ex;
                                                return false;
                                            });
                }

                // Verify

            }
        }
        
        [TestClass]
        public class LessThan : ExpressionFixture
        {
            [TestMethod]
            public void LessThanExprCanEvaluate()
            {
                // Prepare
                var viewModelProperty = new FakeViewModelProperty(10);
                var lessThanExpression = ExpressionNode.LessThanValue(viewModelProperty, 5);

                // Act
                var taskResult = lessThanExpression.Evaluate(new CancellationToken());
                taskResult.RunSynchronously();

                // Verify
                Assert.IsFalse(taskResult.Result);
            }

            [TestMethod]
            public void LessThanLateValueExprCanEvaluate()
            {
                // Prepare
                int[] dynamicValue = { 5 };
                var viewModelProperty = new FakeViewModelProperty(10);
                var equalsToExpression = ExpressionNode.LessThanLateValue(viewModelProperty, () => dynamicValue[0]);

                // Act
                dynamicValue[0] = 20;
                var taskResult = equalsToExpression.Evaluate(new CancellationToken());
                taskResult.RunSynchronously();

                // Verify
                Assert.IsTrue(taskResult.Result);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void LessThanExprShouldNotAllowNullOperands()
            {
                // Prepare
                var viewModelProperty = new FakeViewModelProperty(null);
                var lessThanExpression = ExpressionNode.LessThanValue(viewModelProperty, null);

                // Act
                var taskResult = lessThanExpression.Evaluate(new CancellationToken());
                taskResult.Start();
                try
                {
                    // to throw eventual exceptions
                    taskResult.Wait();
                }
                catch (AggregateException ae)
                {
                    ae.Flatten().Handle(ex =>
                    {
                        if (ex is InvalidOperationException)
                            throw ex;
                        return false;
                    });
                }
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void LessThanExprPropertyValueShouldBeComparable()
            {
                // Prepare
                var viewModelProperty = new FakeViewModelProperty(new object());
                var lessThanExpression = ExpressionNode.LessThanValue(viewModelProperty, 2);

                // Act
                var taskResult = lessThanExpression.Evaluate(new CancellationToken());
                taskResult.Start();
                try
                {
                    // to throw eventual exceptions
                    taskResult.Wait();
                }
                catch (AggregateException ae)
                {
                    ae.Flatten().Handle(ex =>
                    {
                        if (ex is InvalidOperationException)
                            throw ex;
                        return false;
                    });
                }
            }
        }

        [TestMethod]
        public void CanEvaluateMatchingExpression()
        {
            // Prepare
            var viewModelProperty = new FakeViewModelProperty("Chuck Norris");
            var matchingExpression = ExpressionNode.MatchingProperty(viewModelProperty, "^(Ch)");    // expressions that starts with 'Ch'

            // Act
            var taskResult = matchingExpression.Evaluate(new CancellationToken());
            taskResult.RunSynchronously();

            // Verify
            Assert.IsTrue(taskResult.Result);
        }

        [TestMethod]
        public void CanEvaluateCustomValidationExpression()
        {
            // Prepare
            var viewModelProperty = new FakeViewModelProperty(string.Empty);
            var customValidationExpression = ExpressionNode.CustomValidation(viewModelProperty, (value, token) => String.IsNullOrEmpty(value as string));

            // Act
            var taskResult = customValidationExpression.Evaluate(new CancellationToken());
            taskResult.RunSynchronously();

            // Verify
            Assert.IsTrue(taskResult.Result);
        }

    }
}
