using GasyTek.Lakana.Mvvm.Tests.Fakes;
using GasyTek.Lakana.Mvvm.Validation.Fluent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasyTek.Lakana.Mvvm.Tests
{
    [TestClass]
    public class FluentApiFixture
    {
        private Parser _parser;
        private FakeEditableViewModel _fakeEditableViewModel;
        private IFluentProperty<FakeEditableViewModel> _fluentApi;
        
        [TestInitialize]
        public void OnSetup()
        {
            _fakeEditableViewModel = new FakeEditableViewModel
                                         {
                                                Model = new Product
                                                            {
                                                                Code = "PR001", Quantity = 2, PurchasingPrice = 30, SellingPrice = 50, SellerEmail = "abc@abc.com"
                                                            }
                                            };
            _fluentApi = new FluentImplementer<FakeEditableViewModel>(_fakeEditableViewModel);
            _parser = new Parser();
        }

        private void Given(IFluentOtherwise<FakeEditableViewModel> expression)
        {
            
        }

        private void VerifyThatRuleIsBroken()
        {
            var expressionNode = _parser.Parse(((FluentImplementer<FakeEditableViewModel>) _fluentApi).InternalTokens);
            var task = expressionNode.Evaluate();
            task.Start();

            // the rule is broken when the evaluation of the rule expression is equal to false.
            Assert.IsFalse(task.Result);
        }

        [TestMethod]
        public void CanEvaluateGreaterThanRule()
        {
            // define the rule
            Given(_fluentApi.Property(vm => vm.Quantity).IsNot.GreaterThan(5));

            // break the rule
            _fakeEditableViewModel.Quantity.Value = 10;

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateGreaterThanRuleAgainstProperty()
        {
            // define the rule
            Given(_fluentApi.Property(vm => vm.SellingPrice).Is.GreaterThan(vm => vm.PurchasingPrice));

            // break the rule
            _fakeEditableViewModel.PurchasingPrice.Value = 20;
            _fakeEditableViewModel.SellingPrice.Value = 15;

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateLessThanRule()
        {
            // define the rule
            Given(_fluentApi.Property(vm => vm.Quantity).Is.LessThan(20));

            // break the rule
            _fakeEditableViewModel.Quantity.Value = 25;

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateLessThanRuleAgainstProperty()
        {
            // define the rule
            Given(_fluentApi.Property(vm => vm.PurchasingPrice).Is.LessThan(vm => vm.SellingPrice));

            // break the rule
            _fakeEditableViewModel.PurchasingPrice.Value = 20;
            _fakeEditableViewModel.SellingPrice.Value = 15;

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateEqualToRule()
        {
            // define the rule
            Given(_fluentApi.Property(vm => vm.Quantity).Is.EqualTo(10));

            // break the rule
            _fakeEditableViewModel.Quantity.Value = 20;

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateEqualToRuleAgainstProperty()
        {
            // define the rule
            Given(_fluentApi.Property(vm => vm.PurchasingPrice).Is.EqualTo(vm => vm.SellingPrice));

            // break the rule
            _fakeEditableViewModel.SellingPrice.Value = 15;
            _fakeEditableViewModel.PurchasingPrice.Value = 10;

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateNullRule()
        {
            // define the rule
            Given(_fluentApi.Property(vm => vm.Code).Is.Null());

            // break the rule
            _fakeEditableViewModel.Code.Value = "abc";

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateNullOrEmptyRule()
        {
            // define the rule
            Given(_fluentApi.Property(vm => vm.Code).Is.NullOrEmpty());

            // break the rule
            _fakeEditableViewModel.Code.Value = "abc";

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateValidEmailRule()
        {
            // define the rule
            Given(_fluentApi.Property(vm => vm.SellerEmail).Is.ValidEmail());

            // break the rule
            _fakeEditableViewModel.SellerEmail.Value = "abc@";

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateStartingWithRule()
        {
            // define the rule
            Given(_fluentApi.Property(vm => vm.SellerEmail).Is.StartingWith("ab"));

            // break the rule
            _fakeEditableViewModel.SellerEmail.Value = "bc@bc.com";

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateEndingWithRule()
        {
            // define the rule
            Given(_fluentApi.Property(vm => vm.SellerEmail).Is.EndingWith(".com"));

            // break the rule
            _fakeEditableViewModel.SellerEmail.Value = "bc@bc.net";

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateRequiredRule()
        {
            // define the rule
            Given(_fluentApi.Property(vm => vm.Code).Is.Required());

            // break the rule
            _fakeEditableViewModel.Code.Value = "";

            // verify that rule is broken
            VerifyThatRuleIsBroken();
            
        }

        [TestMethod]
        public void CanEvaluateDifferentOfRule()
        {
            // define the rule
            Given(_fluentApi.Property(vm => vm.Code).Is.DifferentOf("xyz"));

            // break the rule
            _fakeEditableViewModel.Code.Value = "xyz";

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateDifferentOfRuleAgainstProperty()
        {
            // define the rule
            Given(_fluentApi.Property(vm => vm.PurchasingPrice).Is.DifferentOf(vm => vm.SellingPrice));

            // break the rule
            _fakeEditableViewModel.PurchasingPrice.Value = 10;
            _fakeEditableViewModel.SellingPrice.Value = 10;

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateBetweenRule()
        {
            // define the rule
            Given(_fluentApi.Property(vm => vm.Quantity).Is.Between(5,8));

            // break the rule
            _fakeEditableViewModel.Quantity.Value = 9;

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }

        [TestMethod]
        public void CanEvaluateBetweenRuleAgainstProperties()
        {
            // define the rule
            Given(_fluentApi.Property(vm => vm.Quantity).Is.BetweenPoperties(vm => vm.PurchasingPrice, vm => vm.SellingPrice));

            // break the rule
            _fakeEditableViewModel.Quantity.Value = 5;
            _fakeEditableViewModel.PurchasingPrice.Value = 8;
            _fakeEditableViewModel.SellingPrice.Value = 10;

            // verify that rule is broken
            VerifyThatRuleIsBroken();
        }
    }
}
