using GasyTek.Lakana.Mvvm.Validation;
using GasyTek.Lakana.Mvvm.ViewModels;
using Samples.GasyTek.Lakana.Model;

namespace Samples.GasyTek.Lakana.Screens
{
    public class ContactViewModel : EditableViewModelBase<Contact>
    {
        protected override void OnCreateViewModelProperties()
        {
            
        }

        protected override IValidationEngine OnCreateValidationEngine()
        {
            return null;
        }

        protected override void OnSave()
        {
            
        }

        protected override void OnCancel()
        {
            
        }
    }
}