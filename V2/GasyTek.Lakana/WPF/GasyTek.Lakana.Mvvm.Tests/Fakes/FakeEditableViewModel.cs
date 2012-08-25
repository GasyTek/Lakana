using System;
using System.ComponentModel.DataAnnotations;
using GasyTek.Lakana.Mvvm.Validation;
using GasyTek.Lakana.Mvvm.ViewModelProperties;
using GasyTek.Lakana.Mvvm.ViewModels;

namespace GasyTek.Lakana.Mvvm.Tests.Fakes
{
    class FakeEditableViewModel : EditViewModelBase<Product>
    {
        [Required]
        public IValueViewModelProperty<string> Code { get; private set; }
        public IValueViewModelProperty<int> Quantity { get; private set; }
        public IValueViewModelProperty<int> PurchasingPrice { get; set; }
        public IValueViewModelProperty<int> SellingPrice { get; set; }
        public IValueViewModelProperty<string> SellerEmail { get; set; }

        public Func<IValidationEngine> ValidationEngineProvider { get; set; } 

        protected override void OnCreateViewModelProperties()
        {
            Code = CreateValueProperty(Model.Code);
            Quantity = CreateValueProperty(Model.Quantity);
            PurchasingPrice = CreateValueProperty(Model.PurchasingPrice);
            SellingPrice = CreateValueProperty(Model.SellingPrice);
            SellerEmail = CreateValueProperty(Model.SellerEmail);
        }

        protected override IValidationEngine OnCreateValidationEngine()
        {
            if (ValidationEngineProvider == null) return null;
            return ValidationEngineProvider();
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
