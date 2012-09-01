using System.ComponentModel.DataAnnotations;
using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Mvvm.Validation;
using GasyTek.Lakana.Mvvm.ViewModelProperties;
using GasyTek.Lakana.Mvvm.ViewModels;
using Samples.GasyTek.Lakana.Mvvm.Resources;

namespace Samples.GasyTek.Lakana.Mvvm.Validation.DataAnnotation
{
    public class SampleDataAnnotationValidationViewModel : EditableViewModelBase<Employee>
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Labels), ErrorMessageResourceName = "CodeIsRequired")]
        [StringLength(3, ErrorMessageResourceType = typeof(Labels), ErrorMessageResourceName = "CodeMustNotExceed3Characters")]
        public IValueViewModelProperty<string> Code { get; private set; }

        [Range(1, 50, ErrorMessageResourceType = typeof(Labels), ErrorMessageResourceName = "AgeMustBeBetween1and50")]
        public IValueViewModelProperty<int> Age { get; private set; }

        [CustomValidation(typeof(CountryValidator), "ValidateCountry")]
        public ILookupViewModelProperty<Country, Country> Country { get; private set; }

        public IEnumViewModelProperty<Rank> Rank { get; private set; }

        protected override void OnCreateViewModelProperties()
        {
            var objectToEdit = Model;

            Code = CreateValueProperty(objectToEdit.Code);
            Code.UIMetadata = new UIMetadata
            {
                LabelProvider = () => "Code",
                DescriptionProvider = () => "Description of 'Code' property."
            };

            Age = CreateValueProperty(objectToEdit.Age);
            Age.UIMetadata = new UIMetadata
            {
                LabelProvider = () => "Age",
                DescriptionProvider = () => "Description of 'Age' property."
            };

            Country = CreateLookupProperty(objectToEdit.Country, Database.GetCountriesLookupValues);
            Country.UIMetadata = new UIMetadata
            {
                LabelProvider = () => "Country",
                DescriptionProvider = () => "Description of 'Country' property."
            };

            Rank = CreateEnumProperty(objectToEdit.Rank, typeof(Labels));
            Rank.UIMetadata = new UIMetadata
            {
                LabelProvider = () => "Rank",
                DescriptionProvider = () => "Description of 'Rank' property."
            };
        }

        protected override IValidationEngine OnCreateValidationEngine()
        {
            return new DataAnnotationValidationEngine();
        }

        protected override void OnSave()
        {
            
        }

        protected override void OnCancel()
        {
            
        }
    }
}
