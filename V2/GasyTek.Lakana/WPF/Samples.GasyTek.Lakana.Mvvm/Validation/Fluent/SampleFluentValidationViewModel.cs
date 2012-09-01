using System;
using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Mvvm.Validation;
using GasyTek.Lakana.Mvvm.ViewModelProperties;
using GasyTek.Lakana.Mvvm.ViewModels;
using Samples.GasyTek.Lakana.Mvvm.Resources;

namespace Samples.GasyTek.Lakana.Mvvm.Validation.Fluent
{
    public class SampleFluentValidationViewModel : EditableViewModelBase<Employee>
    {
        public IValueViewModelProperty<string> Code { get; private set; }
        public IValueViewModelProperty<int> Age { get; private set; }
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
            return new MyFluentValidationEngine(this);
        }

        protected override void OnSave()
        {
        }

        protected override void OnCancel()
        {
        }
    }
}