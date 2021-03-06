using System;
using GasyTek.Lakana.Mvvm.Validation.Fluent;

namespace Samples.GasyTek.Lakana.Mvvm.Validation.Fluent
{
    /// <summary>
    /// The fluent validation engine for SampleFluentValidationViewModel.
    /// </summary>
    public class MyFluentValidationEngine : FluentValidationEngine<SampleFluentValidationViewModel>
    {
        public MyFluentValidationEngine(SampleFluentValidationViewModel viewModelInstance) 
            : base(viewModelInstance)
        {
        }

        protected override void OnDefineRules()
        {
            // Example of simple Fluent rules

            Property<string>(vm => vm.Code).Is.Required().Otherwise("Code is required.");
            Property<string>(vm => vm.Code).Is.Valid(ViewModelInstance.CodeIsUnique()).Otherwise("Code already exist.");
            Property<string>(vm => vm.Code).Has.MaxLength(3).Otherwise("Code length must not eceed 3 characters.");
            // NOTE : Above rules can be defined in one sentence like below, if you want them to share the same error message
            //Property(vm => vm.Code).Is.Required().And.Has.MaxLength(3).Otherwise("Code is required and its length must not eceed 3 characters.");
            Property<int>(vm => vm.Age).Is.BetweenValues(1, 50).Otherwise("Age must be between 1 and 50.");
            Property<Country>(vm => vm.Country).Is.EqualTo(Database.GetCountry(4)).Otherwise("Country other than Madagascar is not allowed.");
            
            // Example of more complex Fluent rules
            Property<DateTime>(vm => vm.DateOfBirth)
                .Is.GreaterThan(new DateTime(1990, 01, 01))
                .Otherwise("Date of Birth must be after 1990");
            Property<DateTime>(vm => vm.DateOfBirth)
                .Is.LessThanOrEqualTo(vm => vm.DateOfHire)
                .Otherwise("Date of Birth must preced the Date of Hire");
            Property<DateTime>(vm => vm.DateOfHire)
                .Is.BetweenPoperties(vm=>vm.DateOfBirth, vm => vm.DateOfDeath)
                .Otherwise("Date of Hire must be between Date of Birth and Date of Death.");

        }
    }
}