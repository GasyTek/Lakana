using System.Linq;
using System.Threading;
using GasyTek.Lakana.Mvvm.Tests.Fakes;
using GasyTek.Lakana.Mvvm.Validation.Fluent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasyTek.Lakana.Mvvm.Tests
{
    [TestClass]
    public class FluentValidationEngineFixture
    {
        private const string FluentValidationEngineProperty = "FluentValidationEngineProperty";
        private const string FakeEditableViewModelProperty = "FakeEditableViewModelProperty";

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void OnSetup()
        {
            var fakeEditableViewModel = new FakeEditableViewModel ();
            var fluentValidationEngine = new FakeFluentValidationEngine(fakeEditableViewModel);
            fakeEditableViewModel.ValidationEngineProvider = () => fluentValidationEngine;
            fakeEditableViewModel.Model = new Product();

            TestContext.Properties.Add(FakeEditableViewModelProperty, fakeEditableViewModel);
            TestContext.Properties.Add(FluentValidationEngineProperty, fluentValidationEngine);
        }

        #region Helper methods
        
        private FakeEditableViewModel FakeEditableViewModel
        {
            get { return (FakeEditableViewModel) TestContext.Properties[FakeEditableViewModelProperty]; }
        }

        private FakeFluentValidationEngine FakeFluentValidationEngine
        {
            get { return (FakeFluentValidationEngine) TestContext.Properties[FluentValidationEngineProperty]; }
        }

        private void ConfigureThread()
        {
            // assigns a synchronization context to the testing thread
            // to allow task to use TaskScheduler.FromSynchronizationContext()
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

        private void VerifyThatRuleIsBroken(string propertyName)
        {
            // if this is satisfied, that means that rules were broken
            var fluentValidationEngine = (FakeFluentValidationEngine) TestContext.Properties[FluentValidationEngineProperty];
            Assert.IsFalse(fluentValidationEngine.IsValid(propertyName));
        }

        #endregion

        [TestMethod]
        [ExpectedException(typeof(RuleDefinitionException))]
        public void CanDetectIncompleteRules()
        {
            // Prepare
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine.AssertThatProperty(vm => vm.Quantity);    // incomplete rule

            // Act
            FakeFluentValidationEngine.BuildRules();

            // Verify
        }

        [TestMethod]
        public void CanDefineManyRulesOnOneProperty()
        {
            // Prepare
            ConfigureThread();

            var propertyName = FakeEditableViewModel.Quantity.PropertyMetadata.Name;
            FakeFluentValidationEngine.DefineRulesAction = 
                () =>
                    {
                        FakeFluentValidationEngine.AssertThatProperty(vm => vm.Quantity).Is.GreaterThan(20);
                        FakeFluentValidationEngine.AssertThatProperty(vm => vm.Quantity).Is.EqualTo(40);
                    };
            FakeFluentValidationEngine.BuildRules(); 

            // Act
            FakeEditableViewModel.Quantity.Value = 15;
            FakeEditableViewModel.WaitForPropertyValidationsToTerminate();

            // Verify
            // if this is satisfied, that means that rules were broken
            Assert.IsNotNull(FakeFluentValidationEngine.GetErrors(propertyName));
            Assert.IsTrue(FakeFluentValidationEngine.GetErrors(propertyName).Count() == 2);
        }

        [TestMethod]
        public void CanBreakSimpleRule()
        {
            // Prepare
            ConfigureThread();

            var propertyName = FakeEditableViewModel.PurchasingPrice.PropertyMetadata.Name;
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine.AssertThatProperty(vm => vm.PurchasingPrice).Is.GreaterThan(20);
            FakeFluentValidationEngine.BuildRules();

            // Act
            FakeEditableViewModel.PurchasingPrice.Value = 20;
            FakeEditableViewModel.WaitForPropertyValidationsToTerminate();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakSimpleRuleWithCustomMessage()
        {
            // Prepare
            ConfigureThread();

            const string erroMessage = "PurchasingPrice must be greater that 20";
            var propertyName = FakeEditableViewModel.PurchasingPrice.PropertyMetadata.Name;
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine.AssertThatProperty(vm => vm.PurchasingPrice)
                .Is.GreaterThan(20).Otherwise(erroMessage);
            FakeFluentValidationEngine.BuildRules();

            // Act
            FakeEditableViewModel.PurchasingPrice.Value = 20;
            FakeEditableViewModel.WaitForPropertyValidationsToTerminate();

            // Verify
            Assert.IsNotNull(FakeFluentValidationEngine.GetErrors(propertyName));
            Assert.IsTrue(FakeFluentValidationEngine.GetErrors(propertyName).Any());
            Assert.AreEqual(erroMessage, FakeFluentValidationEngine.GetErrors(propertyName).ToList() [0]);
        }

        [TestMethod]
        public void CanBreakSimpleRuleThatComparesTwoProperties()
        {
            // Prepare
            ConfigureThread();
            FakeEditableViewModel.ConfigureExpectedNumberOfPropertyValidation(2);

            var propertyName = FakeEditableViewModel.PurchasingPrice.PropertyMetadata.Name;
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine.AssertThatProperty(vm => vm.PurchasingPrice).Is.LessThan(vm => vm.SellingPrice);
            FakeFluentValidationEngine.BuildRules();

            // Act
            FakeEditableViewModel.SellingPrice.Value = 19;
            FakeEditableViewModel.PurchasingPrice.Value = 20;
            FakeEditableViewModel.WaitForPropertyValidationsToTerminate();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakRuleWithAndConjunction()
        {
            // Prepare
            ConfigureThread();
            FakeEditableViewModel.ConfigureExpectedNumberOfPropertyValidation(2);

            var propertyName = FakeEditableViewModel.PurchasingPrice.PropertyMetadata.Name;
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine.AssertThatProperty(vm => vm.PurchasingPrice)
                .Is.LessThan(vm => vm.SellingPrice).And.Is.EqualTo(20);
            FakeFluentValidationEngine.BuildRules();

            // Act
            FakeEditableViewModel.SellingPrice.Value = 21;
            FakeEditableViewModel.PurchasingPrice.Value = 19;
            FakeEditableViewModel.WaitForPropertyValidationsToTerminate();
            
            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakRuleWithOrConjunction()
        {
            // Prepare
            ConfigureThread();

            var propertyName = FakeEditableViewModel.Quantity.PropertyMetadata.Name;
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine.AssertThatProperty(vm => vm.Quantity)
                .Is.EqualTo(20).Or.Is.EqualTo(30);
            FakeFluentValidationEngine.BuildRules();

            // Act
            FakeEditableViewModel.Quantity.Value = 25;
            FakeEditableViewModel.WaitForPropertyValidationsToTerminate();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakRuleThatUsesIsNotOperator()
        {
            // Prepare
            ConfigureThread();

            var propertyName = FakeEditableViewModel.Quantity.PropertyMetadata.Name;
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine.AssertThatProperty(vm => vm.Quantity).IsNot.EqualTo(20);
            FakeFluentValidationEngine.BuildRules();

            // Act
            FakeEditableViewModel.Quantity.Value = 20;
            FakeEditableViewModel.WaitForPropertyValidationsToTerminate();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakRuleThatUsesBetweenOperator()
        {
            // Prepare
            ConfigureThread();

            var propertyName = FakeEditableViewModel.Quantity.PropertyMetadata.Name;
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine.AssertThatProperty(vm => vm.Quantity).Is.Between(15, 25);
            FakeFluentValidationEngine.BuildRules();

            // Act
            FakeEditableViewModel.Quantity.Value = 30;
            FakeEditableViewModel.WaitForPropertyValidationsToTerminate();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakRuleThatUsesBetweenPropertiesOperator()
        {
            // Prepare
            ConfigureThread();
            FakeEditableViewModel.ConfigureExpectedNumberOfPropertyValidation(3);

            var propertyName = FakeEditableViewModel.Quantity.PropertyMetadata.Name;
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine
                .AssertThatProperty(vm => vm.Quantity).Is.BetweenPoperties(vm => vm.PurchasingPrice, vm => vm.SellingPrice);
            FakeFluentValidationEngine.BuildRules();

            // Act
            FakeEditableViewModel.PurchasingPrice.Value = 10;
            FakeEditableViewModel.SellingPrice.Value = 20;
            FakeEditableViewModel.Quantity.Value = 30;
            FakeEditableViewModel.WaitForPropertyValidationsToTerminate();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakRuleThatUsesIsNotBetweenOperator()
        {
            // Prepare
            ConfigureThread();

            var propertyName = FakeEditableViewModel.Quantity.PropertyMetadata.Name;
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine.AssertThatProperty(vm => vm.Quantity).IsNot.Between(15, 25);
            FakeFluentValidationEngine.BuildRules();

            // Act
            FakeEditableViewModel.Quantity.Value = 20;
            FakeEditableViewModel.WaitForPropertyValidationsToTerminate();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void WhenTwoPropertiesInvolvedTheLastValidatedHasTheSharedErrorMessage()
        {
            // Prepare
            ConfigureThread();
            FakeEditableViewModel.ConfigureExpectedNumberOfPropertyValidation(4);

            var fluentValidationEngine = (FakeFluentValidationEngine)TestContext.Properties[FluentValidationEngineProperty];
            var purchasingPricePropertyName = FakeEditableViewModel.PurchasingPrice.PropertyMetadata.Name;
            var sellingPricePropertyName = FakeEditableViewModel.SellingPrice.PropertyMetadata.Name;
            FakeEditableViewModel.SellingPrice.Value = 30;
            FakeEditableViewModel.PurchasingPrice.Value = 20;
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine
                .AssertThatProperty(vm => vm.PurchasingPrice).Is.LessThan(vm => vm.SellingPrice).Otherwise("ERROR");
            FakeFluentValidationEngine.BuildRules();
            
            // Act
            FakeEditableViewModel.SellingPrice.Value = 10;
            FakeEditableViewModel.PurchasingPrice.Value = 15;
            FakeEditableViewModel.WaitForPropertyValidationsToTerminate();

            // Verify
            var purchasingPriceErrors = fluentValidationEngine.GetErrors(purchasingPricePropertyName);
            var sellingPriceErrors = fluentValidationEngine.GetErrors(sellingPricePropertyName);

            Assert.IsFalse(sellingPriceErrors.Any());
            Assert.IsNotNull(purchasingPriceErrors);
            Assert.IsTrue(purchasingPriceErrors.Count() == 1);
        }

        [TestMethod]
        public void WhenTwoPropertiesInvolvedErrorsOtherThanSharedOnesRemains()
        {
            // Prepare
            ConfigureThread();
            FakeEditableViewModel.ConfigureExpectedNumberOfPropertyValidation(4);

            var fluentValidationEngine =
                (FakeFluentValidationEngine) TestContext.Properties[FluentValidationEngineProperty];
            var purchasingPricePropertyName = FakeEditableViewModel.PurchasingPrice.PropertyMetadata.Name;
            var sellingPricePropertyName = FakeEditableViewModel.SellingPrice.PropertyMetadata.Name;
            FakeEditableViewModel.SellingPrice.Value = 30;
            FakeEditableViewModel.PurchasingPrice.Value = 20;

            FakeFluentValidationEngine.DefineRulesAction = () =>
                                                               {
                                                                   // Selling price shoud be greater than 30
                                                                   FakeFluentValidationEngine.AssertThatProperty(
                                                                       vm => vm.SellingPrice).Is.GreaterThan(30);

                                                                   // Purchasing price shoud be less than Selling price
                                                                   FakeFluentValidationEngine
                                                                       .AssertThatProperty(vm => vm.PurchasingPrice).Is.
                                                                       LessThan(vm => vm.SellingPrice);
                                                               };
        

            FakeFluentValidationEngine.BuildRules();

            // Act
            FakeEditableViewModel.SellingPrice.Value = 10;
            FakeEditableViewModel.PurchasingPrice.Value = 15;
            FakeEditableViewModel.WaitForPropertyValidationsToTerminate();

            // Verify
            var purchasingPriceErrors = fluentValidationEngine.GetErrors(purchasingPricePropertyName);
            var sellingPriceErrors = fluentValidationEngine.GetErrors(sellingPricePropertyName);

            Assert.IsNotNull(sellingPriceErrors);
            Assert.IsTrue(sellingPriceErrors.Count() == 1);
            Assert.IsNotNull(purchasingPriceErrors);
            Assert.IsTrue(purchasingPriceErrors.Count() == 1);

        }

    }
}
