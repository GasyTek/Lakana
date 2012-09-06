using System.Windows;
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
        public ILookupViewModelProperty<Country, Country> Country { get; private set; }
        public IEnumViewModelProperty<Rank> Rank { get; private set; }

        protected override void OnCreateViewModelProperties()
        {
            var objectToEdit = Model;

            // HOW TO : create a single valued view model property
            Age = CreateValueProperty(objectToEdit.Age);
            Age.UIMetadata = new UIMetadata
            {
                LabelProvider = () => "Age",
                DescriptionProvider = () => "Description of 'Age' property."
            };

            // HOW TO : create a lookup value view model property
            Country = CreateLookupProperty(objectToEdit.Country, Database.GetCountriesLookupValues);
            Country.UIMetadata = new UIMetadata
            {
                LabelProvider = () => "Country",
                DescriptionProvider = () => "Description of 'Country' property."
            };

            // HOW TO : create an enum view model property
            // NOTE : The lookup items are of type "IEnumItem<Rank>"
            Rank = CreateEnumProperty(objectToEdit.Rank, RankEnumUIMetadataProvider.GetUIMetadata);
            Rank.UIMetadata = new UIMetadata
            {
                LabelProvider = () => "Rank",
                DescriptionProvider = () => "Description of 'Rank' property."
            };
        }

        protected override IValidationEngine OnCreateValidationEngine()
        {
            return null;
        }

        protected override void OnSave()
        {
            MessageBox.Show("Modifications saved !", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        protected override void OnCancel()
        {
            MessageBox.Show("Modifications canceled !", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}