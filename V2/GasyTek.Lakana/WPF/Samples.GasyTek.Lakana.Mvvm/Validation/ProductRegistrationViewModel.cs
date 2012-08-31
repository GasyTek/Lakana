using System;
using System.ComponentModel.DataAnnotations;
using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Mvvm.Validation;
using GasyTek.Lakana.Mvvm.ViewModelProperties;
using GasyTek.Lakana.Mvvm.ViewModels;

namespace Samples.GasyTek.Lakana.Mvvm.Validation
{
    //public class ProductRegistrationViewModel : EditableViewModelBase<Model>
    //{
    //    [Required]
    //    public IViewModelProperty Code { get; private set; }

    //    [StringLength(3, ErrorMessage = "Name length must be less than 3")]
    //    [RegularExpression(ValidationConstants.EmailPattern, ErrorMessage = "Email is not valid")]
    //    public IViewModelProperty Name { get; private set; }
        
    //    public IViewModelProperty BeginDistributionDate { get; private set; }

    //    public IViewModelProperty EndDistributionDate { get; private set; }
    //    public IViewModelProperty Category { get; private set; }

    //    protected override void OnCreateViewModelProperties()
    //    {
    //        var product = Model;

    //        // Code property
    //        Code = CreateValueProperty(product.Code);
    //        Code.UIMetadata = new UIMetadata { LabelProvider = () => "Code"};
            
    //        // Name property
    //        Name = CreateValueProperty(product.Name);
    //        Name.UIMetadata = new UIMetadata { LabelProvider = () => "Name" };

    //        // BeginDistributionDate property
    //        BeginDistributionDate = CreateValueProperty(product.BeginDistributionDate);
    //        BeginDistributionDate.UIMetadata = new UIMetadata { LabelProvider = () => "Begin Date" };

    //        // EndDistributionDate property
    //        EndDistributionDate = CreateValueProperty(product.EndDistributionDate);
    //        EndDistributionDate.UIMetadata = new UIMetadata { LabelProvider = () => "End Date" };

    //        //// Category property
    //        //Category = CreateEnumProperty(product.Category, null);
    //    }

    //    //protected override void OnDefineValidationRules()
    //    //{
    //    //    //ForProperty(vm => vm.Code).
    //    //    //    DefineRule(p => String.IsNullOrEmpty(p.Value<string>()) == false).
    //    //    //    WhichFailsWithMessage("Code is required");

    //    //    //ForProperty(vm => vm.Name).
    //    //    //    DefineRule(p => p.Value<string>().Length < 4).
    //    //    //    WhichFailsWithMessage("Length must not be greater than 4");

    //    //    //ForProperties(vm => vm.Code, vm => vm.Name).
    //    //    //    DefineRule(pvm => pvm.Code.Value<string>() != pvm.Name.Value<string>()).
    //    //    //    WhichFailsWithMessage("Please specify a different Name and Code").
    //    //    //    And.
    //    //    //    DefineRule(pvm => pvm.Code.Value<string>() != "abc" && pvm.Name.Value<string>() != "abc").
    //    //    //    WhichFailsWithMessage("Code and Name have to be different from ABC");

    //    //    //ForProperties(vm => vm.Code, vm => vm.Name).
    //    //    //    DefineRule(pvm => pvm.Code.Value<string>() != pvm.Name.Value<string>()).
    //    //    //    WhichFailsWithMessage("Please specify a different Name and Code");

    //    //    //ForProperties(vm => vm.BeginDistributionDate, vm => vm.EndDistributionDate).
    //    //    //    DefineRule(vm => vm.BeginDistributionDate.Value<DateTime>() < vm.EndDistributionDate.Value<DateTime>()).
    //    //    //    WhichFailsWithMessage("Set an end date that is greater than begin date").
    //    //    //    And.
    //    //    //    DefineRule(
    //    //    //        vm =>
    //    //    //        vm.BeginDistributionDate.Value<DateTime>() < DateTime.Now.AddDays(-1) &&
    //    //    //        vm.EndDistributionDate.Value<DateTime>() > DateTime.Now.AddDays(1)).
    //    //    //    WhichFailsWithMessage("Begin and End date must comprised between TODAY -1 and TODAY + 1");
    //    //}


    //    protected override IValidationEngine OnCreateValidationEngine()
    //    {
    //        //return new CustomValidationEngine();
    //        return new DataAnnotationValidationEngine();
    //    }

    //    protected override void OnSave()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    protected override void OnCancel()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
