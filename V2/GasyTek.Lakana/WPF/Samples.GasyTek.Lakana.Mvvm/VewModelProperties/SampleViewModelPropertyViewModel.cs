using System.Collections.Generic;
using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Mvvm.Validation;
using GasyTek.Lakana.Mvvm.ViewModelProperties;
using GasyTek.Lakana.Mvvm.ViewModels;
using Samples.GasyTek.Lakana.Mvvm.Resources;

namespace Samples.GasyTek.Lakana.Mvvm.VewModelProperties
{
    public class SampleViewModelPropertyViewModel : EditableViewModelBase<Employee>
    {
        public IValueViewModelProperty<int> Age { get; private set; }
        public ILookupViewModelProperty<int, Country> Country { get; private set; }
        public IEnumViewModelProperty<Rank> Rank { get; private set; }

        protected override void OnCreateViewModelProperties()
        {
            var objectToEdit = Model;

            // HOW TO : create a single valued view model property
            Age = CreateValueProperty(objectToEdit.Age);
            Age.UIMetadata = new UIMetadata
            {
                LabelProvider = () => "Age",
                DescriptionProvider = () => "Description of Age property."
            };

            // HOW TO : create a lookup value view model property
            Country = CreateLookupProperty(objectToEdit.CountryId, GetCountries);
            Country.UIMetadata = new UIMetadata
            {
                LabelProvider = () => "Country",
                DescriptionProvider = () => "Description of Country property."
            };

            // HOW TO : create an enum view model property
            // You have to specify the resource that contains the translation for enum members.
            // NOTE : The lookup items are of type "IEnumItem<Rank>"
            Rank = CreateEnumProperty(objectToEdit.Rank, typeof(Labels));
            Rank.UIMetadata = new UIMetadata
            {
                LabelProvider = () => "Rank",
                DescriptionProvider = () => "Description of Rank property."
            };
        }

        protected override IValidationEngine OnCreateValidationEngine()
        {
            return null;
        }

        protected override void OnSave()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnCancel()
        {
            throw new System.NotImplementedException();
        }

        private List<Country> GetCountries()
        {
            return new List<Country>
                       {
                           new Country {Id = 1,Name = "France"},
                           new Country {Id = 2,Name = "USA"},
                           new Country {Id = 3,Name = "China"},
                           new Country {Id = 4,Name = "Madagascar"},
                       };
        }
    }
}