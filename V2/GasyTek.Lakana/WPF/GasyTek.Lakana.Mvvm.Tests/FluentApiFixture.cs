using System;
using System.Threading;
using GasyTek.Lakana.Mvvm.Tests.Fakes;
using GasyTek.Lakana.Mvvm.Validation.Fluent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasyTek.Lakana.Mvvm.Tests
{
    [TestClass]
    public class FluentApiFixture
    {
        private FakeEditableViewModel _fakeEditableViewModel;
        private Parser _parser;
        private object _fluentApi;
        
        [TestInitialize]
        public void OnSetup()
        {
            _fakeEditableViewModel = new FakeEditableViewModel
                                            {
                                                Model = new Product
                                                {
                                                    Code = "PR001",
                                                    Quantity = 2,
                                                    PurchasingPrice = 30,
                                                    SellingPrice = 50,
                                                    SellerEmail = "abc@abc.com"
                                                }
                                            };
            _parser = new Parser();
        }


        #region Helper methods

        private FakeEditableViewModel FakeEditableViewModel
        {
            get { return _fakeEditableViewModel; }
        }

        private IFluentProperty<FakeEditableViewModel, TPropertyValue> FluentApi<TPropertyValue>()
        {
            if (_fluentApi == null)
                _fluentApi = new FluentImplementer<FakeEditableViewModel, TPropertyValue>(FakeEditableViewModel);
            return (IFluentProperty<FakeEditableViewModel, TPropertyValue>)_fluentApi;
        }

        private Parser Parser
        {
            get { return _parser; }
        }

        private void Given(IFluentOtherwise expression)
        {
        }

        private void VerifyThatRuleIsBroken<TPropertyValue>()
        {
            var expressionNode = Parser.Parse(((FluentImplementer<FakeEditableViewModel, TPropertyValue>)FluentApi<TPropertyValue>()).InternalTokens);
            var task = expressionNode.Evaluate(new CancellationToken());
            task.RunSynchronously();

            // the rule is broken when the evaluation of the rule expression is equal to false.
            Assert.IsFalse(task.Result);
        }

        private void VerifyThatRuleIsSatisfied<TPropertyValue>()
        {
            var expressionNode = Parser.Parse(((FluentImplementer<FakeEditableViewModel, TPropertyValue>)FluentApi<TPropertyValue>()).InternalTokens);
            var task = expressionNode.Evaluate(new CancellationToken());
            task.Start();

            Assert.IsTrue(task.Result);
        }

        #endregion

        #region GreaterThan

        [TestClass]
        public class GreaterThan : FluentApiFixture
        {
            [TestMethod]
            public void CanEvaluateGreaterThanRule()
            {
                // define the rule
                Given(FluentApi<int>().Property(vm => vm.Quantity).IsNot.GreaterThan(5));

                // break the rule
                FakeEditableViewModel.Quantity.Value = 10;

                // verify that rule is broken
                VerifyThatRuleIsBroken<int>();
            }

            [TestMethod]
            public void CanEvaluateGreaterThanRuleAgainstProperty()
            {
                // define the rule
                Given(FluentApi<int>().Property(vm => vm.SellingPrice).Is.GreaterThan(vm => vm.PurchasingPrice));

                // break the rule
                FakeEditableViewModel.PurchasingPrice.Value = 20;
                FakeEditableViewModel.SellingPrice.Value = 15;

                // verify that rule is broken
                VerifyThatRuleIsBroken<int>();
            }

            [TestMethod]
            public void CanEvaluateGreaterThanRuleAgainstLateValue()
            {
                // define the rule
                int[] dynamicValue = {3};
                Given(FluentApi<int>().Property(vm => vm.PurchasingPrice).Is.GreaterThan(() => dynamicValue[0]));

                // break the rule
                dynamicValue[0] = 10;
                FakeEditableViewModel.PurchasingPrice.Value = 5;

                // verify that rule is broken
                VerifyThatRuleIsBroken<int>();
            }
        }
        
        #endregion

        #region LessThan

        [TestClass]
        public class LessThan : FluentApiFixture
        {
            [TestMethod]
            public void CanEvaluateLessThanRule()
            {
                // define the rule
                Given(FluentApi<int>().Property(vm => vm.Quantity).Is.LessThan(20));

                // break the rule
                FakeEditableViewModel.Quantity.Value = 25;

                // verify that rule is broken
                VerifyThatRuleIsBroken<int>();
            }

            [TestMethod]
            public void CanEvaluateLessThanRuleAgainstProperty()
            {
                // define the rule
                Given(FluentApi<int>().Property(vm => vm.PurchasingPrice).Is.LessThan(vm => vm.SellingPrice));

                // break the rule
                FakeEditableViewModel.PurchasingPrice.Value = 20;
                FakeEditableViewModel.SellingPrice.Value = 15;

                // verify that rule is broken
                VerifyThatRuleIsBroken<int>();
            }

            [TestMethod]
            public void CanEvaluateLessThanRuleAgainstLateValue()
            {
                // define the rule
                int[] dynamicValue = {15};
                Given(FluentApi<int>().Property(vm => vm.PurchasingPrice).Is.LessThan(() => dynamicValue[0]));

                // break the rule
                dynamicValue[0] = 5;
                FakeEditableViewModel.PurchasingPrice.Value = 10;

                // verify that rule is broken
                VerifyThatRuleIsBroken<int>();
            }
        }

        #endregion

        #region EqualTo

        [TestClass]
        public class EqualTo : FluentApiFixture
        {
            [TestMethod]
            public void CanEvaluateEqualToRule()
            {
                // define the rule
                Given(FluentApi<int>().Property(vm => vm.Quantity).Is.EqualTo(10));

                // break the rule
                FakeEditableViewModel.Quantity.Value = 20;

                // verify that rule is broken
                VerifyThatRuleIsBroken<int>();
            }

            [TestMethod]
            public void CanEvaluateEqualToRuleAgainstProperty()
            {
                // define the rule
                Given(FluentApi<int>().Property(vm => vm.PurchasingPrice).Is.EqualTo(vm => vm.SellingPrice));

                // break the rule
                FakeEditableViewModel.SellingPrice.Value = 15;
                FakeEditableViewModel.PurchasingPrice.Value = 10;

                // verify that rule is broken
                VerifyThatRuleIsBroken<int>();
            }

            [TestMethod]
            public void CanEvaluateEqualToRuleAgainstAsyncValue()
            {
                // define the rule
                int[] dynamicValue = {10};
                Given(FluentApi<int>().Property(vm => vm.PurchasingPrice).Is.EqualTo(() => dynamicValue[0]));

                // break the rule
                dynamicValue[0] = 15;
                FakeEditableViewModel.PurchasingPrice.Value = 10;

                // verify that rule is broken
                VerifyThatRuleIsBroken<int>();
            }
        }

        #endregion

        #region Null

        [TestClass]
        public class Null : FluentApiFixture
        {
            [TestMethod]
            public void CanEvaluateNullRule()
            {
                // define the rule
                Given(FluentApi<string>().Property(vm => vm.Code).Is.Null());

                // break the rule
                FakeEditableViewModel.Code.Value = "abc";

                // verify that rule is broken
                VerifyThatRuleIsBroken<string>();
            } 
        }

        #endregion

        #region NullOrEmpty

        [TestClass]
        public class NullOrEmpty : FluentApiFixture
        {
            [TestMethod]
            public void CanEvaluateNullOrEmptyRule()
            {
                // define the rule
                Given(FluentApi<string>().Property(vm => vm.Code).Is.NullOrEmpty());

                // break the rule
                FakeEditableViewModel.Code.Value = "abc";

                // verify that rule is broken
                VerifyThatRuleIsBroken<string>();
            }
        }

        #endregion

        #region ValidEmail

        [TestClass]
        public class ValidEmail : FluentApiFixture
        {
            [TestMethod]
            public void CanEvaluateValidEmailRule()
            {
                // define the rule
                Given(FluentApi<string>().Property(vm => vm.SellerEmail).Is.ValidEmail());

                // break the rule
                FakeEditableViewModel.SellerEmail.Value = "abc@";

                // verify that rule is broken
                VerifyThatRuleIsBroken<string>();
            }
        }

        #endregion

        #region StartingWith

        [TestClass]
        public class StartingWith : FluentApiFixture
        {
            [TestMethod]
            public void CanEvaluateStartingWithRule()
            {
                // define the rule
                Given(FluentApi<string>().Property(vm => vm.SellerEmail).Is.StartingWith("ab"));

                // break the rule
                FakeEditableViewModel.SellerEmail.Value = "bc@bc.com";

                // verify that rule is broken
                VerifyThatRuleIsBroken<string>();
            }
        }

        #endregion

        #region EndingWith

        [TestClass]
        public class EndingWith : FluentApiFixture
        {
            [TestMethod]
            public void CanEvaluateEndingWithRule()
            {
                // define the rule
                Given(FluentApi<string>().Property(vm => vm.SellerEmail).Is.EndingWith(".com"));

                // break the rule
                FakeEditableViewModel.SellerEmail.Value = "bc@bc.net";

                // verify that rule is broken
                VerifyThatRuleIsBroken<string>();
            }
        }

        #endregion

        #region Required

        [TestClass]
        public class Required : FluentApiFixture
        {
            [TestMethod]
            public void CanEvaluateRequiredRule()
            {
                // define the rule
                Given(FluentApi<string>().Property(vm => vm.Code).Is.Required());

                // break the rule
                FakeEditableViewModel.Code.Value = "";

                // verify that rule is broken
                VerifyThatRuleIsBroken<string>();
            }
        }

        #endregion

        #region DifferentOf

        [TestClass]
        public class DifferentOf : FluentApiFixture
        {
            [TestMethod]
            public void CanEvaluateDifferentOfRule()
            {
                // define the rule
                Given(FluentApi<string>().Property(vm => vm.Code).Is.DifferentOf("xyz"));

                // break the rule
                FakeEditableViewModel.Code.Value = "xyz";

                // verify that rule is broken
                VerifyThatRuleIsBroken<string>();
            }

            [TestMethod]
            public void CanEvaluateDifferentOfRuleAgainstProperty()
            {
                // define the rule
                Given(FluentApi<int>().Property(vm => vm.PurchasingPrice).Is.DifferentOf(vm => vm.SellingPrice));

                // break the rule
                FakeEditableViewModel.PurchasingPrice.Value = 10;
                FakeEditableViewModel.SellingPrice.Value = 10;

                // verify that rule is broken
                VerifyThatRuleIsBroken<int>();
            }

            [TestMethod]
            public void CanEvaluateDifferentOfRuleAgainstAsyncValue()
            {
                // define the rule
                int[] dynamicValue = {10};
                Given(FluentApi<int>().Property(vm => vm.PurchasingPrice).Is.DifferentOf(() => dynamicValue[0]));

                // break the rule
                dynamicValue[0] = 6;
                FakeEditableViewModel.PurchasingPrice.Value = 6;

                // verify that rule is broken
                VerifyThatRuleIsBroken<int>();
            }
        }
        
        #endregion

        #region BetweenValues

        [TestClass]
        public class Between : FluentApiFixture
        {
            [TestMethod]
            public void CanEvaluateBetweenRule()
            {
                // define the rule
                Given(FluentApi<int>().Property(vm => vm.Quantity).Is.BetweenValues(5, 8));

                // break the rule
                FakeEditableViewModel.Quantity.Value = 9;

                // verify that rule is broken
                VerifyThatRuleIsBroken<int>();
            }

            [TestMethod]
            public void BetweenIncludesLowerBoundary()
            {
                // define the rule
                Given(FluentApi<int>().Property(vm => vm.Quantity).Is.BetweenValues(5, 8));

                // break the rule
                FakeEditableViewModel.Quantity.Value = 5;

                // verify that rule is broken
                VerifyThatRuleIsSatisfied<int>();
            }

            [TestMethod]
            public void BetweenIncludesUpperBoundary()
            {
                // define the rule
                Given(FluentApi<int>().Property(vm => vm.Quantity).Is.BetweenValues(5, 8));

                // break the rule
                FakeEditableViewModel.Quantity.Value = 8;

                // verify that rule is broken
                VerifyThatRuleIsSatisfied<int>();
            }
        }

        #endregion

        #region BetweenPoperties

        [TestClass]
        public class BetweenProperties : FluentApiFixture
        {
            [TestMethod]
            public void CanEvaluateBetweenRuleAgainstProperties()
            {
                // define the rule
                Given(FluentApi<int>().Property(vm => vm.Quantity).Is.BetweenPoperties(vm => vm.PurchasingPrice, vm => vm.SellingPrice));

                // break the rule
                FakeEditableViewModel.Quantity.Value = 5;
                FakeEditableViewModel.PurchasingPrice.Value = 8;
                FakeEditableViewModel.SellingPrice.Value = 10;

                // verify that rule is broken
                VerifyThatRuleIsBroken<int>();
            }

            [TestMethod]
            public void BetweenPropertiesIncludeLowerBoundary()
            {
                // define the rule
                Given(FluentApi<int>().Property(vm => vm.Quantity).Is.BetweenPoperties(vm => vm.PurchasingPrice, vm => vm.SellingPrice));

                // break the rule
                FakeEditableViewModel.Quantity.Value = 5;
                FakeEditableViewModel.PurchasingPrice.Value = 5;
                FakeEditableViewModel.SellingPrice.Value = 10;

                // verify that rule is broken
                VerifyThatRuleIsSatisfied<int>();
            }

            [TestMethod]
            public void BetweenPropertiesIncludeUpperBoundary()
            {
                // define the rule
                Given(FluentApi<int>().Property(vm => vm.Quantity).Is.BetweenPoperties(vm => vm.PurchasingPrice, vm => vm.SellingPrice));

                // break the rule
                FakeEditableViewModel.Quantity.Value = 10;
                FakeEditableViewModel.PurchasingPrice.Value = 5;
                FakeEditableViewModel.SellingPrice.Value = 10;

                // verify that rule is broken
                VerifyThatRuleIsSatisfied<int>();
            }
        }

        #endregion

        #region MaxLength

        [TestClass]
        public class MaxLength : FluentApiFixture
        {
            [TestMethod]
            public void CanEvaluateMaxLengthRule()
            {
                // define the rule
                Given(FluentApi<string>().Property(vm => vm.Code).Has.MaxLength(3));

                // break the rule
                FakeEditableViewModel.Code.Value = "ABCD";

                // verify that rule is broken
                VerifyThatRuleIsBroken<string>();
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void CannotEvaluateMaxLengthRuleWhenParameterIsNegative()
            {
                // define the rule
                Given(FluentApi<string>().Property(vm => vm.Code).Has.MaxLength(-3));

                // break the rule
                FakeEditableViewModel.Code.Value = "ABCD";

                // verify that rule is broken
            }

            [TestMethod]
            public void CanEvaluateMaxLengthRuleWhenPropertyValueIsNull()
            {
                // define the rule
                Given(FluentApi<string>().Property(vm => vm.Code).Has.MaxLength(3));

                // break the rule
                FakeEditableViewModel.Code.Value = null;

                // verify that rule is broken
                VerifyThatRuleIsSatisfied<string>();
            }

            [TestMethod]
            public void CanEvaluateMaxLengthRuleWhenPropertyValueIsEmpty()
            {
                // define the rule
                Given(FluentApi<string>().Property(vm => vm.Code).Has.MaxLength(3));

                // break the rule
                FakeEditableViewModel.Code.Value = "";

                // verify that rule is broken
                VerifyThatRuleIsSatisfied<string>();
            }
        }

        #endregion

        #region MinLength

        [TestClass]
        public class MinLength : FluentApiFixture
        {
            [TestMethod]
             public void CanEvaluateMinLengthRule()
             {
                 // define the rule
                Given(FluentApi<string>().Property(vm => vm.Code).Has.MinLength(3));

                 // break the rule
                 FakeEditableViewModel.Code.Value = "AB";

                 // verify that rule is broken
                 VerifyThatRuleIsBroken<string>();
             }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void CannotEvaluateMinLengthRuleWhenParameterIsNegative()
            {
                // define the rule
                Given(FluentApi<string>().Property(vm => vm.Code).Has.MinLength(-3));

                // break the rule
                FakeEditableViewModel.Code.Value = "AB";

                // verify that rule is broken
            }

            [TestMethod]
            public void CanEvaluateMinLengthRuleWhenPropertyValueIsNull()
            {
                // define the rule
                Given(FluentApi<string>().Property(vm => vm.Code).Has.MinLength(3));

                // break the rule
                FakeEditableViewModel.Code.Value = null;

                // verify that rule is broken
                VerifyThatRuleIsBroken<string>();
            }

            [TestMethod]
            public void CanEvaluateMinLengthRuleWhenPropertyValueIsEmpty()
            {
                // define the rule
                Given(FluentApi<string>().Property(vm => vm.Code).Has.MinLength(3));

                // break the rule
                FakeEditableViewModel.Code.Value = "";

                // verify that rule is broken
                VerifyThatRuleIsBroken<string>();
            }
        }

        #endregion

        #region Valid

        [TestClass]
        public class Satisfying : FluentApiFixture
        {
            [TestMethod]
            public void CanEvaluateSatisfyingRule()
            {
                // define the rule
                CustomValidator<string> emailStartsWithGoodString = (value, token) => value.StartsWith("ab");
                Given(FluentApi<string>().Property(vm => vm.SellerEmail).Is.Valid(emailStartsWithGoodString));

                // break the rule
                FakeEditableViewModel.SellerEmail.Value = "bc@bc.com";

                // verify that rule is broken
                VerifyThatRuleIsBroken<string>();
            }
        }

        #endregion

        //#region When

        //[TestClass]
        //public class When : FluentApiFixture
        //{
        //    [TestMethod]
        //    public void CanBreakWhenRule()
        //    {
        //        // define the rule
        //        var condition = FluentApi.Property(vm => vm.Code).Is.EqualTo("ABC");
        //        Given(FluentApi.Property(vm => vm.SellingPrice).Is.GreaterThan(10).When(condition));

        //        // break the rule
        //        FakeEditableViewModel.Code.Value = "ABC";
        //        FakeEditableViewModel.SellingPrice.Value = 9;

        //        // verify that rule is broken
        //        VerifyThatRuleIsBroken();
        //    }

        //    [TestMethod]
        //    public void CanSatisfyWhenRule()
        //    {
        //        // define the rule
        //        var condition = FluentApi.Property(vm => vm.Code).Is.EqualTo("ABC");
        //        Given(FluentApi.Property(vm => vm.SellingPrice).Is.GreaterThan(10).When(condition));

        //        // break the rule
        //        FakeEditableViewModel.Code.Value = "ABC";
        //        FakeEditableViewModel.SellingPrice.Value = 12;

        //        // verify that rule is broken
        //        VerifyThatRuleIsSatisfied();
        //    }
        //}

        //#endregion
    }
}
