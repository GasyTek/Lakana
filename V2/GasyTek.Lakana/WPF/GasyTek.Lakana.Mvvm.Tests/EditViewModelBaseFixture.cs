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
            _fakeEditableViewModel = new FakeEditableViewModel { Model = new Product { Code = "PR001", Quantity = 10 } };
        }

        [TestMethod]
        public void ViewModelHasChangedIfAtLeastOnePropertyHasChanged()
        {
            // Prepare
            // Act
            _fakeEditableViewModel.Code.Value = "new value";

            // Verify
            Assert.IsTrue(_fakeEditableViewModel.HasChanged);
        }

        [TestMethod]
        public void ViewModelIsValidIfAllPropertiesAreValid()
        {
            // Prepare
            // Act
            _fakeEditableViewModel.Code.Value = null;

            // Verify
            Assert.IsFalse(_fakeEditableViewModel.IsValid);
        }

    }
}
