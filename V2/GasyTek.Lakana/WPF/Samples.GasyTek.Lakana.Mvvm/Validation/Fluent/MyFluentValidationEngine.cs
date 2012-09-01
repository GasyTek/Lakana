using GasyTek.Lakana.Mvvm.Validation.Fluent;

namespace Samples.GasyTek.Lakana.Mvvm.Validation.Fluent
{
    public class MyFluentValidationEngine : FluentValidationEngine<SampleFluentValidationViewModel>
    {
        public MyFluentValidationEngine(SampleFluentValidationViewModel viewModelInstance) 
            : base(viewModelInstance)
        {
        }

        protected override void DefineRules()
        {
            // Example of simple Fluent rules

            Property(vm => vm.Code).Is.Required().Otherwise("Code is required");
            Property(vm => vm.Code).Has.MaxLength(3).Otherwise("Code length must not eceed 3 characters.");
            // NOTE : Above rules can be defined in one sentence like below, if you want them to share the same error message
            //Property(vm => vm.Code).Is.Required().And.Has.MaxLength(3).Otherwise("Code is required and its length must not eceed 3 characters.");
            Property(vm => vm.Age).Is.Between(1, 50).Otherwise("Age must be between 1 and 50.");
            Property(vm => vm.Country).Is.EqualTo(Database.GetCountry(4)).Otherwise("Country other than Madagascar is not allowed.");

        }
    }
}