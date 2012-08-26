using System.Threading;
using GasyTek.Lakana.Mvvm.Tests.Fakes;
using GasyTek.Lakana.Mvvm.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasyTek.Lakana.Mvvm.Tests
{
    [TestClass]
    public class EditViewModelBaseFixture
    {
        private FakeEditableViewModel _fakeEditableViewModel;

        [TestInitialize]
        public void OnSetup()
        {
            _fakeEditableViewModel = new FakeEditableViewModel();
            _fakeEditableViewModel.ValidationEngineProvider = () => new DataAnnotationValidationEngine();
            _fakeEditableViewModel.Model = new Product {Code = "PR001", Quantity = 10};
        }

        private void ConfigureThread()
        {
            // assigns a synchronization context to the testing thread
            // to allow task to use TaskScheduler.FromSynchronizationContext()
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

        [TestMethod]
        public void ViewModelHasChangedIfAtLeastOnePropertyHasChanged()
        {
            // Prepare
            ConfigureThread();

            // Act
            _fakeEditableViewModel.Code.Value = "new value";
            _fakeEditableViewModel.SynchronizationTask.Wait();

            // Verify
            Assert.IsTrue(_fakeEditableViewModel.HasChanged);
        }

        [TestMethod]
        public void ViewModelIsValidIfAllPropertiesAreValid()
        {
            // Prepare
            ConfigureThread();

            // Act
            _fakeEditableViewModel.Code.Value = null;
            _fakeEditableViewModel.SynchronizationTask.Wait();

            // Verify
            Assert.IsFalse(_fakeEditableViewModel.IsValid);
        }

    }
}
