using System;
using GasyTek.Lakana.Mvvm.Validation;
using GasyTek.Lakana.Mvvm.ViewModels;

namespace Samples.GasyTek.Lakana.Mvvm.Validation.Custom
{
    public class SampleCustomValidationViewModel : EditableViewModelBase<Employee>
    {
        protected override void OnCreateViewModelProperties()
        {
            throw new NotImplementedException();
        }

        protected override IValidationEngine OnCreateValidationEngine()
        {
            return new CustomValidationEngine();
        }

        protected override void OnSave()
        {
            throw new NotImplementedException();
        }

        protected override void OnCancel()
        {
            throw new NotImplementedException();
        }
    }
}
