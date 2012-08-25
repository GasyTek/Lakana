using System;
using System.ComponentModel.DataAnnotations;
using GasyTek.Lakana.Mvvm.Validation;
using GasyTek.Lakana.Mvvm.ViewModelProperties;
using GasyTek.Lakana.Mvvm.ViewModels;

namespace GasyTek.Lakana.Mvvm.Tests.Fakes
{
    class FakeEditableViewModel : EditViewModelBase<FakeEditableViewModel, Product>
    {
        [Required]
        public IValueViewModelProperty<string> Code { get; private set; }
        public IValueViewModelProperty<int> Quantity { get; private set; }
        public IValueViewModelProperty<int> PurchasingPrice { get; set; }
        public IValueViewModelProperty<int> SellingPrice { get; set; }
        public IValueViewModelProperty<string> SellerEmail { get; set; }
        
        public FakeEditableViewModel()
        {
            ValidationEngine = new DataAnnotationValidationEngine();
        }

        public void AssignValidationEngine(IValidationEngine validationEngine)
        {
            ValidationEngine = validationEngine;
        }

        protected override void OnCreateProperties()
        {
            Code = CreateValueProperty(Model.Code);
            Quantity = CreateValueProperty(Model.Quantity);
            PurchasingPrice = CreateValueProperty(Model.PurchasingPrice);
            SellingPrice = CreateValueProperty(Model.SellingPrice);
            SellerEmail = CreateValueProperty(Model.SellerEmail);
        }

        protected override void OnSave()
        {
            throw new NotImplementedException();
        }

        protected override void OnCancel()
        {
            throw new NotImplementedException();
        }
    }
}
