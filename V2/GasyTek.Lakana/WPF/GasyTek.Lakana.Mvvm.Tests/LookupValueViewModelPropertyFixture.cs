using System;
using GasyTek.Lakana.Mvvm.ViewModelProperties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasyTek.Lakana.Mvvm.Tests
{
    [TestClass]
    public class LookupValueViewModelPropertyFixture
    {
        [TestMethod]
        public void HasChangedIsSupported()
        {
            // Prepare
            var items = new[] { "1", "2", "3", "4" };
            var lookupViewModelProperty = new LookupViewModelProperty<string, string>("1", () => items, new ObservableValidationEngine());

            // Act
            lookupViewModelProperty.SelectedValue = "2";

            // Verify
            Assert.IsTrue(lookupViewModelProperty.HasChanged);
        }

        [TestMethod]
        public void NullSameAsEmptyForStrings()
        {
            // Prepare
            var items = new[] { "1", "2", "3", "4" };
            var valueViewModelProperty = new LookupViewModelProperty<string, string>(null, () => items, new ObservableValidationEngine());

            // Act
            valueViewModelProperty.SelectedValue = String.Empty;

            // Verify
            Assert.IsFalse(valueViewModelProperty.HasChanged);
        }
    }
}