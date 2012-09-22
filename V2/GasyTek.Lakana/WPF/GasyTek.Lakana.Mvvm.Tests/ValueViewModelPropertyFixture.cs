using System;
using System.Threading;
using GasyTek.Lakana.Mvvm.ViewModelProperties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasyTek.Lakana.Mvvm.Tests
{
    [TestClass]
    public class ValueViewModelPropertyFixture
    {
        [TestMethod]
        public void HasChangedIsSupported()
        {
            // Prepare
            var valueViewModelProperty = new ValueViewModelProperty<int>(1, new ObservableValidationEngine());

            // Act
            valueViewModelProperty.Value = 2;

            // Verify
            Assert.IsTrue(valueViewModelProperty.HasChanged);
        }

        [TestMethod]
        public void NullSameAsEmptyForStrings()
        {
            // Prepare
            var valueViewModelProperty = new ValueViewModelProperty<string>(null, new ObservableValidationEngine());

            // Act
            valueViewModelProperty.Value = String.Empty;

            // Verify
            Assert.IsFalse(valueViewModelProperty.HasChanged);
        }
    }
}
