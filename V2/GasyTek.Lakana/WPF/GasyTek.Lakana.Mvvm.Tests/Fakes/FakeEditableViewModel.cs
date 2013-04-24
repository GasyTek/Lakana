using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using GasyTek.Lakana.Mvvm.Validation;
using GasyTek.Lakana.Mvvm.ViewModelProperties;
using GasyTek.Lakana.Mvvm.ViewModels;

namespace GasyTek.Lakana.Mvvm.Tests.Fakes
{
    class FakeEditableViewModel : EditableViewModelBase<Product>
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
        private CountdownEvent SyncObject { get; set; }

        public Func<IValidationEngine> ValidationEngineProvider { get; set; } 

        public FakeEditableViewModel()
        {
            SyncObject = new CountdownEvent(1);
        }

        /// <summary>
        /// Configure the view model to accept exactly the given number of property assignement that triggers validation.
        /// </summary>
        /// <param name="nbPropertyValidation">The nb property assignement.</param>
        public void ConfigureExpectedNumberOfPropertyValidation(int nbPropertyValidation)
        {
            SyncObject.Reset(nbPropertyValidation);
        }

        /// <summary>
        /// Synchronizes asynchronous validation.
        /// This will block until all validations did not terminate.
        /// </summary>
        public void WaitForPropertyValidationsToTerminate()
        {
            SyncObject.Wait();
        }

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
            var engine = validationEngine as FakeFluentValidationEngine;
            if (engine != null)
            {
                engine.ValidationTerminated += (sender, args) => SyncObject.Signal();
            }
            else
            {
                validationEngine.ErrorsChangedEvent += (sender, args) => SyncObject.Signal();
            }

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
