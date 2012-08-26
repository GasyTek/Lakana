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
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine.TestProperty(vm => vm.Quantity);    // incomplete rule

            // Act
            FakeFluentValidationEngine.BuildRules();

            // Verify
        }

        [TestMethod]
        public void CanDefineManyRulesOnOneProperty()
        {
            ConfigureThread();

            // Prepare
            var propertyName = FakeEditableViewModel.Quantity.PropertyMetadata.Name;
            FakeFluentValidationEngine.DefineRulesAction = 
                () =>
                    {
                        FakeFluentValidationEngine.TestProperty(vm => vm.Quantity).Is.GreaterThan(20);
                        FakeFluentValidationEngine.TestProperty(vm => vm.Quantity).Is.EqualTo(40);
                    };
            FakeFluentValidationEngine.BuildRules(); 

            // Act
            FakeEditableViewModel.Quantity.Value = 15;
            FakeEditableViewModel.SynchronizationTask.Wait();

            // Verify
            // if this is satisfied, that means that rules were broken
            Assert.IsNotNull(FakeFluentValidationEngine.GetErrors(propertyName));
            Assert.IsTrue(FakeFluentValidationEngine.GetErrors(propertyName).Count() == 2);
        }

        [TestMethod]
        public void CanBreakSimpleRule()
        {
            ConfigureThread();

            // Prepare
            var propertyName = FakeEditableViewModel.PurchasingPrice.PropertyMetadata.Name;
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine.TestProperty(vm => vm.PurchasingPrice).Is.GreaterThan(20);
            FakeFluentValidationEngine.BuildRules();

            // Act
            FakeEditableViewModel.PurchasingPrice.Value = 20;
            FakeEditableViewModel.SynchronizationTask.Wait();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakSimpleRuleWithCustomMessage()
        {
            ConfigureThread();

            // Prepare
            const string erroMessage = "PurchasingPrice must be greater that 20";
            var propertyName = FakeEditableViewModel.PurchasingPrice.PropertyMetadata.Name;
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine.TestProperty(vm => vm.PurchasingPrice)
                .Is.GreaterThan(20).Otherwise(erroMessage);
            FakeFluentValidationEngine.BuildRules();

            // Act
            FakeEditableViewModel.PurchasingPrice.Value = 20;
            FakeEditableViewModel.SynchronizationTask.Wait();

            // Verify
            Assert.IsNotNull(FakeFluentValidationEngine.GetErrors(propertyName));
            Assert.IsTrue(FakeFluentValidationEngine.GetErrors(propertyName).Any());
            Assert.AreEqual(erroMessage, FakeFluentValidationEngine.GetErrors(propertyName).ToList() [0]);
        }

        [TestMethod]
        public void CanBreakSimpleRuleThatComparesTwoProperties()
        {
            ConfigureThread();

            // Prepare
            var propertyName = FakeEditableViewModel.PurchasingPrice.PropertyMetadata.Name;
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine.TestProperty(vm => vm.PurchasingPrice).Is.LessThan(vm => vm.SellingPrice);
            FakeFluentValidationEngine.BuildRules();

            // Act
            FakeEditableViewModel.PurchasingPrice.Value = 20;
            FakeEditableViewModel.SellingPrice.Value = 19;
            FakeEditableViewModel.SynchronizationTask.Wait();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakRuleWithAndConjunction()
        {
            ConfigureThread();

            // Prepare
            var propertyName = FakeEditableViewModel.PurchasingPrice.PropertyMetadata.Name;
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine.TestProperty(vm => vm.PurchasingPrice)
                .Is.LessThan(vm => vm.SellingPrice).And.Is.EqualTo(20);
            FakeFluentValidationEngine.BuildRules();

            // Act
            FakeEditableViewModel.PurchasingPrice.Value = 19;
            FakeEditableViewModel.SellingPrice.Value = 21;
            FakeEditableViewModel.SynchronizationTask.Wait();
            
            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakRuleWithOrConjunction()
        {
            ConfigureThread();

            // Prepare
            var propertyName = FakeEditableViewModel.Quantity.PropertyMetadata.Name;
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine.TestProperty(vm => vm.Quantity)
                .Is.EqualTo(20).Or.Is.EqualTo(30);
            FakeFluentValidationEngine.BuildRules();

            // Act
            FakeEditableViewModel.Quantity.Value = 25;
            FakeEditableViewModel.SynchronizationTask.Wait();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakRuleThatUsesIsNotOperator()
        {
            ConfigureThread();

            // Prepare
            var propertyName = FakeEditableViewModel.Quantity.PropertyMetadata.Name;
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine.TestProperty(vm => vm.Quantity).IsNot.EqualTo(20);
            FakeFluentValidationEngine.BuildRules();

            // Act
            FakeEditableViewModel.Quantity.Value = 20;
            FakeEditableViewModel.SynchronizationTask.Wait();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakRuleThatUsesBetweenOperator()
        {
            ConfigureThread();

            // Prepare
            var propertyName = FakeEditableViewModel.Quantity.PropertyMetadata.Name;
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine.TestProperty(vm => vm.Quantity).Is.Between(15, 25);
            FakeFluentValidationEngine.BuildRules();

            // Act
            FakeEditableViewModel.Quantity.Value = 30;
            FakeEditableViewModel.SynchronizationTask.Wait();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakRuleThatUsesBetweenPropertiesOperator()
        {
            ConfigureThread();

            // Prepare
            var propertyName = FakeEditableViewModel.Quantity.PropertyMetadata.Name;
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine.TestProperty(vm => vm.Quantity)
                .Is.BetweenPoperties(vm => vm.PurchasingPrice, vm => vm.SellingPrice);
            FakeFluentValidationEngine.BuildRules();

            // Act
            FakeEditableViewModel.Quantity.Value = 30;
            FakeEditableViewModel.PurchasingPrice.Value = 10;
            FakeEditableViewModel.SellingPrice.Value = 20;
            FakeEditableViewModel.SynchronizationTask.Wait();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakRuleThatUsesIsNotBetweenOperator()
        {
            ConfigureThread();

            // Prepare
            var propertyName = FakeEditableViewModel.Quantity.PropertyMetadata.Name;
            FakeFluentValidationEngine.DefineRulesAction = () => FakeFluentValidationEngine.TestProperty(vm => vm.Quantity).IsNot.Between(15, 25);
            FakeFluentValidationEngine.BuildRules();

            // Act
            FakeEditableViewModel.Quantity.Value = 20;
            FakeEditableViewModel.SynchronizationTask.Wait();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

    }
}
