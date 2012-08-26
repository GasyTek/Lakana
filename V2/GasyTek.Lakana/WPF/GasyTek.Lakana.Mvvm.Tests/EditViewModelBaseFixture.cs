using System.Threading;
using GasyTek.Lakana.Mvvm.Tests.Fakes;
using GasyTek.Lakana.Mvvm.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasyTek.Lakana.Mvvm.Tests
{
    [TestClass]
    public class EditViewModelBaseFixture
    {
        private const string FakeEditableViewModelProperty = "FakeEditableViewModelProperty";

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void OnSetup()
        {
            var fakeEditableViewModel = new FakeEditableViewModel();
            fakeEditableViewModel.ValidationEngineProvider = () => new DataAnnotationValidationEngine();
            fakeEditableViewModel.Model = new Product { Code = "PR001", Quantity = 10 };

            TestContext.Properties.Add(FakeEditableViewModelProperty, fakeEditableViewModel);
        }

        #region Helper methods

        private FakeEditableViewModel FakeEditableViewModel
        {
            get { return (FakeEditableViewModel)TestContext.Properties[FakeEditableViewModelProperty]; }
        }

        private void ConfigureThread()
        {
            // assigns a synchronization context to the testing thread
            // to allow task to use TaskScheduler.FromSynchronizationContext()
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

        #endregion

        [TestMethod]
        public void ViewModelHasChangedIfAtLeastOnePropertyHasChanged()
        {
            // Prepare
            ConfigureThread();

            // Act
            FakeEditableViewModel.Code.Value = "new value";
            FakeEditableViewModel.SynchronizationTask.Wait();

            // Verify
            Assert.IsTrue(FakeEditableViewModel.HasChanged);
        }

        [TestMethod]
        public void ViewModelIsValidIfAllPropertiesAreValid()
        {
            // Prepare
            ConfigureThread();

            // Act
            FakeEditableViewModel.Code.Value = null;
            FakeEditableViewModel.SynchronizationTask.Wait();

            // Verify
            Assert.IsFalse(FakeEditableViewModel.IsValid);
        }

    }
}
