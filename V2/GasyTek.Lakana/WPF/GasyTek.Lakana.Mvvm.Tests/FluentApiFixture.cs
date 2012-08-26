using GasyTek.Lakana.Mvvm.Tests.Fakes;
using GasyTek.Lakana.Mvvm.Validation.Fluent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasyTek.Lakana.Mvvm.Tests
{
    [TestClass]
    public class FluentApiFixture
    {
        private const string FakeEditableViewModelProperty = "FakeEditableViewModelProperty";
        private const string ParserProperty = "ParserProperty";
        private const string FluentApiProperty = "FluentApiProperty";

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void OnSetup()
        {
            var fakeEditableViewModel = new FakeEditableViewModel
                                         {
                                                Model = new Product
                                                            {
                                                                Code = "PR001", Quantity = 2, PurchasingPrice = 30, SellingPrice = 50, SellerEmail = "abc@abc.com"
                                                            }
                                            };
            var fluentApi = new FluentImplementer<FakeEditableViewModel>(fakeEditableViewModel);
            var parser = new Parser();

            TestContext.Properties.Add(FakeEditableViewModelProperty, fakeEditableViewModel);
            TestContext.Properties.Add(FluentApiProperty, fluentApi);
            TestContext.Properties.Add(ParserProperty, parser);
        }

        #region Helper methods

        private FakeEditableViewModel FakeEditableViewModel
        {
            get { return (FakeEditableViewModel)TestContext.Properties[FakeEditableViewModelProperty]; }
        }

        private IFluentProperty<FakeEditableViewModel> FluentApi
        {
            get { return (FluentImplementer<FakeEditableViewModel>)TestContext.Properties[FluentApiProperty]; }
        }        
        
        private Parser Parser
        {
            get { return (Parser)TestContext.Properties[ParserProperty]; }
        }

        private void Given(IFluentOtherwise<FakeEditableViewModel> expression)
        {
            
        }

        private void VerifyThatRuleIsBroken()
        {
            var expressionNode = Parser.Parse(((FluentImplementer<FakeEditableViewModel>) FluentApi).InternalTokens);
            var task = expressionNode.Evaluate();
            task.Start();

            // the rule is broken when the evaluation of the rule expression is equal to false.
            Assert.IsFalse(task.Result);
        }

        #endregion

        [TestMethod]
        public void CanEvaluateGreaterThanRule()
        {
            // define the rule
            Given(FluentApi.Property(vm => vm.Quantity).IsNot.GreaterThan(5));

            // break the rule
            FakeEditableViewModel.Quantity.Value = 10;

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateGreaterThanRuleAgainstProperty()
        {
            // define the rule
            Given(FluentApi.Property(vm => vm.SellingPrice).Is.GreaterThan(vm => vm.PurchasingPrice));

            // break the rule
            FakeEditableViewModel.PurchasingPrice.Value = 20;
            FakeEditableViewModel.SellingPrice.Value = 15;

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateLessThanRule()
        {
            // define the rule
            Given(FluentApi.Property(vm => vm.Quantity).Is.LessThan(20));

            // break the rule
            FakeEditableViewModel.Quantity.Value = 25;

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateLessThanRuleAgainstProperty()
        {
            // define the rule
            Given(FluentApi.Property(vm => vm.PurchasingPrice).Is.LessThan(vm => vm.SellingPrice));

            // break the rule
            FakeEditableViewModel.PurchasingPrice.Value = 20;
            FakeEditableViewModel.SellingPrice.Value = 15;

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateEqualToRule()
        {
            // define the rule
            Given(FluentApi.Property(vm => vm.Quantity).Is.EqualTo(10));

            // break the rule
            FakeEditableViewModel.Quantity.Value = 20;

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateEqualToRuleAgainstProperty()
        {
            // define the rule
            Given(FluentApi.Property(vm => vm.PurchasingPrice).Is.EqualTo(vm => vm.SellingPrice));

            // break the rule
            FakeEditableViewModel.SellingPrice.Value = 15;
            FakeEditableViewModel.PurchasingPrice.Value = 10;

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateNullRule()
        {
            // define the rule
            Given(FluentApi.Property(vm => vm.Code).Is.Null());

            // break the rule
            FakeEditableViewModel.Code.Value = "abc";

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateNullOrEmptyRule()
        {
            // define the rule
            Given(FluentApi.Property(vm => vm.Code).Is.NullOrEmpty());

            // break the rule
            FakeEditableViewModel.Code.Value = "abc";

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateValidEmailRule()
        {
            // define the rule
            Given(FluentApi.Property(vm => vm.SellerEmail).Is.ValidEmail());

            // break the rule
            FakeEditableViewModel.SellerEmail.Value = "abc@";

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateStartingWithRule()
        {
            // define the rule
            Given(FluentApi.Property(vm => vm.SellerEmail).Is.StartingWith("ab"));

            // break the rule
            FakeEditableViewModel.SellerEmail.Value = "bc@bc.com";

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateEndingWithRule()
        {
            // define the rule
            Given(FluentApi.Property(vm => vm.SellerEmail).Is.EndingWith(".com"));

            // break the rule
            FakeEditableViewModel.SellerEmail.Value = "bc@bc.net";

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateRequiredRule()
        {
            // define the rule
            Given(FluentApi.Property(vm => vm.Code).Is.Required());

            // break the rule
            FakeEditableViewModel.Code.Value = "";

            // verify that rule is broken
            VerifyThatRuleIsBroken();
            
        }

        [TestMethod]
        public void CanEvaluateDifferentOfRule()
        {
            // define the rule
            Given(FluentApi.Property(vm => vm.Code).Is.DifferentOf("xyz"));

            // break the rule
            FakeEditableViewModel.Code.Value = "xyz";

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateDifferentOfRuleAgainstProperty()
        {
            // define the rule
            Given(FluentApi.Property(vm => vm.PurchasingPrice).Is.DifferentOf(vm => vm.SellingPrice));

            // break the rule
            FakeEditableViewModel.PurchasingPrice.Value = 10;
            FakeEditableViewModel.SellingPrice.Value = 10;

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateBetweenRule()
        {
            // define the rule
            Given(FluentApi.Property(vm => vm.Quantity).Is.Between(5,8));

            // break the rule
            FakeEditableViewModel.Quantity.Value = 9;

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateBetweenRuleAgainstProperties()
        {
            // define the rule
            Given(FluentApi.Property(vm => vm.Quantity).Is.BetweenPoperties(vm => vm.PurchasingPrice, vm => vm.SellingPrice));

            // break the rule
            FakeEditableViewModel.Quantity.Value = 5;
            FakeEditableViewModel.PurchasingPrice.Value = 8;
            FakeEditableViewModel.SellingPrice.Value = 10;

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }
    }
}
