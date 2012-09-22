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

        [TestMethod]
        public void CanEvaluateEqualToExpression()
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
        public void CanEvaluateEqualToLateValueExpression()
        {
            // Prepare
            int[] dynamicValue = {5};
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
        public void CanEvaluateEqualToLateValueExpressionMoreThanOnce()
        {
            // Prepare
            int[] dynamicValue = {5};
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
        public void CanEvaluateGreaterThanExpression()
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
        public void CanEvaluateGreaterThanLateValueExpression()
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
        public void CanEvaluateLessThanExpression()
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
        public void CanEvaluateLessThanLateValueExpression()
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
        public void CanEvaluateNullExpression()
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
        public void CanEvaluateNullOrEmptyExpression()
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
