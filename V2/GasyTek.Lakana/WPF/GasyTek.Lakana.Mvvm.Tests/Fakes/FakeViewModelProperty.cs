using System;
using GasyTek.Lakana.Mvvm.ViewModelProperties;

namespace GasyTek.Lakana.Mvvm.Tests.Fakes
{
    /// <summary>
    /// A fake view model property for tests.
    /// </summary>
    class FakeViewModelProperty : ViewModelProperty
    {
        private readonly object _value;

        public FakeViewModelProperty(object value)
            : base(new ObservableValidationEngine())
        {
            _value = value;
        }

        protected override void OnNotifyValueProperty()
        {
        }

        protected override object OnGetValue()
        {
            return _value;
        }

        protected override Type OnGetValueType()
        {
            return _value.GetType();
        }

        protected override bool OnAskHasChanged()
        {
            return false;
        }
    }
}
