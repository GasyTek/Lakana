using GasyTek.Lakana.Mvvm.Validation;
using GasyTek.Lakana.Mvvm.ViewModelProperties;
using GasyTek.Lakana.Mvvm.ViewModels;
using GasyTek.Lakana.Navigation.Services;
using Samples.GasyTek.Lakana.Model;
using Samples.GasyTek.Lakana.Utils;

namespace Samples.GasyTek.Lakana.Screens
{
    public class ContactViewModel : EditableViewModelBase<Contact>, IViewKeyAware
    {
        public string ViewKey { get; set; }
        public IViewModelProperty FirstName { get; set; }
        public IViewModelProperty LastName { get; set; }
        public IViewModelProperty PhoneNumber { get; set; }
        public IViewModelProperty Sex { get; set; }
        public IViewModelProperty DateOfBirth { get; set; }
        public IViewModelProperty DateOfDeath { get; set; }
        public IViewModelProperty Email { get; set; }

        protected override void OnCreateViewModelProperties()
        {
            FirstName = CreateValueProperty(Model.FirstName);
            LastName = CreateValueProperty(Model.LastName);
            PhoneNumber = CreateValueProperty(Model.PhoneNumber);
            Sex = CreateEnumProperty(Model.Sex);
            DateOfBirth = CreateValueProperty(Model.DateOfBirth);
            DateOfDeath = CreateValueProperty(Model.DateOfDeath);
            Email = CreateValueProperty(Model.Email);
        }

        protected override IValidationEngine OnCreateValidationEngine()
        {
            return new ContactViewModelValidation(this);
        }

        protected override void OnSave()
        {
            Singletons.NavigationServiceInstance.Close(ViewKey);
        }

        protected override void OnCancel()
        {
            Singletons.NavigationServiceInstance.Close(ViewKey);
        }
    }
}