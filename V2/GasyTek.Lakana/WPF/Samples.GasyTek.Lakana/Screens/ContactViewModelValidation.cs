using System;
using GasyTek.Lakana.Mvvm.Validation.Fluent;
using Samples.GasyTek.Lakana.Model;

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
                .Otherwise("First name is required.");

            Property<string>(vm => vm.LastName).Is.Required()
                .Otherwise("Last name is required.");

            Property<DateTime>(vm => vm.DateOfBirth).Is.LessThan(vm => vm.DateOfDeath)
                .Otherwise("Date of birth should be before the date of death.");

            Property<string>(vm => vm.Email).Is.ValidEmail()
                .Otherwise("Email should have the form xxx@yyy.zzz .");

            Property<string>(vm => vm.PhoneNumber).Is.Required()
                .Otherwise("Phone number is required.");
            Property<string>(vm => vm.PhoneNumber).Has.MaxLength(4)
                .Otherwise("Phone number should not exceed 4 characters.");
            Property<string>(vm => vm.PhoneNumber).Is.Valid((phoneNumber, token) => WebService.IsPhoneUnique(phoneNumber))
                .Otherwise("Phone number already exist.");
            
        }
    }
}