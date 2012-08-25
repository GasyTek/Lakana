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
        private FakeFluentValidationEngine _fluentValidationEngine;
        private FakeEditableViewModel _fakeEditableViewModel;

        [TestInitialize]
        public void OnSetup()
        {
            _fakeEditableViewModel = new FakeEditableViewModel ();
            _fluentValidationEngine = new FakeFluentValidationEngine(_fakeEditableViewModel);
            _fakeEditableViewModel.ValidationEngineProvider = () => _fluentValidationEngine;
            _fakeEditableViewModel.Model = new Product();
        }

        #region Helper methods

        private void ConfigureThread()
        {
            // assigns a synchronization context to the testing thread
            // to allow task to use TaskScheduler.FromSynchronizationContext()
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

        private void VerifyThatRuleIsBroken(string propertyName)
        {
            // if this is satisfied, that means that rules were broken
            Assert.IsFalse(_fluentValidationEngine.IsValid(propertyName));
        }

        #endregion

        [TestMethod]
        [ExpectedException(typeof(RuleDefinitionException))]
        public void CanDetectIncompleteRules()
        {
            // Prepare
            _fluentValidationEngine.DefineRulesAction = () => _fluentValidationEngine.RequiresThat2(vm => vm.Quantity);    // incomplete rule

            // Act
            _fluentValidationEngine.BuildRules();

            // Verify
        }

        [TestMethod]
        public void CanDefineManyRulesOnOneProperty()
        {
            ConfigureThread();

            // Prepare
            var propertyName = _fakeEditableViewModel.Quantity.PropertyMetadata.Name;
            _fluentValidationEngine.DefineRulesAction = 
                () =>
                    {
                        _fluentValidationEngine.RequiresThat2(vm => vm.Quantity).Is.GreaterThan(20);
                        _fluentValidationEngine.RequiresThat2(vm => vm.Quantity).Is.EqualTo(40);
                    };
            _fluentValidationEngine.BuildRules(); 

            // Act
            _fakeEditableViewModel.Quantity.Value = 15;
            _fluentValidationEngine.SynchronizationTask.Wait();

            // Verify
            // if this is satisfied, that means that rules were broken
            Assert.IsNotNull(_fluentValidationEngine.GetErrors(propertyName));
            Assert.IsTrue(_fluentValidationEngine.GetErrors(propertyName).Count() == 2);
        }

        [TestMethod]
        public void CanBreakSimpleRule()
        {
            ConfigureThread();

            // Prepare
            var propertyName = _fakeEditableViewModel.PurchasingPrice.PropertyMetadata.Name;
            _fluentValidationEngine.DefineRulesAction = () => _fluentValidationEngine.RequiresThat2(vm => vm.PurchasingPrice).Is.GreaterThan(20);
            _fluentValidationEngine.BuildRules();

            // Act
            _fakeEditableViewModel.PurchasingPrice.Value = 20;
            _fluentValidationEngine.SynchronizationTask.Wait();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakSimpleRuleWithCustomMessage()
        {
            ConfigureThread();

            // Prepare
            const string erroMessage = "PurchasingPrice must be greater that 20";
            var propertyName = _fakeEditableViewModel.PurchasingPrice.PropertyMetadata.Name;
            _fluentValidationEngine.DefineRulesAction = () => _fluentValidationEngine.RequiresThat2(vm => vm.PurchasingPrice)
                .Is.GreaterThan(20).Otherwise(erroMessage);
            _fluentValidationEngine.BuildRules();

            // Act
            _fakeEditableViewModel.PurchasingPrice.Value = 20;
            _fluentValidationEngine.SynchronizationTask.Wait();

            // Verify
            Assert.IsNotNull(_fluentValidationEngine.GetErrors(propertyName));
            Assert.IsTrue(_fluentValidationEngine.GetErrors(propertyName).Any());
            Assert.AreEqual(erroMessage, _fluentValidationEngine.GetErrors(propertyName).ToList() [0]);
        }

        [TestMethod]
        public void CanBreakSimpleRuleThatComparesTwoProperties()
        {
            ConfigureThread();

            // Prepare
            var propertyName = _fakeEditableViewModel.PurchasingPrice.PropertyMetadata.Name;
            _fluentValidationEngine.DefineRulesAction = () => _fluentValidationEngine.RequiresThat2(vm => vm.PurchasingPrice).Is.LessThan(vm => vm.SellingPrice);
            _fluentValidationEngine.BuildRules();

            // Act
            _fakeEditableViewModel.PurchasingPrice.Value = 20;
            _fakeEditableViewModel.SellingPrice.Value = 19;
            _fluentValidationEngine.SynchronizationTask.Wait();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakRuleWithAndConjunction()
        {
            ConfigureThread();

            // Prepare
            var propertyName = _fakeEditableViewModel.PurchasingPrice.PropertyMetadata.Name;
            _fluentValidationEngine.DefineRulesAction = () => _fluentValidationEngine.RequiresThat2(vm => vm.PurchasingPrice)
                .Is.LessThan(vm => vm.SellingPrice).And.Is.EqualTo(20);
            _fluentValidationEngine.BuildRules();

            // Act
            _fakeEditableViewModel.PurchasingPrice.Value = 19;
            _fakeEditableViewModel.SellingPrice.Value = 21;
            _fluentValidationEngine.SynchronizationTask.Wait();
            
            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakRuleWithOrConjunction()
        {
            ConfigureThread();

            // Prepare
            var propertyName = _fakeEditableViewModel.Quantity.PropertyMetadata.Name;
            _fluentValidationEngine.DefineRulesAction = () => _fluentValidationEngine.RequiresThat2(vm => vm.Quantity)
                .Is.EqualTo(20).Or.Is.EqualTo(30);
            _fluentValidationEngine.BuildRules();

            // Act
            _fakeEditableViewModel.Quantity.Value = 25;
            _fluentValidationEngine.SynchronizationTask.Wait();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakRuleThatUsesIsNotOperator()
        {
            ConfigureThread();

            // Prepare
            var propertyName = _fakeEditableViewModel.Quantity.PropertyMetadata.Name;
            _fluentValidationEngine.DefineRulesAction = () => _fluentValidationEngine.RequiresThat2(vm => vm.Quantity).IsNot.EqualTo(20);
            _fluentValidationEngine.BuildRules();

            // Act
            _fakeEditableViewModel.Quantity.Value = 20;
            _fluentValidationEngine.SynchronizationTask.Wait();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakRuleThatUsesBetweenOperator()
        {
            ConfigureThread();

            // Prepare
            var propertyName = _fakeEditableViewModel.Quantity.PropertyMetadata.Name;
            _fluentValidationEngine.DefineRulesAction = () => _fluentValidationEngine.RequiresThat2(vm => vm.Quantity).Is.Between(15, 25);
            _fluentValidationEngine.BuildRules();

            // Act
            _fakeEditableViewModel.Quantity.Value = 30;
            _fluentValidationEngine.SynchronizationTask.Wait();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakRuleThatUsesBetweenPropertiesOperator()
        {
            ConfigureThread();

            // Prepare
            var propertyName = _fakeEditableViewModel.Quantity.PropertyMetadata.Name;
            _fluentValidationEngine.DefineRulesAction = () => _fluentValidationEngine.RequiresThat2(vm => vm.Quantity)
                .Is.BetweenPoperties(vm => vm.PurchasingPrice, vm => vm.SellingPrice);
            _fluentValidationEngine.BuildRules();

            // Act
            _fakeEditableViewModel.Quantity.Value = 30;
            _fakeEditableViewModel.PurchasingPrice.Value = 10;
            _fakeEditableViewModel.SellingPrice.Value = 20;
            _fluentValidationEngine.SynchronizationTask.Wait();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

        [TestMethod]
        public void CanBreakRuleThatUsesIsNotBetweenOperator()
        {
            ConfigureThread();

            // Prepare
            var propertyName = _fakeEditableViewModel.Quantity.PropertyMetadata.Name;
            _fluentValidationEngine.DefineRulesAction = () => _fluentValidationEngine.RequiresThat2(vm => vm.Quantity).IsNot.Between(15, 25);
            _fluentValidationEngine.BuildRules();

            // Act
            _fakeEditableViewModel.Quantity.Value = 20;
            _fluentValidationEngine.SynchronizationTask.Wait();

            // Verify
            VerifyThatRuleIsBroken(propertyName);
        }

    }
}
