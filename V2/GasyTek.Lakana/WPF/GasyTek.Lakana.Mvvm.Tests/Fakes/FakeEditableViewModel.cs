using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
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

        /// <summary>
        /// Used by the unit testing framework to wait for a result of 
        /// an asynchronous operation before testing the result.
        /// </summary>
        public Task SynchronizationTask { get; private set; }

        public Func<IValidationEngine> ValidationEngineProvider { get; set; } 

        public FakeEditableViewModel()
        {
            SynchronizationTask = new Task(DummyAction);
        }

        private void DummyAction()
        {}

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
            var validationEngine = ValidationEngineProvider();
            validationEngine.ErrorsChangedEvent += (sender, args) =>
                                                       {
                                                           // surrounded by try/catch to avoid the error that appears
                                                           // when trying to start a task that alread started.
                                                           try { SynchronizationTask.Start(); }
                                                           catch { }   
                                                       };
            return validationEngine;
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
