﻿using System;
using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Mvvm.Validation;
using GasyTek.Lakana.Mvvm.ViewModelProperties;
using GasyTek.Lakana.Mvvm.ViewModels;

namespace Samples.GasyTek.Lakana.Mvvm.Validation.Custom
{
    public class SampleCustomValidationViewModel : EditableViewModelBase<Employee>
    {
        public IValueViewModelProperty<string> Code { get; private set; }
        public IValueViewModelProperty<int> Age { get; private set; }
        public ILookupViewModelProperty<Country, Country> Country { get; private set; }
        public IEnumViewModelProperty<Rank> Rank { get; private set; }
        public IValueViewModelProperty<DateTime> DateOfBirth { get; private set; }
        public IValueViewModelProperty<DateTime> DateOfHire { get; private set; }

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

            Rank = CreateEnumProperty(objectToEdit.Rank, RankEnumUIMetadataProvider.GetUIMetadata);
            Rank.UIMetadata = new UIMetadata
            {
                LabelProvider = () => "Rank",
                DescriptionProvider = () => "Description of 'Rank' property."
            };

            DateOfBirth = CreateValueProperty(objectToEdit.DateOfBirth);
            DateOfBirth.UIMetadata = new UIMetadata
            {
                LabelProvider = () => "Date of Birth",
                DescriptionProvider = () => "Description of 'DateOfBirth' property."
            };

            DateOfHire = CreateValueProperty(objectToEdit.DateOfHire);
            DateOfHire.UIMetadata = new UIMetadata
            {
                LabelProvider = () => "Date of Hire",
                DescriptionProvider = () => "Description of 'DateOfHire' property."
            };
        }

        protected override IValidationEngine OnCreateValidationEngine()
        {
            return new MyCustomValidationEngine(this);
        }

        protected override void OnSave()
        {
            
        }

        protected override void OnCancel()
        {
            
        }
    }
}
