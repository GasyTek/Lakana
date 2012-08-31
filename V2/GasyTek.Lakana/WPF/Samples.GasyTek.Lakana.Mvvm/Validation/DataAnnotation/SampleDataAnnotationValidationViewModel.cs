using System;
using GasyTek.Lakana.Mvvm.Validation;
using GasyTek.Lakana.Mvvm.ViewModels;

namespace Samples.GasyTek.Lakana.Mvvm.Validation.DataAnnotation
{
    public class SampleDataAnnotationValidationViewModel : EditableViewModelBase<Employee>
    {
        protected override void OnCreateViewModelProperties()
        {
            throw new NotImplementedException();
        }

        protected override IValidationEngine OnCreateValidationEngine()
        {
            throw new NotImplementedException();
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
