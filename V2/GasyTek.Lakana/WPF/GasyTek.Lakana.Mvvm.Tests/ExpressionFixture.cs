using System;
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
            var taskResult = ast.Evaluate();

            // Act
            taskResult.Start();

            // Verify
            Assert.IsFalse(taskResult.Result);
        }

        [TestMethod]
        public void CanEvaluateEqualToExpression()
        {
            // Prepare
            var viewModelProperty = new FakeViewModelProperty(10);
            var equalsToExpression = EqualToExpression.CreateUsingValue(viewModelProperty, 10);

            // Act
            var taskResult = equalsToExpression.Evaluate();
            taskResult.Start();

            // Verify
            Assert.IsTrue(taskResult.Result);
        }

        [TestMethod]
        public void CanEvaluateGreaterThanExpression()
        {
            // Prepare
            var viewModelProperty = new FakeViewModelProperty(10);
            var greaterThanExpression = GreaterThanExpression.CreateUsingValue(viewModelProperty, 5);

            // Act
            var taskResult = greaterThanExpression.Evaluate();
            taskResult.Start();

            // Verify
            Assert.IsTrue(taskResult.Result);
        }

        [TestMethod]
        public void CanEvaluateLessThanExpression()
        {
            // Prepare
            var viewModelProperty = new FakeViewModelProperty(10);
            var lessThanExpression = LessThanExpression.CreateUsingValue(viewModelProperty, 5);

            // Act
            var taskResult = lessThanExpression.Evaluate();
            taskResult.Start();

            // Verify
            Assert.IsFalse(taskResult.Result);
        }

        [TestMethod]
        public void CanEvaluateMatchingExpression()
        {
            // Prepare
            var viewModelProperty = new FakeViewModelProperty("Chuck Norris");
            var matchingExpression = MatchingExpression.CreateUsingProperty(viewModelProperty, "^(Ch)");    // expressions that starts with 'Ch'

            // Act
            var taskResult = matchingExpression.Evaluate();
            taskResult.Start();

            // Verify
            Assert.IsTrue(taskResult.Result);
        }

        [TestMethod]
        public void CanEvaluateNullExpression()
        {
            // Prepare
            var viewModelProperty = new FakeViewModelProperty(new object());
            var equalsToExpression = EqualToExpression.CreateUsingValue(viewModelProperty, null);

            // Act
            var taskResult = equalsToExpression.Evaluate();
            taskResult.Start();

            // Verify
            Assert.IsFalse(taskResult.Result);
        }

        [TestMethod]
        public void CanEvaluateNullOrEmptyExpression()
        {
            // Prepare
            var viewModelProperty = new FakeViewModelProperty(string.Empty);
            var equalsToExpression = EqualToExpression.CreateUsingValue(viewModelProperty, "");

            // Act
            var taskResult = equalsToExpression.Evaluate();
            taskResult.Start();

            // Verify
            Assert.IsTrue(taskResult.Result);
        }


    }
}
