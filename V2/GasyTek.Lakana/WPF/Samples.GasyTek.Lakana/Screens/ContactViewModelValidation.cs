using System;
using GasyTek.Lakana.Mvvm.Validation.Fluent;
using Samples.GasyTek.Lakana.Model;
using Samples.GasyTek.Lakana.Resources;

namespace Samples.GasyTek.Lakana.Screens
{
    internal class ContactViewModelValidation : FluentValidationEngine<ContactViewModel>
    {
        public ContactViewModelValidation(ContactViewModel viewModelInstance) 
            : base(viewModelInstance)
        {
        }

        protected override void OnDefineRules()
        {
            Property<string>(vm => vm.FirstName).Is.Required()
                .Otherwise(Texts.ValidationFirstNameRequired);

            Property<string>(vm => vm.LastName).Is.Required()
                .Otherwise(Texts.ValidationLastNameRequired);

            Property<DateTime>(vm => vm.DateOfBirth).Is.LessThan(vm => vm.DateOfDeath)
                .Otherwise(Texts.ValidationDateOfBirthBeforeDateOfDeath);

            Property<string>(vm => vm.Email).Is.ValidEmail()
                .Otherwise(Texts.ValidationEmailFormat);

            Property<string>(vm => vm.PhoneNumber).Is.Required()
                .Otherwise(Texts.ValidationPhoneRequired);
            Property<string>(vm => vm.PhoneNumber).Has.MaxLength(4)
                .Otherwise(Texts.ValidationPhoneLength);
            Property<string>(vm => vm.PhoneNumber).Is.Valid((phoneNumber, token) => WebService.IsPhoneUnique(phoneNumber))
                .Otherwise(Texts.ValidationPhoneExists);
            
        }
    }
}